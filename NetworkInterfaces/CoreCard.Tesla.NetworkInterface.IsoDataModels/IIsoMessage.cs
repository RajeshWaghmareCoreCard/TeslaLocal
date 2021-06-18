using System.Collections.Generic;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IIsoMessage
    {
        #region Public Methods
        Dictionary<DataElement, IField> Fields();

        #endregion Public Methods

        #region Public Properties

        bool IsAdviceMessage { get; }

        bool IsResponseMessage { get; }

        string MessageTypeIdentifier { get; }

        string MessageId { get; }

        #endregion Public Properties
    }
}