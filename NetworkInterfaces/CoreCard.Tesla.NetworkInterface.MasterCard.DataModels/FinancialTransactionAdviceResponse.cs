using System.Collections.Generic;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    public class FinancialTransactionAdviceResponse : IIsoResponse
    {
        public FinancialTransactionAdviceResponse(IIsoFieldFactory fieldFactory)
        {
            //DE2
            PrimaryAccountNumber = fieldFactory.GetIsoFieldLL("PrimaryAccountNumber", true);
            //DE3
            ProcessingCode = fieldFactory.GetIsoFieldFixed("ProcessingCode", 6, true);
            //DE4
            TransactionAmount = fieldFactory.GetIsoFieldFixed("Amount,Transaction", 12, true);
            /*Because DE5 is  mandatory in 0200 for issuer and DE5 in response shud be present if
            it is present in request, it is marked as mandatory here.*/
            //DE5
            SettlementAmount = fieldFactory.GetIsoFieldFixed("Amount,Settlement", 12, true);
            //DE7
            TransmissionDateTime = fieldFactory.GetIsoFieldFixed("TransmissionDateTime", 10, true);
            /*Because DE5 is  mandatory in 0200 for issuer and DE9 in response shud be present if
            it is present in request, it is marked as mandatory here.*/
            //DE9
            SettlementConversionRate = fieldFactory.GetIsoFieldFixed("ConversionRate,Settlement", 8, true);
            //DE11
            STAN = fieldFactory.GetIsoFieldFixed("SystemTraceAuditNumber", 6, true);
            //DE12
            LocalTransactionTime = fieldFactory.GetIsoFieldFixed("Time,LocalTransaction", 6, true);
            //DE13
            LocalTransationDate = fieldFactory.GetIsoFieldFixed("Date,LocalTransaction", 4, true);
            //DE15
            SettlementDate = fieldFactory.GetIsoFieldFixed("Date,Settlement", 4, true);
            //DE16
            ConversionDate = fieldFactory.GetIsoFieldFixed("Date,Conversion", 4, true);
            //DE20
            PANCountryCode = fieldFactory.GetIsoFieldFixed("PANCountryCode", 3, false);
            //DE28
            TransactionFeeAmount = fieldFactory.GetIsoFieldFixed("Amount,TransactionFee", 9, false);
            //DE32
            AcquiringInstitutionIdCode = fieldFactory.GetIsoFieldLL("AcquiringInstitutionIdCode", true);
            //DE33
            ForwardingInstitutionIdCode = fieldFactory.GetIsoFieldLL("ForwardingInstitutionIdCode", true);
            //DE37
            RetrievalReferenceNumber = fieldFactory.GetIsoFieldFixed("RetrievalReferenceNumber", 12, false);
            //DE39
            ResponseCode = fieldFactory.GetIsoFieldFixed("ResponseCode", 2, true);
            //DE41
            CardAcceptorTerminalId = fieldFactory.GetIsoFieldFixed("CardAcceptorTerminalId", 8, true);
            //DE44
            AdditionalResponseData = fieldFactory.GetIsoFieldLL("AdditionalResponseData", false);
            //DE49
            TransactionCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Transaction", 3, true);
            //DE50
            /*Because DE5 is mandatory in 0200 for issuer and DE50 in response shud be present if
            it is present in request, it is marked as mandatory here.*/
            SettlementCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Settlement", 3, true);
            //DE54
            AdditionalAmounts = fieldFactory.GetIsoFieldLLL("AdditonalAmounts", false);
            //DE56
            PaymentAccountData = fieldFactory.GetIsoFieldLLL("PaymentAccountData", false);
            //DE62
            INFData = fieldFactory.GetIsoFieldLLL("IntermediateNetworkFacility(INF)Data", false);
            //DE63
            NetworkData = fieldFactory.GetIsoFieldLLL("NetworkData", true);
            //DE95
            ReplacementAmounts = fieldFactory.GetIsoFieldFixed("ReplacementAmounts", 42, false);
            //DE100
            ReceivingInstitutionIdCode = fieldFactory.GetIsoFieldLLL("ReceivingInstitutionIdCode", false);
            //DE112
            AdditionalDataNationalUse = fieldFactory.GetIsoFieldLLL("AdditionalDataNationalUse", false);
            //DE126
            SwitchPrivateData = fieldFactory.GetIsoFieldLLL("SwitchPrivateData", true);
            //DE127
            ProcessorPrivateData = fieldFactory.GetIsoFieldLLL("ProcessorPrivateData", false);
        }

        public IField AcquiringInstitutionIdCode { get; private set; }
        public IField AdditionalAmounts { get; private set; }
        public IField AdditionalDataNationalUse { get; private set; }
        public IField AdditionalResponseData { get; private set; }
        public IField CardAcceptorTerminalId { get; private set; }
        public IField ConversionDate { get; private set; }
        public IField ForwardingInstitutionIdCode { get; private set; }
        public IField INFData { get; private set; } //Intermediate Network Facility
        public bool IsAdviceMessage { get { return false; } }
        public bool IsResponseMessage { get { return true; } }
        public IField LocalTransactionTime { get; private set; }
        public IField LocalTransationDate { get; private set; }
        public string MessageId { get { return string.Concat(STAN.Value, MessageTypeIdentifier); } }
        public string MessageTypeIdentifier { get { return MessageTypeIdentifiers.FinancialTransactionAdviceResponse; } }
        public IField NetworkData { get; private set; }
        public IField PANCountryCode { get; private set; }
        public IField PaymentAccountData { get; private set; }
        public IField PrimaryAccountNumber { get; private set; }
        public IField ProcessingCode { get; private set; }
        public IField ProcessorPrivateData { get; private set; }
        public IField ReceivingInstitutionIdCode { get; private set; }
        public IField ReplacementAmounts { get; private set; }
        public IField ResponseCode { get; private set; }
        public IField RetrievalReferenceNumber { get; private set; }
        public IField STAN { get; private set; }
        public IField SettlementAmount { get; private set; }
        public IField SettlementConversionRate { get; private set; }
        public IField SettlementCurrencyCode { get; private set; }
        public IField SettlementDate { get; private set; }
        public IField SwitchPrivateData { get; private set; }
        public IField TransactionAmount { get; private set; }
        public IField TransactionCurrencyCode { get; private set; }
        public IField TransactionFeeAmount { get; private set; }
        public IField TransmissionDateTime { get; private set; }
        public Dictionary<DataElement, IField> Fields()
        {
            Dictionary<DataElement, IField> fields = new()
            {

                { DataElement.DE2, PrimaryAccountNumber },
                { DataElement.DE3, ProcessingCode },
                { DataElement.DE4, TransactionAmount },
                { DataElement.DE5, SettlementAmount },
                { DataElement.DE7, TransmissionDateTime },
                { DataElement.DE9, SettlementConversionRate },
                { DataElement.DE11, STAN },
                { DataElement.DE12, LocalTransactionTime },
                { DataElement.DE13, LocalTransationDate },
                { DataElement.DE15, SettlementDate },
                { DataElement.DE16, ConversionDate },
                { DataElement.DE20, PANCountryCode },
                { DataElement.DE28, TransactionFeeAmount },
                { DataElement.DE32, AcquiringInstitutionIdCode },
                { DataElement.DE33, ForwardingInstitutionIdCode },
                { DataElement.DE37, RetrievalReferenceNumber },
                { DataElement.DE39, ResponseCode },
                { DataElement.DE41, CardAcceptorTerminalId },
                { DataElement.DE44, AdditionalResponseData },
                { DataElement.DE49, TransactionCurrencyCode },
                { DataElement.DE50, SettlementCurrencyCode },
                { DataElement.DE54, AdditionalAmounts },
                { DataElement.DE56, PaymentAccountData },
                { DataElement.DE62, INFData },
                { DataElement.DE63, NetworkData },
                { DataElement.DE95, ReplacementAmounts },
                { DataElement.DE100, ReceivingInstitutionIdCode },
                { DataElement.DE112, AdditionalDataNationalUse },
                { DataElement.DE126, SwitchPrivateData },
                { DataElement.DE127, ProcessorPrivateData }
            };

            return fields;
        }
    }
}
