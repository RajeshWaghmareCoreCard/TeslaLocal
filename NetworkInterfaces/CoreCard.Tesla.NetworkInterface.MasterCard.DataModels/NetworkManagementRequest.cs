using System.Collections.Generic;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    public class NetworkManagementRequest : IIsoRequest
    {
        public NetworkManagementRequest(IIsoFieldFactory fieldFactory)
        {
            PrimaryAccountNumber = fieldFactory.GetIsoFieldLL("PrimaryAccountNumber", true);
            TransmissionDateTime = fieldFactory.GetIsoFieldFixed("TransmissionDateTime", 10, true);
            STAN = fieldFactory.GetIsoFieldFixed("STAN", 6, true);
            PrimaryAccountNumberCountryCode = fieldFactory.GetIsoFieldFixed("PrimaryAccountNumberCountryCode", 3, true);
            ForwardingInstitutionIdCode = fieldFactory.GetIsoFieldLL("ForwardingInstitutionIdCode", true);
            SecurityRelatedControlInfo = fieldFactory.GetIsoFieldFixed("SecurityRelatedControlInfo", 16, false);
            NetworkData = fieldFactory.GetIsoFieldLLL("NetworkData", false);
            NetworkManagementInformationCode = fieldFactory.GetIsoFieldFixed("NetworkManagementInformationCode", 3, true);
            ServiceIndicator = fieldFactory.GetIsoFieldFixed("ServiceIndicator", 7, true);
            //TODO: Contains binary data so confirm length
            MessageSecurityCode = fieldFactory.GetIsoFieldFixed("MessageSecurityCode", 8, true);
            PrivateData = fieldFactory.GetIsoFieldLLL("PrivateData7", false);
        }
        public IField ForwardingInstitutionIdCode { get; private set; }
        public bool IsAdviceMessage { get { return false; } }
        public bool IsRepeatRequest { get { return false; } }
        public bool IsResponseMessage { get { return false; } }
        public string MessageId { get { return string.Concat(STAN.Value, MessageTypeIdentifier); } }
        public IField MessageSecurityCode { get; private set; }
        public string MessageTypeIdentifier { get { return MessageTypeIdentifiers.NetworkManagementRequest; } }
        public IField NetworkData { get; private set; }
        public IField NetworkManagementInformationCode { get; private set; }
        public IField PrimaryAccountNumber { get; private set; }
        public IField PrimaryAccountNumberCountryCode { get; private set; }
        public IField PrivateData { get; private set; }
        public IField ResponseCode { get; private set; }
        public IField STAN { get; private set; }
        public IField SecurityRelatedControlInfo { get; private set; }
        public IField ServiceIndicator { get; private set; }
        public IField TransmissionDateTime { get; private set; }

        public Dictionary<DataElement, IField> Fields()
        {
            Dictionary<DataElement, IField> fields = new()
            {
                { DataElement.DE2, PrimaryAccountNumber },
                { DataElement.DE7, TransmissionDateTime },
                { DataElement.DE11, STAN },
                { DataElement.DE20, PrimaryAccountNumberCountryCode },
                { DataElement.DE33, ForwardingInstitutionIdCode },
                { DataElement.DE53, SecurityRelatedControlInfo },
                { DataElement.DE63, NetworkData },
                { DataElement.DE70, NetworkManagementInformationCode },
                { DataElement.DE94, ServiceIndicator },
                { DataElement.DE96, MessageSecurityCode },
                { DataElement.DE127, PrivateData }
            };
            return fields;
        }
    }
}
