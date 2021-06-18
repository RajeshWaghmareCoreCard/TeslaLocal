using System.Collections.Generic;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    public class FinancialTransactionAdvice : IIsoRequest
    {

        //TODO: Complete the following verification
        public FinancialTransactionAdvice(IIsoFieldFactory fieldFactory)
        {
            //DE2
            PrimaryAccountNumber = fieldFactory.GetIsoFieldLL("PrimaryAccountNumber", true);
            //DE3
            ProcessingCode = fieldFactory.GetIsoFieldFixed("ProcessingCode", 6, true);
            //DE4
            TransactionAmount = fieldFactory.GetIsoFieldFixed("Amount,Transaction", 12, true);
            //confirm value of mandatory field
            //DE5
            SettlementAmount = fieldFactory.GetIsoFieldFixed("Amount,Settlement", 12, false);
            //DE6
            CardHolderBillingAmount = fieldFactory.GetIsoFieldFixed("Amount,CardHolderBilling", 12, false);
            //DE7
            TransmissionDateTime = fieldFactory.GetIsoFieldFixed("TransmissionDateTime", 10, true);
            //DE9
            SettlementConversionRate = fieldFactory.GetIsoFieldFixed("ConversionRate,Settlement", 8, false);
            //DE10
            CardHolderBillingConversionRate = fieldFactory.GetIsoFieldFixed("ConversionRate,CardHolderBilling", 8, false);
            //DE11
            STAN = fieldFactory.GetIsoFieldFixed("SystemTraceAuditNumber", 6, true);
            //DE12
            LocalTransactionTime = fieldFactory.GetIsoFieldFixed("LocalTransactionTime", 6, true);
            //DE13
            LocalTransationDate = fieldFactory.GetIsoFieldFixed("LocalTransationDate", 4, true);
            //DE14
            ExpirationDate = fieldFactory.GetIsoFieldFixed("ExpirationDate", 4, false);
            //DE15
            SettlementDate = fieldFactory.GetIsoFieldFixed("Date,Settlement", 4, true);
            //DE16
            ConversionDate = fieldFactory.GetIsoFieldFixed("Date,Conversion", 4, false);
            //DE18
            MerchantType = fieldFactory.GetIsoFieldFixed("MerchantType", 18, true);
            //DE22
            POSEntryMode = fieldFactory.GetIsoFieldFixed("POSEntryMode", 3, true);
            //DE23
            CardSequenceNumber = fieldFactory.GetIsoFieldFixed("CardSequenceNumber", 3, false);
            //DE26
            POS_PIN_CaptureCode = fieldFactory.GetIsoFieldFixed("POS_PIN_CaptureCode", 2, false);
            //DE28
            TransactionFeeAmount = fieldFactory.GetIsoFieldFixed("Amount,TransactionFee", 9, false);
            //DE32
            AcquiringInstitutionIdCode = fieldFactory.GetIsoFieldLL("AcquiringInstitutionIdCode", true);
            //DE33
            ForwardingInstitutionIdCode = fieldFactory.GetIsoFieldLL("ForwardingInstitutionIdCode", true);
            //DE35
            Track2Data = fieldFactory.GetIsoFieldLL("Track2Data", false);
            //DE37
            RetrievalReferenceNumber = fieldFactory.GetIsoFieldFixed("RetrievalReferenceNumber", 12, false);
            //DE38
            AuthorizationIdResponse = fieldFactory.GetIsoFieldFixed("AuthorizationIdResponse", 6, false);
            //DE39
            ResponseCode = fieldFactory.GetIsoFieldFixed("ResponseCode", 2, true);
            //DE41
            CardAcceptorTerminalId = fieldFactory.GetIsoFieldFixed("CardAcceptorTerminalId", 8, true);
            //DE42
            CardAcceptorIdCode = fieldFactory.GetIsoFieldFixed("CardAcceptorIdCode", 15, false);
            //DE43
            CardAcceptorNameLocation = fieldFactory.GetIsoFieldFixed("CardAcceptorNameLocation", 40, true);
            //DE44
            AdditionalResponseData = fieldFactory.GetIsoFieldLL("AdditionalResponseData", false);
            //DE48
            AdditionalDataPrivateUse = fieldFactory.GetIsoFieldLLL("AdditionalDataPrivateUse", false);
            //DE49
            TransactionCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Transaction", 3, true);
            //DE50
            SettlementCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Settlement", 3, false);
            //DE51
            CardHolderBillingCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,CardHolderBilling", 3, false);
            //DE54
            AdditionalAmounts = fieldFactory.GetIsoFieldLLL("AdditonalAmounts", false);
            //DE55
            ICCSystemRelatedData = fieldFactory.GetIsoFieldLLL("IntegratedCircuitCard(ICC)SystemRelatedData", false);
            //DE56
            PaymentAccountData = fieldFactory.GetIsoFieldLLL("PaymentAccountData", false);
            //DE58
            AuthorizingAgentInstitutionId = fieldFactory.GetIsoFieldLLL("AuthorizingAgentInstitutionId", false);
            //DE60
            AdviceReasonCode = fieldFactory.GetIsoFieldLLL("AdviceReasonCode", true);
            //DE61
            POSData = fieldFactory.GetIsoFieldLLL("POSData", false);
            //DE62
            INFData = fieldFactory.GetIsoFieldLLL("IntermediateNetworkFacility(INF)Data", false);
            //DE63
            NetworkData = fieldFactory.GetIsoFieldLLL("NetworkData", true);
            //DE90
            OriginalDataElements = fieldFactory.GetIsoFieldFixed("POSData", 42, true);
            //DE95
            ReplacementAmounts = fieldFactory.GetIsoFieldFixed("ReplaceAmounts", 42, true);
            //DE100
            ReceivingInstitutionIdentificationCode = fieldFactory.GetIsoFieldFixed("ReceivingInstitutionIdentificationCode", 11, false);
            //DE102
            AccountId1 = fieldFactory.GetIsoFieldLL("AccountId1", false);
            //DE103
            AccountId2 = fieldFactory.GetIsoFieldLL("AccountId2", false);
            //DE108
            AdditionalTransactionReferenceData = fieldFactory.GetIsoFieldLLL("AdditionalTransactionReferenceData",
            false);
            //DE110
            AdditionalData2 = fieldFactory.GetIsoFieldLLL("AdditionalData2", false);
            //DE111
            CurrencyConversionAssessmentAmount = fieldFactory.GetIsoFieldLLL("CurrencyConversionAssessmentAmount", false);
            //DE112
            AdditionalDataNationalUse = fieldFactory.GetIsoFieldLLL("AdditionalDataNationalUse", false);
            //DE124
            MemberDefinedData = fieldFactory.GetIsoFieldLLL("MemberDefinedData", false);
            //DE126
            SwitchPrivateData = fieldFactory.GetIsoFieldLLL("SwitchPrivateData", true);
            //DE127
            PrivateData = fieldFactory.GetIsoFieldLLL("PrivateData", false);
        }
        public IField AccountId1 { get; private set; }
        public IField AccountId2 { get; private set; }
        public IField AcquiringInstitutionIdCode { get; private set; }
        public IField AdditionalAmounts { get; private set; }
        public IField AdditionalData2 { get; private set; }
        public IField AdditionalDataNationalUse { get; private set; }
        public IField AdditionalDataPrivateUse { get; private set; }
        public IField AdditionalResponseData { get; private set; }
        public IField AdditionalTransactionReferenceData { get; private set; }
        public IField AdviceReasonCode { get; private set; }
        public IField AuthorizationIdResponse { get; private set; }
        public IField AuthorizingAgentInstitutionId { get; private set; }
        public IField CardAcceptorIdCode { get; private set; }
        public IField CardAcceptorNameLocation { get; private set; }
        public IField CardAcceptorTerminalId { get; private set; }
        public IField CardHolderBillingAmount { get; private set; }
        public IField CardHolderBillingConversionRate { get; private set; }
        public IField CardHolderBillingCurrencyCode { get; private set; }
        public IField CardSequenceNumber { get; private set; }
        public IField ConversionDate { get; private set; }
        public IField CurrencyConversionAssessmentAmount { get; private set; }
        public IField ExpirationDate { get; private set; }
        public IField ForwardingInstitutionIdCode { get; private set; }
        public IField ICCSystemRelatedData { get; private set; }//Integrated Circuit Card
        public IField INFData { get; private set; }//Intermediate Network Facility
        public bool IsAdviceMessage { get { return true; } }
        public bool IsRepeatRequest { get { return false; } }
        public bool IsResponseMessage { get { return false; } }
        public IField LocalTransactionTime { get; private set; }
        public IField LocalTransationDate { get; private set; }
        public IField MemberDefinedData { get; private set; }
        public IField MerchantType { get; private set; }
        public string MessageId { get { return string.Concat(STAN.Value, MessageTypeIdentifier); } }
        public string MessageTypeIdentifier { get { return MessageTypeIdentifiers.FinancialTransactionAdvice; } }
        public IField NetworkData { get; private set; }
        public IField OriginalDataElements { get; private set; }
        public IField POSData { get; private set; }
        public IField POSEntryMode { get; private set; }
        public IField POS_PIN_CaptureCode { get; private set; }
        public IField PaymentAccountData { get; private set; }
        public IField PrimaryAccountNumber { get; private set; }
        public IField PrivateData { get; private set; }
        public IField ProcessingCode { get; private set; }
        public IField ReceivingInstitutionIdentificationCode { get; private set; }
        public IField ReplacementAmounts { get; private set; }
        public IField ResponseCode { get; private set; }
        public IField RetrievalReferenceNumber { get; private set; }
        public IField STAN { get; private set; }
        public IField SettlementAmount { get; private set; }
        public IField SettlementConversionRate { get; private set; }
        public IField SettlementCurrencyCode { get; private set; }
        public IField SettlementDate { get; private set; }
        public IField SwitchPrivateData { get; private set; }
        public IField Track2Data { get; private set; }
        public IField TransactionAmount { get; private set; }
        public IField TransactionCurrencyCode { get; private set; }
        public IField TransactionFeeAmount { get; private set; }
        public IField TransmissionDateTime { get; private set; }

        public Dictionary<DataElement, IField> Fields()
        {
            var fields = new Dictionary<DataElement, IField>()
            {
                { DataElement.DE2, PrimaryAccountNumber },
                { DataElement.DE3, ProcessingCode },
                { DataElement.DE4, TransactionAmount },
                { DataElement.DE5, SettlementAmount },
                { DataElement.DE6, CardHolderBillingAmount },
                { DataElement.DE7, TransmissionDateTime },
                { DataElement.DE9, SettlementConversionRate },
                { DataElement.DE10, CardHolderBillingConversionRate },
                { DataElement.DE11, STAN },
                {DataElement.DE12,LocalTransactionTime},
                {DataElement.DE13,LocalTransationDate},
                {DataElement.DE14,ExpirationDate},
                { DataElement.DE15, SettlementDate },
                { DataElement.DE16, ConversionDate },
                {DataElement.DE18,MerchantType},
                { DataElement.DE22, POSEntryMode },
                {DataElement.DE23,CardSequenceNumber},
                {DataElement.DE26,POS_PIN_CaptureCode},
                { DataElement.DE28, TransactionFeeAmount },
                { DataElement.DE32, AcquiringInstitutionIdCode },
                { DataElement.DE33, ForwardingInstitutionIdCode },
                {DataElement.DE35,Track2Data},
                { DataElement.DE37, RetrievalReferenceNumber },
                { DataElement.DE38, AuthorizationIdResponse },
                { DataElement.DE39, ResponseCode },
                { DataElement.DE41, CardAcceptorTerminalId },
                { DataElement.DE42,CardAcceptorIdCode},
                { DataElement.DE43,CardAcceptorNameLocation},
                { DataElement.DE44, AdditionalResponseData },
                { DataElement.DE48, AdditionalDataPrivateUse },
                { DataElement.DE49, TransactionCurrencyCode },
                { DataElement.DE50, SettlementCurrencyCode },
                { DataElement.DE51, CardHolderBillingCurrencyCode },
                { DataElement.DE54, AdditionalAmounts },
                { DataElement.DE55, ICCSystemRelatedData },
                { DataElement.DE56, PaymentAccountData },
                {DataElement.DE58,AuthorizingAgentInstitutionId},
                {DataElement.DE60, AdviceReasonCode},
                {DataElement.DE61,POSData},
                { DataElement.DE62, INFData },
                { DataElement.DE63, NetworkData },
                {DataElement.DE90,OriginalDataElements},
                {DataElement.DE95,ReplacementAmounts},
                {DataElement.DE100,ReceivingInstitutionIdentificationCode},
                { DataElement.DE102, AccountId1 },
                { DataElement.DE103, AccountId2 },
                { DataElement.DE108, AdditionalTransactionReferenceData },
                {DataElement.DE110,AdditionalData2},
                {DataElement.DE111,CurrencyConversionAssessmentAmount},
                { DataElement.DE112, AdditionalDataNationalUse },
                { DataElement.DE124, MemberDefinedData },
                {   DataElement.DE126,SwitchPrivateData},
                { DataElement.DE127, PrivateData }
            };

            return fields;
        }
    }
}
