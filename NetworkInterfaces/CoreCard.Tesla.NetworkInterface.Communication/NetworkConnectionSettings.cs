namespace CoreCard.Tesla.NetworkInterface.Communication
{
    public class NetworkConnectionSettings
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public int BufferSize { get; set; }
        public int NetworkReconnectDelay { get; set; }
        public bool UseASCIIForLengthBytes { get; set; }
        public DataLengthCalculation DataLengthCalculation { get; set; }

    }
}