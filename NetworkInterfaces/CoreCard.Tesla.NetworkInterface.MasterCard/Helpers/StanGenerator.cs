using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Options;
using Serilog;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.Helpers
{
    public class StanGenerator
    {
        private readonly MasterCardNetworkSettings _networkSettings;
        int _currentStanValue; // Get it from db later
        public StanGenerator(IOptions<MasterCardNetworkSettings> networkSettings)
        {
            _networkSettings = networkSettings.Value;
            _currentStanValue = 1;
        }
        public string GenerateSTAN()
        {
            Log.Debug(" Start GenerateSTAN");

            int newValue = Interlocked.Increment(ref _currentStanValue);

            var result = (newValue % (_networkSettings.MaxStanValue - _networkSettings.MinStanValue)) + _networkSettings.MinStanValue;

            return result.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0');
        }

    }

}