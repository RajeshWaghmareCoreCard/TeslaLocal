using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IIsoMessageStreamFactory
    {
        //We can add methods here to get ASCII message streams and others depending on the encoding type.
        IIsoMessageStream GetIsoMessageStream(ILoggerFactory loggerFactory, IIsoMessageFactory isoMessageFactory);
    }
}
