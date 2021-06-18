using System.Collections.Generic;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    public class AuthorizationResponse : IIsoResponse
    {

        public AuthorizationResponse(IIsoFieldFactory fieldFactory)
        {
            PrimaryAccountNumber = fieldFactory.GetIsoFieldLL("PrimaryAccountNumber", true);
            ProcessingCode = fieldFactory.GetIsoFieldFixed("ProcessingCode", 6, true);
            TransactionAmount = fieldFactory.GetIsoFieldFixed("Amount,Transaction", 12, true);
            SettlementAmount = fieldFactory.GetIsoFieldFixed("Amount,Settlement", 12, false);
            CardHolderBillingAmount = fieldFactory.GetIsoFieldFixed("Amount,CardHolderBilling", 12, false);
            TransmissionDateTime = fieldFactory.GetIsoFieldFixed("TransmissionDateTime", 10, true);
            SettlementConversionRate = fieldFactory.GetIsoFieldFixed("ConversionRate,Settlement", 8, false);
            CardHolderBillingConversionRate = fieldFactory.GetIsoFieldFixed("ConversionRate,CardHolderBilling", 8,
            false);
            STAN = fieldFactory.GetIsoFieldFixed("SystemTraceAuditNumber", 6, true);
            SettlementDate = fieldFactory.GetIsoFieldFixed("Date,Settlement", 4, true);
            ConversionDate = fieldFactory.GetIsoFieldFixed("Date,Conversion", 4, false);
            PrimaryAccountNumberCountryCode = fieldFactory.GetIsoFieldFixed("PrimaryAccountNumber(PAN)CountryCode",
            3, false);
            TransactionFeeAmount = fieldFactory.GetIsoFieldFixed("Amount,TransactionFee", 9, false);
            AcquiringInstitutionIdCode = fieldFactory.GetIsoFieldLL("AcquiringInstitutionIdCode", true);
            ForwardingInstitutionIdCode = fieldFactory.GetIsoFieldLL("ForwardingInstitutionIdCode", false);
            RetrievalReferenceNumber = fieldFactory.GetIsoFieldFixed("RetrievalReferenceNumber", 12, false);
            AuthorizationIdResponse = fieldFactory.GetIsoFieldFixed("AuthorizationIdResponse", 6, false);
            ResponseCode = fieldFactory.GetIsoFieldFixed("ResponseCode", 2, true);
            CardAcceptorTerminalId = fieldFactory.GetIsoFieldFixed("CardAcceptorTerminalId", 8, false);
            AdditionalResponseData = fieldFactory.GetIsoFieldLL("AdditionalResponseData", false);
            AdditionalDataPrivateUse = fieldFactory.GetIsoFieldLLL("AdditionalDataPrivateUse", false);
            TransactionCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Transaction", 3, true);
            SettlementCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Settlement", 3, false);
            CardHolderBillingCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,CardHolderBilling", 3, false);
            AdditionalAmounts = fieldFactory.GetIsoFieldLLL("AdditonalAmounts", false);
            ICCSystemRelatedData = fieldFactory.GetIsoFieldLLL("IntegratedCircuitCard(ICC)SystemRelatedData", false)
            ;
            PaymentAccountData = fieldFactory.GetIsoFieldLLL("PaymentAccountData", false);
            INFData = fieldFactory.GetIsoFieldLLL("IntermediateNetworkFacility(INF)Data", false);
            NetworkData = fieldFactory.GetIsoFieldLLL("NetworkData", true);
            AccountId1 = fieldFactory.GetIsoFieldLL("AccountId1", false);
            AccountId2 = fieldFactory.GetIsoFieldLL("AccountId2", false);
            AdditionalTransactionReferenceData = fieldFactory.GetIsoFieldLLL("AdditionalTransactionReferenceData",
            false);
            AdditionalDataNationalUse = fieldFactory.GetIsoFieldLLL("AdditionalDataNationalUse", false);
            RecordData = fieldFactory.GetIsoFieldLLL("RecordData", false);
            AuthorizingAgentIdCode = fieldFactory.GetIsoFieldLLL("AuthorizingAgentIdCode", false);
            ReceiptFreeText = fieldFactory.GetIsoFieldLLL("ReceiptFreeText", false);
            MemberDefinedData = fieldFactory.GetIsoFieldLLL("MemberDefinedData", false);
            PrivateData = fieldFactory.GetIsoFieldLLL("PrivateData", false);
        }
        public IField AccountId1 { get; private set; }
        public IField AccountId2 { get; private set; }
        public IField AcquiringInstitutionIdCode { get; private set; }
        public IField AdditionalAmounts { get; private set; }
        public IField AdditionalDataNationalUse { get; private set; }
        public IField AdditionalDataPrivateUse { get; private set; }
        public IField AdditionalResponseData { get; private set; }
        public IField AdditionalTransactionReferenceData { get; private set; }
        public IField AuthorizationIdResponse { get; private set; }
        public IField AuthorizingAgentIdCode { get; private set; }
        public IField CardAcceptorTerminalId { get; private set; }
        public IField CardHolderBillingAmount { get; private set; }
        public IField CardHolderBillingConversionRate { get; private set; }
        public IField CardHolderBillingCurrencyCode { get; private set; }
        public IField ConversionDate { get; private set; }
        public IField ForwardingInstitutionIdCode { get; private set; }
        public IField ICCSystemRelatedData { get; private set; }//Integrated Circuit Card
        public IField INFData { get; private set; }//Intermediate Network Facility

        public bool IsAdviceMessage { get { return false; } }
        public bool IsRepeatRequest { get { return false; } }

        public bool IsResponseMessage { get { return true; } }
        public IField MemberDefinedData { get; private set; }

        public string MessageId { get { return string.Concat(STAN.Value, MessageTypeIdentifier); } }

        public string MessageTypeIdentifier { get { return MessageTypeIdentifiers.AdministrativeAdviceResponse; } }
        public IField NetworkData { get; private set; }
        public IField PaymentAccountData { get; private set; }
        public IField PrimaryAccountNumber { get; private set; }
        public IField PrimaryAccountNumberCountryCode { get; private set; }
        public IField PrivateData { get; private set; }
        public IField ProcessingCode { get; private set; }
        public IField ReceiptFreeText { get; private set; }
        public IField RecordData { get; private set; }
        public IField ResponseCode { get; private set; }
        public IField RetrievalReferenceNumber { get; private set; }

        public IField STAN { get; private set; }
        public IField SettlementAmount { get; private set; }
        public IField SettlementConversionRate { get; private set; }
        public IField SettlementCurrencyCode { get; private set; }
        public IField SettlementDate { get; private set; }
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
                { DataElement.DE10, CardHolderBillingConversionRate },
                { DataElement.DE11, STAN },
                { DataElement.DE15, SettlementDate },
                { DataElement.DE16, ConversionDate },
                { DataElement.DE20, PrimaryAccountNumberCountryCode },
                { DataElement.DE28, TransactionFeeAmount },
                { DataElement.DE32, AcquiringInstitutionIdCode },
                { DataElement.DE33, ForwardingInstitutionIdCode },
                { DataElement.DE37, RetrievalReferenceNumber },
                { DataElement.DE38, AuthorizationIdResponse },
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
                { DataElement.DE102, AccountId1 },
                { DataElement.DE103, AccountId2 },
                { DataElement.DE108, AdditionalTransactionReferenceData },
                { DataElement.DE112, AdditionalDataNationalUse },
                { DataElement.DE120, RecordData },
                { DataElement.DE121, AuthorizingAgentIdCode },
                { DataElement.DE123, ReceiptFreeText },
                { DataElement.DE124, MemberDefinedData },
                { DataElement.DE127, PrivateData }
            };

            return fields;
        }
    }
}