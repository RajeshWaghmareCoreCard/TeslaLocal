using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CoreCard.Tesla.Common;

namespace CoreCard.Tesla.NetworkInterface.Communication
{
    internal class TcpSocketClient : IDisposable, ISocketClient
    {
        #region Private Fields

        private readonly SocketAsyncEventArgs _connectArgs;
        private readonly SocketAsyncEventArgs _receiveArgs;
        private readonly ConcurrentStack<SocketAsyncEventArgs> _sendArgs;
        private readonly Socket _socket;

        private bool _isDisconnected = true;
        private int _lenOfNextMessage;
        private int _messageIndex;
        private byte[] _nextMessage;
        private int _remainingLength;
        private bool _startofMessage = true;
        private readonly ILogger<TcpSocketClient> _logger;
        private readonly NetworkConnectionSettings _networkConnectionSettings;

        #endregion Private Fields

        public Action<Socket> OnConnected { get; set; }
        public Action<Socket> OnDisconnected { get; set; }
        public Action<byte[]> OnReceive { get; set; }
        public Action<SocketAsyncEventArgs> OnSent { get; set; }

        #region Public Constructors

        public TcpSocketClient(NetworkConnectionSettings networkConnectionSettings, ILogger<TcpSocketClient> logger)
        {
            _networkConnectionSettings = networkConnectionSettings;
            _logger = logger;

            if (!IPAddress.TryParse(_networkConnectionSettings.IPAddress, out IPAddress ipAddress))
                throw new ArgumentException("Invalid IP Address to connect to.");

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _connectArgs = new SocketAsyncEventArgs();
            _connectArgs.Completed += ConnectComplete;
            _connectArgs.UserToken = _socket;
            _connectArgs.RemoteEndPoint = new IPEndPoint(ipAddress, _networkConnectionSettings.Port); ;
            _connectArgs.DisconnectReuseSocket = true;

            _receiveArgs = new SocketAsyncEventArgs();
            _receiveArgs.Completed += ReceiveComplete;
            _receiveArgs.SetBuffer(new byte[_networkConnectionSettings.BufferSize], 0, _networkConnectionSettings.BufferSize);
            _receiveArgs.UserToken = _socket;
            _receiveArgs.DisconnectReuseSocket = true;

            _sendArgs = new ConcurrentStack<SocketAsyncEventArgs>();
            for (int i = 0; i < 500; i++)
            {
                SocketAsyncEventArgs args = new();
                byte[] buffer = new byte[_networkConnectionSettings.BufferSize];
                args.Completed += SendComplete;
                args.UserToken = _socket;
                args.SetBuffer(buffer, 0, _networkConnectionSettings.BufferSize);
                _sendArgs.Push(args);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public void Connect()
        {

            //If the socket the already connected try to reuse it..
            if (_socket.Connected)
                _socket.Disconnect(true);

            if (!_socket.ConnectAsync(_connectArgs))
            {
                ConnectComplete(this, _connectArgs);
            }
        }

        public void Disconnect()
        {
            try
            {
                if (!_isDisconnected)
                {
                    _isDisconnected = true;
                }
                if (_socket.Connected)
                {

                    //_socket.Shutdown(SocketShutdown.Both);
                    _socket.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in disconnect socket...Exception: {ex}");
            }
        }

        public void Dispose()
        {
            _connectArgs.Dispose();
            _receiveArgs.Dispose();

            while (_sendArgs.TryPop(out SocketAsyncEventArgs args))
            {
                args.Completed -= SendComplete;
                args.Dispose();
            }
            _socket.Close();
            _socket.Dispose();

        }

        public void Send(byte[] data)
        {
            //try
            //{
            if (data.Length <= 0)
            {
                return;
            }

            byte[] lengthBytes = GetLengthBytes(data);
            _logger.LogDebug($"In TCPClient: LengthBytes of message being sent:{0}", BitConverter.ToString(lengthBytes));

            data = ByteHelper.CombineByteArrays(lengthBytes, data);

            if (_sendArgs.TryPop(out SocketAsyncEventArgs e))
            {
                e.SetBuffer(0, data.Length);
                Buffer.BlockCopy(data, 0, e.Buffer, 0, data.Length);
            }
            if (!_socket.SendAsync(e))
            {
                SendComplete(this, e);
            }
            /*}
            catch (SocketException ex)
            {
                throw ;
            }
            catch (Exception ex)
            {
                throw ;
            }*/
        }

        #endregion Public Methods

        #region Private Methods

        private void ConnectComplete(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                _isDisconnected = false;
                StartReceive(_receiveArgs);
                OnConnected?.Invoke(_socket);
            }
        }

        private byte[] GetLengthBytes(byte[] data)
        {
            int length = data.Length;

            return _networkConnectionSettings.DataLengthCalculation switch
            {
                DataLengthCalculation.FourByte => _networkConnectionSettings.UseASCIIForLengthBytes ?
                                                        Encoding.ASCII.GetBytes(length.ToString().PadLeft(4, '0')) :
                                                        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length)),
                _ => _networkConnectionSettings.UseASCIIForLengthBytes ?
                                                    Encoding.ASCII.GetBytes(length.ToString().PadLeft(2, '0')) :
                                                    BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(length))),
            };

        }

        private int ReadLengthofNextMessage(byte[] buffer, int index)
        {
            //if (isLittleEndian)
            //    //return Convert.ToInt32(BitConverter.ToInt32(buffer, index));
            //    return BitConverter.ToInt16(buffer, index);
            //return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, index));

            return _networkConnectionSettings.DataLengthCalculation switch
            {
                DataLengthCalculation.FourByte => _networkConnectionSettings.UseASCIIForLengthBytes ?
                                                    Convert.ToInt32(Encoding.ASCII.GetString(buffer, 0, 4)) :
                                                    IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, index)),
                _ => _networkConnectionSettings.UseASCIIForLengthBytes ?
                                                    Convert.ToInt16(Encoding.ASCII.GetString(buffer, 0, 2)) :
                                                    IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, index)),
            };
        }

        private static void ReadNextMessage(byte[] srcBuffer, int srcIndex, int count, byte[] destBuffer, int destIndex)
        {
            Buffer.BlockCopy(srcBuffer, srcIndex, destBuffer, destIndex, count);
        }

        private void ReceiveComplete(object sender, SocketAsyncEventArgs e)
        {
            int bytesRead = e.BytesTransferred;
            if (bytesRead == 0)
            {
                _logger.LogError($"Disconnecting as we received 0 bytes : SocketError : {e?.SocketError}, EndPoint : {e?.RemoteEndPoint}");
                //Socket connection was closed
                _isDisconnected = true;
                OnDisconnected?.Invoke(e.UserToken as Socket);
            }

            if (OnReceive != null && bytesRead > 0)
            {
                int index = 0;

                do
                {
                    if (_startofMessage)
                    {
                        _lenOfNextMessage = ReadLengthofNextMessage(e.Buffer, index);
                        _logger.LogDebug($"Length of incoming message:{ _lenOfNextMessage}");
                        _remainingLength = _lenOfNextMessage;
                        _nextMessage = new byte[_remainingLength];
                        bytesRead = _networkConnectionSettings.DataLengthCalculation == DataLengthCalculation.TwoByte ? bytesRead - 2 : bytesRead - 4;
                        index = _networkConnectionSettings.DataLengthCalculation == DataLengthCalculation.TwoByte ? index + 2 : index + 4;
                        _startofMessage = false;
                    }

                    if (bytesRead >= _remainingLength)
                    {
                        ReadNextMessage(e.Buffer, index, _remainingLength, _nextMessage, _messageIndex);
                        index += _remainingLength;

                        byte[] receivedMessage = new byte[_lenOfNextMessage];
                        Buffer.BlockCopy(_nextMessage, 0, receivedMessage, 0, _lenOfNextMessage);
                        var task = Task.Run(() => { OnReceive(receivedMessage); })
                            .ContinueWith(t => _logger.LogError($"Exception posted by Task - {t}"),
                                    TaskContinuationOptions.OnlyOnFaulted);

                        bytesRead -= _remainingLength;
                        _startofMessage = true;
                        _remainingLength = 0;
                        _messageIndex = 0;
                    }
                    else
                    {
                        ReadNextMessage(e.Buffer, index, bytesRead, _nextMessage, _messageIndex);
                        _remainingLength -= bytesRead;
                        _messageIndex = bytesRead;
                        bytesRead = 0;
                        _startofMessage = false;
                    }
                } while (bytesRead != 0);
            }
            if (!_isDisconnected)
            {
                StartReceive(_receiveArgs);
            }
        }

        private void SendComplete(object sender, SocketAsyncEventArgs e)
        {
            if (OnSent != null)
            {
                if (e.BytesTransferred != 0)
                {
                }
            }
            _sendArgs.Push(e);
        }

        private void StartReceive(SocketAsyncEventArgs e)
        {
            if (!_isDisconnected)
            {
                if (!_socket.ReceiveAsync(e))
                {
                    ReceiveComplete(this, e);
                }
            }
        }

        #endregion Private Methods
    }
}