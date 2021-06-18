using System.Collections.Generic;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    public class NetworkManagementResponse : IIsoResponse
    {
        public NetworkManagementResponse(IIsoFieldFactory fieldFactory)
        {
            PrimaryAccountNumber = fieldFactory.GetIsoFieldLL("PrimaryAccountNumber", false);
            TransmissionDateTime = fieldFactory.GetIsoFieldFixed("TransmissionDateTime", 10, false);
            STAN = fieldFactory.GetIsoFieldFixed("STAN", 6, false);
            ForwardingInstitutionIdCode = fieldFactory.GetIsoFieldLL("ForwardingInstitutionIdCode", false);
            ResponseCode = fieldFactory.GetIsoFieldFixed("ResponseData", 2, true);
            AdditionalResponseData = fieldFactory.GetIsoFieldLL("AdditionalResponseData", false);
            NetworkData = fieldFactory.GetIsoFieldLLL("NetworkData", false);
            NetworkManagementInformationCode = fieldFactory.GetIsoFieldFixed("NetworkManagementInformationCode", 3, false);
            AdditionalData2 = fieldFactory.GetIsoFieldLLL("AdditionalData2 ", false);
            PrivateData1 = fieldFactory.GetIsoFieldLLL("PrivateData1", false);
            PrivateData2 = fieldFactory.GetIsoFieldLLL("PrivateData2", false);
        }
        public IField AdditionalData2 { get; private set; }
        public IField AdditionalResponseData { get; private set; }
        public IField ForwardingInstitutionIdCode { get; private set; }
        public bool IsAdviceMessage { get { return false; } }
        public bool IsRepeatRequest { get { return false; } }
        public bool IsResponseMessage { get { return true; } }
        public string MessageId { get { return string.Concat(STAN.Value, MessageTypeIdentifier); } }
        public string MessageTypeIdentifier { get { return MessageTypeIdentifiers.NetworkManagementResponse; } }
        public IField NetworkData { get; private set; }
        public IField NetworkManagementInformationCode { get; private set; }
        public IField PrimaryAccountNumber { get; private set; }
        public IField PrivateData1 { get; private set; }
        public IField PrivateData2 { get; private set; }
        public IField ResponseCode { get; }
        public IField STAN { get; private set; }
        public IField TransmissionDateTime { get; private set; }

        public Dictionary<DataElement, IField> Fields()
        {
            Dictionary<DataElement, IField> fields = new()
            {
                { DataElement.DE2, PrimaryAccountNumber },
                { DataElement.DE7, TransmissionDateTime },
                { DataElement.DE11, STAN },
                { DataElement.DE33, ForwardingInstitutionIdCode },
                { DataElement.DE39, ResponseCode },
                { DataElement.DE44, AdditionalResponseData },
                { DataElement.DE63, NetworkData },
                { DataElement.DE70, NetworkManagementInformationCode },
                { DataElement.DE110, AdditionalData2 },
                { DataElement.DE126, PrivateData1 },
                { DataElement.DE127, PrivateData2 }
            };
            return fields;
        }
    }

}