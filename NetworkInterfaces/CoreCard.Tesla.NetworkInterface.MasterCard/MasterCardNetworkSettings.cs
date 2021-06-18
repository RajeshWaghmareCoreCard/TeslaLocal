using CoreCard.Tesla.NetworkInterface.Communication;

namespace CoreCard.Tesla.NetworkInterface.MasterCard
{
    public class MasterCardNetworkSettings
    {
        public NetworkConnectionSettings NetworkConnectionSettings { get; set; }
        public int SendRequestTimeout { get; set; }
        public int EchoTimerInterval { get; set; }
        public int MinStanValue { get; set; }
        public int MaxStanValue { get; set; }
        public string FIIdCode { get; set; }
    }
}