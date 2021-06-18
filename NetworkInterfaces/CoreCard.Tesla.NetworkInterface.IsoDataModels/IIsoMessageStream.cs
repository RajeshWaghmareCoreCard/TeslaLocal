namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IIsoMessageStream
    {
        #region Public Methods

        IIsoMessage BuildIsoMessageFromByteData(byte[] data, out string logMessage, int skipBytes = 0);
        byte[] ConvertIsoMessageToByteData(IIsoMessage message, out string logMessage);

        #endregion Public Methods
    }
}