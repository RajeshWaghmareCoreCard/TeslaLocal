using System;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IIsoRequest : IIsoMessage
    {
        #region Public Properties

        bool IsRepeatRequest { get; }


        #endregion Public Properties
    }
}