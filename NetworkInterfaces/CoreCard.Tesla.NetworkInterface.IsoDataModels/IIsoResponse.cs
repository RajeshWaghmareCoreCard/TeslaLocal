namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IIsoResponse : IIsoMessage
    {
        #region Public Properties

        IField ResponseCode { get; }

        //TODO: Delete this property if not required.
        //string ResponseMessage { get;  }

        #endregion Public Properties
    }
}