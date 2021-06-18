using System.Collections.Generic;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    public class FinancialResponse : IIsoResponse
    {
        public FinancialResponse(IIsoFieldFactory fieldFactory)
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
            //DE6
            CardHolderBillingAmount = fieldFactory.GetIsoFieldFixed("Amount,CardHolderBilling", 12, true);
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
            /*Because DE5 is mandatory in 0200 for issuer and DE16 in response shud be present if
            it is present in request, it is marked as mandatory here.*/
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
            //DE48
            AdditionalDataPrivateUse = fieldFactory.GetIsoFieldLLL("AdditionalDataPrivateUse", false);
            //DE49
            TransactionCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Transaction", 3, true);
            //DE50
            /*Because DE5 is mandatory in 0200 for issuer and DE50 in response shud be present if
            it is present in request, it is marked as mandatory here.*/
            SettlementCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Settlement", 3, true);
            //DE51
            /*Because DE6 is mandatory in 0200 for issuer and DE51 shud be present if DE6 is present, it is marked as mandatory here*/
            CardHolderBillingCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,CardHolderBilling", 3, true);
            //DE54
            AdditionalAmounts = fieldFactory.GetIsoFieldLLL("AdditonalAmounts", false);
            //DE55
            ICCSystemRelatedData = fieldFactory.GetIsoFieldLLL("IntegratedCircuitCard(ICC)SystemRelatedData", false);
            //DE56
            PaymentAccountData = fieldFactory.GetIsoFieldLLL("PaymentAccountData", false);
            //DE62
            INFData = fieldFactory.GetIsoFieldLLL("IntermediateNetworkFacility(INF)Data", false);
            //DE63
            NetworkData = fieldFactory.GetIsoFieldLLL("NetworkData", true);
            //DE100
            ReceivingInstitutionIdCode = fieldFactory.GetIsoFieldLLL("ReceivingInstitutionIdCode", false);
            //DE102
            AccountIdentification1 = fieldFactory.GetIsoFieldLL("AccountIdentification1", false);
            //DE103
            AccountIdentification2 = fieldFactory.GetIsoFieldLL("AccountIdentification2", false);
            //DE105
            EnhancedIdentificationData = fieldFactory.GetIsoFieldLLL("EnhancedIdentificationData", false);
            //DE108
            AdditonalTransactionReferenceData = fieldFactory.GetIsoFieldLLL("AdditonalTransactionReferenceData", false);
            //DE110
            AdditionalData2 = fieldFactory.GetIsoFieldLLL("AdditionalData2", false);
            //DE112
            AdditionalDataNationalUse = fieldFactory.GetIsoFieldLLL("AdditionalDataNationalUse", false);
            //DE120
            RecordData = fieldFactory.GetIsoFieldLLL("RecordData", false);
            //DE124
            MemberDefinedData = fieldFactory.GetIsoFieldLLL("MemberDefinedData", false);
            //DE126
            SwitchPrivateData = fieldFactory.GetIsoFieldLLL("SwitchPrivateData", true);
            //DE127
            ProcessorPrivateData = fieldFactory.GetIsoFieldLLL("ProcessorPrivateData", false);
        }
        public IField AccountIdentification1 { get; private set; }
        public IField AccountIdentification2 { get; private set; }
        public IField AcquiringInstitutionIdCode { get; private set; }
        public IField AdditionalAmounts { get; private set; }
        public IField AdditionalData2 { get; private set; }
        public IField AdditionalDataNationalUse { get; private set; }
        public IField AdditionalDataPrivateUse { get; private set; }
        public IField AdditionalResponseData { get; private set; }
        public IField AdditonalTransactionReferenceData { get; private set; }
        public IField AuthorizationIdentificationResponse { get; private set; }
        public IField CardAcceptorTerminalId { get; private set; }
        public IField CardHolderBillingAmount { get; private set; }
        public IField CardHolderBillingCurrencyCode { get; private set; }
        public IField ConversionDate { get; private set; }
        public IField EnhancedIdentificationData { get; private set; }
        public IField ForwardingInstitutionIdCode { get; private set; }
        public IField ICCSystemRelatedData { get; private set; } //Integrated Circuit Card
        public IField INFData { get; private set; } //Intermediate Network Facility
        public bool IsAdviceMessage { get { return false; } }
        public bool IsRepeatRequest { get { return false; } }
        public bool IsResponseMessage { get { return true; } }
        public IField LocalTransactionTime { get; private set; }
        public IField LocalTransationDate { get; private set; }
        public IField MemberDefinedData { get; private set; }
        public string MessageId { get { return string.Concat(STAN.Value, MessageTypeIdentifier); } }
        public string MessageTypeIdentifier { get { return MessageTypeIdentifiers.FinancialResponse; } }
        public IField NetworkData { get; private set; }
        public IField PANCountryCode { get; private set; }
        public IField PaymentAccountData { get; private set; }
        public IField PrimaryAccountNumber { get; private set; }
        public IField ProcessingCode { get; private set; }
        public IField ProcessorPrivateData { get; private set; }
        public IField ReceivingInstitutionIdCode { get; private set; }
        public IField RecordData { get; private set; }
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
                { DataElement.DE6, CardHolderBillingAmount },
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
                { DataElement.DE38, AuthorizationIdentificationResponse },
                { DataElement.DE39, ResponseCode },
                { DataElement.DE41, CardAcceptorTerminalId },
                { DataElement.DE44, AdditionalResponseData },
                { DataElement.DE48, AdditionalDataPrivateUse },
                { DataElement.DE49, TransactionCurrencyCode },
                { DataElement.DE50, SettlementCurrencyCode },
                { DataElement.DE51, CardHolderBillingCurrencyCode },
                { DataElement.DE54, AdditionalAmounts },
                { DataElement.DE55, ICCSystemRelatedData },
                { DataElement.DE56, PaymentAccountData },
                { DataElement.DE62, INFData },
                { DataElement.DE63, NetworkData },
                { DataElement.DE100, ReceivingInstitutionIdCode },
                { DataElement.DE102, AccountIdentification1 },
                { DataElement.DE103, AccountIdentification2 },
                { DataElement.DE105, EnhancedIdentificationData },
                { DataElement.DE108, AdditonalTransactionReferenceData },
                { DataElement.DE110, AdditionalData2 },
                { DataElement.DE112, AdditionalDataNationalUse },
                { DataElement.DE120, RecordData },
                { DataElement.DE124, MemberDefinedData },
                { DataElement.DE126, SwitchPrivateData },
                { DataElement.DE127, ProcessorPrivateData }
            };

            return fields;
        }
    }
}
