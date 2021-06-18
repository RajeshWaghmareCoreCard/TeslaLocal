using System;
using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public class IsoMessageStreamFactory : IIsoMessageStreamFactory
    {
        public IIsoMessageStream GetIsoMessageStream(ILoggerFactory loggerFactory, IIsoMessageFactory isoMessageFactory)
        {
            return new IsoMessageStream(loggerFactory.CreateLogger<IsoMessageStream>(), isoMessageFactory);
        }
    }
}
