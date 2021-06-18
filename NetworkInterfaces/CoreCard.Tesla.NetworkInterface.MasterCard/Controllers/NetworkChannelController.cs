using System;
using Microsoft.AspNetCore.Mvc;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.Controllers
{
    /// <summary>
    /// Represents the Controller class which exposes the MasterCard Admin APIs
    /// </summary>
    [ApiController]
    [Route("Controller")]
    public class NetworkChannelController : ControllerBase
    {
        private readonly INetworkChannel _networkChannel;

        public NetworkChannelController(INetworkChannel networkChannel)
        {
            _networkChannel = networkChannel;
        }

        /// <summary>
        /// Provides the settings using which the network interface connects to the MasterCard network.
        /// </summary>
        /// <returns>Settings used to connect to the MasterCard network</returns>
        [HttpGet]
        [Route("GetMasterCardSettings")]
        public MasterCardNetworkSettings GetMasterCardNetworkSettings()
        {
            return _networkChannel.MasterCardNetworkSettings;
        }
    }
}
