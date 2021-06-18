using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CoreCard.Tesla.NetworkInterface.Communication;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.Constants;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;
using CoreCard.Tesla.NetworkInterface.MasterCard.Exceptions;
using CoreCard.Tesla.NetworkInterface.MasterCard.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreCard.Tesla.NetworkInterface.MasterCard
{
    public class NetworkChannel : INetworkChannel
    {
        #region Private Fields

        private static volatile ConcurrentDictionary<string, TaskCompletionSource<IIsoResponse>> _messageList;
        private readonly IIsoMessageFactory _isoMessageFactory;
        private readonly ILogger<NetworkChannel> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IIsoMessageStream _messageStream;
        private readonly MasterCardNetworkSettings _networkSettings;
        private readonly ISocketClientFactory _socketClientFactory;
        private readonly StanGenerator _stanGenerator;
        private int _connectionStarted;

        private System.Timers.Timer _echoTimer;
        private bool _isConnected;
        private bool _isLoggedIn;
        private bool _shutdown;
        private ISocketClient _tcpClient;

        #endregion Private Fields

        public NetworkChannel(ILoggerFactory loggerFactory, StanGenerator stanGenerator, IOptions<MasterCardNetworkSettings> networkSettings, IIsoMessageFactory isoMessageFactory, IIsoMessageStreamFactory isoMessageStreamFactory, ISocketClientFactory socketClientFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<NetworkChannel>();
            _isoMessageFactory = isoMessageFactory;
            _messageStream = isoMessageStreamFactory.GetIsoMessageStream(loggerFactory, isoMessageFactory);
            _networkSettings = networkSettings.Value;
            _stanGenerator = stanGenerator;
            _socketClientFactory = socketClientFactory;
            _messageList = new ConcurrentDictionary<string, TaskCompletionSource<IIsoResponse>>();

            var request = "30323030FEFF4401A8E1E40A00000000000000043136353334323834303030303037303031363039303030303030303030303030323639393030303030303030323639393030303030303030323639393035323630323139303936313030303030303631303030303030303030303031303231393039303532363138303130353236303532363539343639303230393732383338343030383130393030303030303238333337353334323834303030303037303031363D31383031313031303130383134313537333030303333303030303130303030314D54462054455354414243313233544553544D5446313953544C2070686F746F202020202020202020202020202053742E204C6F7569732020202020204D4F30313052363130353030303031383430383430383430303230303034303834304430303030303030303130303030323630303030303034303030323030383430363333383530303030303031324D5331303030303036363037303530303030202020202020202020204E583132334142434445464142434445464748494A4B4C4D4E4F505152535455565758595A";

            request = "30323030FEFF4641A8E1F20A000000000000000431363533343238343030303030373030313630313030303030303030303030303135303030303030303030303135303030303030303030303135303030353236303333333539363130303030303036313030303030303030303030323033333335393035323631383031303532363035323636303131303531303030303430393938373635343332313130393030303030303939393337353334323834303030303037303031363D31383031313031303130383134313537333030303333353030303130303030315465726D2D41303149442D436F64652D413030303030314D5352204D45524348414E5420202020202020202020204348494341474F2020202020202020494C3031305A323130353031303130383430383430383430E148A179A0EA612A3131315F2A020840820258008407A0000000041010950500000080009A032105269C01009F02060000000010009F10120110250000044000DAC100000000000000009F1A0208409F260883DDCD28D2B443429F2701809F33036040209F34034203009F360200039F370401FE18169F53015230323631303030303130303031353030383430313131313131313131313031324D5331303030303036363133303530303030202020202020202020204E583132334142434445464142434445464748494A4B4C4D4E4F505152535455565758595A";

            var byteData = Common.ByteHelper.ParseHex(request);
            OnReceive(byteData);

        }

        public bool IsConnected { get { return _isConnected; } }

        public MasterCardNetworkSettings MasterCardNetworkSettings { get { return _networkSettings; } }

        public void ConnectToNetwork()
        {
            if (Interlocked.CompareExchange(ref _connectionStarted, 1, 0) == 0)
            {
                Task.Run(() =>
                {
                    TryConnect();
                });
            }
            else
            {
                _logger.LogInformation("In ConnectToNetwork: connection already in progress.");

            }
        }

        public void DisconnectFromNetwork()
        {
            _shutdown = true;
            LogOff();
            _echoTimer.Dispose();
            _tcpClient.Disconnect();
        }
        public async Task<IIsoResponse> SendIsoRequestAsync(IIsoRequest isoRequest)
        {
            if (!_isConnected)
            {
                _logger.LogError("Not sending message to network as the network is disconnected currently");

                throw new InvalidOperationException("Failed to send request to the network as the network is disconnected currently");
            }
            try
            {
                var task = SendAsync(isoRequest);
                using CancellationTokenSource cts = new(_networkSettings.SendRequestTimeout);
                if (await Task.WhenAny(task, Task.Delay(_networkSettings.SendRequestTimeout, cts.Token)) == task)
                {
                    _logger.LogDebug("In SendIsoRequestAsync: SendAsync task completed hence cancelling the time out task");
                    cts.Cancel();
                    var response = await task;
                    return response;
                }
                else
                {
                    _logger.LogInformation($"In SendIsoRequestAsync: Sending request to network timed out...hence proecessing timeout for message with message Id:{isoRequest?.MessageId}");
                    if (_messageList.TryGetValue(isoRequest.MessageId, out TaskCompletionSource<IIsoResponse> currentTask))
                    {
                        //ProcessTimeoutRequest(isoRequest.MessageId, currentTask);
                    }

                    throw new TimeoutException("Failed to send the iso message as MasterCard network timed out...");
                }
            }
            catch (Exception e)
            {
                if (_messageList.TryRemove(isoRequest.MessageId, out TaskCompletionSource<IIsoResponse> message))
                {
                    message.TrySetException(e);
                }
                throw;
            }
        }

        private static bool IsLogOffRequest(IIsoMessage isoMessage)
        {
            if (isoMessage.MessageTypeIdentifier != MessageTypeIdentifiers.NetworkManagementRequest) return false;
            var request = (NetworkManagementRequest)isoMessage;
            return request.NetworkManagementInformationCode.Value == NetworkManagementInformationCodes.SignOff;
        }

        private void CreateAndStartEchoTimer()
        {
            _logger.LogInformation($"Starting EchoTimer...timer interval is {_networkSettings.EchoTimerInterval}");

            _echoTimer = new System.Timers.Timer(_networkSettings.EchoTimerInterval);
            _echoTimer.Elapsed += EchoTimerElaped;
            _echoTimer.Enabled = true;
            _echoTimer.Start();
        }

        private NetworkManagementRequest CreateNetworkManagementRequest(string ntwMgmtInfoCode)
        {
            NetworkManagementRequest networkManagementRequest = _isoMessageFactory.GetIsoMessage
        (MessageTypeIdentifiers.NetworkManagementRequest) as NetworkManagementRequest;
            //TODO: Change this
            networkManagementRequest.PrimaryAccountNumber.Value = _networkSettings.FIIdCode;
            networkManagementRequest.TransmissionDateTime.Value = DateTime.UtcNow.ToString("MMddHHmmss");
            networkManagementRequest.STAN.Value = _stanGenerator.GenerateSTAN();
            networkManagementRequest.PrimaryAccountNumberCountryCode.Value = "059";
            networkManagementRequest.ForwardingInstitutionIdCode.Value = _networkSettings.FIIdCode;
            networkManagementRequest.NetworkManagementInformationCode.Value = ntwMgmtInfoCode;
            networkManagementRequest.ServiceIndicator.Value = "0I10000";
            networkManagementRequest.MessageSecurityCode.Value = "00000000";
            return networkManagementRequest;
        }
        private NetworkManagementResponse CreateNetworkResponseFromRequest(NetworkManagementRequest originalRequest)
        {
            NetworkManagementResponse networkManagementResponse = _isoMessageFactory.GetIsoMessage(MessageTypeIdentifiers.
        NetworkManagementResponse) as NetworkManagementResponse;
            networkManagementResponse.PrimaryAccountNumber.Value = originalRequest.PrimaryAccountNumber.Value;
            networkManagementResponse.TransmissionDateTime.Value = originalRequest.TransmissionDateTime.Value;
            networkManagementResponse.STAN.Value = originalRequest.STAN.Value;
            networkManagementResponse.ForwardingInstitutionIdCode.Value = originalRequest.ForwardingInstitutionIdCode.Value;
            networkManagementResponse.ResponseCode.Value = ResponseCodes.CompletedSuccessfully;
            if (originalRequest.NetworkData.IsSet)
                networkManagementResponse.NetworkData.Value = originalRequest.NetworkData.Value;
            networkManagementResponse.NetworkManagementInformationCode.Value = originalRequest.NetworkManagementInformationCode.Value;
            if (originalRequest.PrivateData.IsSet)
                networkManagementResponse.PrivateData1.Value = originalRequest.PrivateData.Value;
            return networkManagementResponse;
        }

        private void CreateSocketClient()
        {
            _logger.LogDebug("Start at CreateSocketClient");
            _isConnected = false;
            if (_tcpClient != null)
            {
                _tcpClient.Disconnect();
                _tcpClient.Dispose();
            }

            try
            {
                _tcpClient = _socketClientFactory.GetTCPSocketClient(_networkSettings.NetworkConnectionSettings, _loggerFactory);
                _tcpClient.OnConnected = OnConnect;
                _tcpClient.OnDisconnected = OnDisconnect;
                _tcpClient.OnSent = OnSend;
                _tcpClient.OnReceive = OnReceive;
            }
            catch (ArgumentException exception)
            {
                _logger.LogError($"Error while creating TCP Client. Error: {exception}");
            }

            _logger.LogDebug("End at CreateSocketClient");
        }

        private void EchoTimerElaped(object sender, System.Timers.ElapsedEventArgs e)
        {
            _logger.LogDebug("In EchoTimerElaped, sending echo request to the network.");

            if (_isConnected)
            {
                var request = CreateNetworkManagementRequest(NetworkManagementInformationCodes.Echo);
                SendNetworkMessage(request);
            }
            else
                _logger.LogError("Unable to send ECHO msg, as not connected to network");

        }

        private void LogOff()
        {
            if (_isLoggedIn)
            {
                if (_isConnected)
                {
                    _logger.LogInformation("Sending a logOff to MasterCard network.");
                    var signOffRequest = CreateNetworkManagementRequest(NetworkManagementInformationCodes.SignOff);
                    SendNetworkMessage(signOffRequest);
                    _isLoggedIn = false;
                }
                else
                    _logger.LogInformation("Not connected to the MasterCard network. Hence not sending a logOff to the network.");

            }
            else
                _logger.LogInformation("Not logged in to the MasterCard network. Hence not sending a logOff to the network.");

            //StanAndRRNGenerator.StoreCurrentStanToDb(Convert.ToInt32(StanAndRRNGenerator.GenerateSTAN()));
        }

        private void LogOn()
        {
            _logger.LogDebug("In LogOn() sending logon request to the network.");
            if (_isConnected)
            {
                var signOnRequest = CreateNetworkManagementRequest(NetworkManagementInformationCodes.SignOn);
                SendNetworkMessage(signOnRequest);
            }
            else
            {
                _logger.LogInformation("InLogOn(): Not connected to MasterCard network. Hence not sending a Logon request.");
            }
        }

        private void OnConnect(Socket socket)
        {
            _isConnected = true;

            _logger.LogInformation($"**************Connected to the network at {socket?.RemoteEndPoint}");

            LogOn();
            //CreateAndStartEchoTimer();
        }

        private void OnDisconnect(Socket socket)
        {
            _logger.LogInformation($"********************Disconnected from the network at endpoint ip:- {socket?.RemoteEndPoint}  ");
            _isConnected = false;
            _isLoggedIn = false;
            _tcpClient.Disconnect();
            //ZMK = null;
            ConnectToNetwork();
        }
        private void OnLogonCompleted()
        {
            _logger.LogInformation("InLogOnCompleted...starting the echo timer...");

            //CreateAndStartEchoTimer();
            //Task.Factory.StartNew(() => InitiateKeyExchangeIfRequired());

            ///ReadSAFForAdviceProcessingCanbe done here.
        }

        private void OnReceive(byte[] data)
        {
            try
            {
                _logger.LogInformation($"Received data from network: {BitConverter.ToString(data)}");
                //Read the data and convert to Iso format

                IIsoMessage message = _messageStream.BuildIsoMessageFromByteData(data, out string logMessage);

                if (message == null)
                {
                    _logger.LogError($"Could not create a iso message from received bytes. Please check the bytes in the received data...");
                    return;
                }

                _logger.LogInformation($"In OnReceive: Received Message :\n{logMessage}\n-----**--------");

                ProcessIsoMessage(message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in OnReceive:{e}");
            }
        }

        private void OnSend(SocketAsyncEventArgs obj)
        {

        }

        private void ProcessIsoMessage(IIsoMessage isoMessage)
        {
            switch (isoMessage.MessageTypeIdentifier)
            {
                case MessageTypeIdentifiers.NetworkManagementRequest:
                    ProcessNetworkRequest(isoMessage as NetworkManagementRequest);
                    break;

                case MessageTypeIdentifiers.NetworkManagementResponse:
                    ProcessNetworkResponse(isoMessage as NetworkManagementResponse);
                    break;

                case MessageTypeIdentifiers.AuthorizationRequest:
                    ProcessIsoRequest(isoMessage as IIsoRequest);
                    break;

                case MessageTypeIdentifiers.AuthorizationResponse:
                    ProcessIsoResponse(isoMessage as IIsoResponse);
                    break;
            }

        }
        private void ProcessIsoRequest(IIsoRequest isoRequest)
        {
            //TODO: Create Json object here and send it to Auth Engine...
            _logger.LogDebug("Creating JSON object for the incoming ISO request");

        }
        private void ProcessIsoResponse(IIsoResponse isoResponse)
        {
            _logger.LogDebug("Processing ISO response received from MasterCard network...");

        }
        private void ProcessNetworkRequest(NetworkManagementRequest networkRequest)
        {
            NetworkManagementResponse networkManagementResponse = null;
            switch (networkRequest.NetworkManagementInformationCode.Value)
            {
                case NetworkManagementInformationCodes.SignOn:
                    _logger.LogInformation("Received a SignOn from MasterCard network");
                    networkManagementResponse = CreateNetworkResponseFromRequest(networkRequest);
                    break;

                case NetworkManagementInformationCodes.SignOff:
                    _logger.LogInformation("Received a SignOff from MasterCard network");
                    networkManagementResponse = CreateNetworkResponseFromRequest(networkRequest);
                    break;

                case NetworkManagementInformationCodes.Echo:
                    _logger.LogInformation("Received an Echo from MasterCard network");

                    //If we are not logged on and receive a echo request then we need to reply with a sign on  response
                    networkManagementResponse = CreateNetworkResponseFromRequest(networkRequest);
                    break;
            }

            _logger.LogInformation("In ProcessNetworkRequest: Sending response to MasterCard network");
            if (networkManagementResponse != null)
                SendNetworkMessage(networkManagementResponse);
        }
        private void ProcessNetworkResponse(NetworkManagementResponse isoResponse)
        {
            try
            {
                switch (isoResponse.NetworkManagementInformationCode.Value)
                {
                    case NetworkManagementInformationCodes.SignOn:
                        _logger.LogInformation("Received SignOn Response");

                        if (isoResponse.ResponseCode.Value != ResponseCodes.CompletedSuccessfully)
                        {
                            _logger.LogError("Could not complete the SignOn with MasterCard network. Pls check DE 39 and DE44 for more information");
                            return;
                        }
                        _isLoggedIn = true;
                        OnLogonCompleted();
                        _logger.LogInformation("Signed On to MasterCard network");
                        break;

                    case NetworkManagementInformationCodes.Echo:
                        _logger.LogInformation("Received Echo Response");
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in ProcessNetworkResponse: {e}");
            }
        }

        private void ProcessResponseFromFalcon(IIsoResponse response)
        {
            _logger.LogDebug($"In ProcesResponse, Response MessageId:{response.MessageId}, MessageType:{ response.MessageTypeIdentifier}");

            //If the response we received is not present in the task list means we received a late response.
            if (!_messageList.TryRemove(response.MessageId, out TaskCompletionSource<IIsoResponse> currentTask))
            {
                _logger.LogInformation($"Task list does not contain the task for id: {response.MessageId}");
                //TODO: Do required processing here for late response.
            }
            else
            {
                try
                {
                    var originalRequest = (IIsoRequest)currentTask.Task.AsyncState;
                    //response.PopulateResponse(originalRequest); //TODO: Populate response.
                    currentTask.SetResult(response);
                }
                catch (Exception)
                {
                    //TODO:  If the current response is a financial response and it had succeeded then process a reversal for the same.
                    /*if (response.IsFinancialTransaction && response.ResponseCode.Value == ResponseCodes.CompletedSuccessfully)
                    {
                        Task.Factory.StartNew(() => ProcessReversalAsync(response as IIsoMessage));
                    }*/
                }
            }
        }
        private async Task<IIsoResponse> SendAsync(IIsoRequest msg)
        {
            TaskCompletionSource<IIsoResponse> tcs = new(msg);

            if (!_messageList.TryAdd(msg.MessageId, tcs))
            {
                tcs.TrySetException(new DuplicateStanException(msg.MessageId));
            }
            else
            {
                //TODO: Do SAF if required

                //IMessageStream stream = _serviceProvider.GetService<IMessageStream>();
                byte[] data = _messageStream.ConvertIsoMessageToByteData(msg, out string logMessage);
                _logger.LogDebug($"Sending message to the MasterCard network\nByteData:{BitConverter.ToString(data)}\nDataElements:{logMessage}\n------------------");
                _tcpClient.Send(data);
            }

            //TODO: Confirm the following statement...
            return await tcs.Task;
        }
        private void SendNetworkMessage(IIsoMessage msg)
        {
            _logger.LogDebug($"In SendNetworkMessage, sending message for messageId:{0}", msg.MessageId);
            if (_isConnected && !_shutdown || (_isConnected && IsLogOffRequest(msg)))
            {

                //IMessageStream stream = _serviceProvider.GetService<IMessageStream>();
                byte[] data = _messageStream.ConvertIsoMessageToByteData(msg, out string logMessage);

                _logger.LogDebug($"In External Channel: Sending message to the MasterCard network\nByteData:{BitConverter.ToString(data)}\nDataElements:{logMessage}\n------------------");

                _tcpClient.Send(data);
            }
            else
                _logger.LogDebug($"In SendNetworkMessage, not sending request to network because ", !_isConnected ? "_isConnected is false" : "_shutdown is true");
        }

        private async void TryConnect()
        {
            while (!_isConnected)
            {
                if (!_shutdown)
                {
                    try
                    {
                        CreateSocketClient();
                        _isConnected = false;
                        _logger.LogInformation($"Trying to connect to the network at...{_networkSettings.NetworkConnectionSettings.IPAddress}:{_networkSettings.NetworkConnectionSettings.Port}");
                        _tcpClient.Connect();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(string.Format("Exception while connecting to the network: {0}", e.Message));
                    }

                    await Task.Delay(_networkSettings.NetworkConnectionSettings.NetworkReconnectDelay);
                    _logger.LogInformation("After wait for reconnection");
                }
                else
                    break;
            }
            Interlocked.Exchange(ref _connectionStarted, 0);
        }
    }
}
