using System.Collections.Generic;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels.Constants;

namespace CoreCard.Tesla.NetworkInterface.MasterCard.DataModels
{
    public class AuthorizationRequest : IIsoRequest
    {

        public AuthorizationRequest(IIsoFieldFactory fieldFactory)
        {
            PrimaryAccountNumber = fieldFactory.GetIsoFieldLL("PrimaryAccountNumber", true);
            ProcessingCode = fieldFactory.GetIsoFieldFixed("ProcessingCode", 6, true);
            TransactionAmount = fieldFactory.GetIsoFieldFixed("Amount,Transaction", 12, true);
            SettlementAmount = fieldFactory.GetIsoFieldFixed("Amount,Settlement", 12, false);
            CardHolderBillingAmount = fieldFactory.GetIsoFieldFixed("Amount,CardHolderBilling", 12, false);
            TransmissionDateTime = fieldFactory.GetIsoFieldFixed("TransmissionDateTime", 10, true);
            SettlementConversionRate = fieldFactory.GetIsoFieldFixed("ConversionRate,Settlement", 8, false);
            CardHolderBillingConversionRate = fieldFactory.GetIsoFieldFixed("ConversionRate,CardHolderBilling", 8, true);
            STAN = fieldFactory.GetIsoFieldFixed("SystemTraceAuditNumber", 6, true);
            LocalTransactionTime = fieldFactory.GetIsoFieldFixed("Time,LocalTransaction", 6, false);
            LocalTransationDate = fieldFactory.GetIsoFieldFixed("Date,LocalTransaction", 4, false);
            ExpirationDate = fieldFactory.GetIsoFieldFixed("Date,Expiration", 4, false);
            SettlementDate = fieldFactory.GetIsoFieldFixed("Date,Settlement", 4, true);
            ConversionDate = fieldFactory.GetIsoFieldFixed("Date,Conversion", 4, true);
            MerchantType = fieldFactory.GetIsoFieldFixed("MerchantType", 4, true);
            PrimaryAccountNumberCountryCode = fieldFactory.GetIsoFieldFixed("PrimaryAccountNumber(PAN)CountryCode", 3, false);
            POSEntryMode = fieldFactory.GetIsoFieldFixed("PointOfService(POS)EntryMode", 3, true);
            CardSequenceNumber = fieldFactory.GetIsoFieldFixed("CardSequenceNumber", 3, false);
            POS_PIN_CaptureCode = fieldFactory.GetIsoFieldFixed("PointOfService(POS)PersonalIDNumber(PIN)CaptureCode", 2, false);
            TransactionFeeAmount = fieldFactory.GetIsoFieldFixed("Amount,TransactionFee", 9, false);
            AcquiringInstitutionIdCode = fieldFactory.GetIsoFieldLL("AcquiringInstitutionIdCode", true);
            ForwardingInstitutionIdCode = fieldFactory.GetIsoFieldLL("ForwardingInstitutionIdCode", false);
            Track2Data = fieldFactory.GetIsoFieldLL("Track2Data", false);
            RetrievalReferenceNumber = fieldFactory.GetIsoFieldFixed("RetrievalReferenceNumber", 12, false);
            CardAcceptorTerminalId = fieldFactory.GetIsoFieldFixed("CardAcceptorTerminalId", 8, false);
            CardAcceptorIdCode = fieldFactory.GetIsoFieldFixed("CardAcceptorIdCode", 15, false);
            CardAcceptorNameLocation = fieldFactory.GetIsoFieldFixed("CardAcceptorNameLocation", 40, false);
            Track1Data = fieldFactory.GetIsoFieldLL("Track1Data", false);
            AdditionalDataPrivateUse = fieldFactory.GetIsoFieldLLL("AdditionalDataPrivateUse", true);
            TransactionCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Transaction", 3, true);
            SettlementCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,Settlement", 3, false);
            CardHolderBillingCurrencyCode = fieldFactory.GetIsoFieldFixed("CurrencyCode,CardHolderBilling", 3, true);
            PINData = fieldFactory.GetIsoFieldFixed("PersonalIdNumber(PIN)Data", 8, false);
            SecurityRelatedControlInformation = fieldFactory.GetIsoFieldFixed("SecurityRelatedControlInformation", 16, false);
            AdditionalAmounts = fieldFactory.GetIsoFieldLLL("AdditonalAmounts", false);
            ICCSystemRelatedData = fieldFactory.GetIsoFieldLLL("IntegratedCircuitCard(ICC)SystemRelatedData", false);
            PaymentAccountData = fieldFactory.GetIsoFieldLLL("PaymentAccountData", false);
            POSData = fieldFactory.GetIsoFieldLLL("PointOfService(POS)Data", true);
            INFData = fieldFactory.GetIsoFieldLLL("IntermediateNetworkFacility(INF)Data", false);
            NetworkData = fieldFactory.GetIsoFieldLLL("NetworkData", true);
            DigitalPaymentData = fieldFactory.GetIsoFieldLLL("DigitalPaymentData", false);
            AdditionalTransactionReferenceData = fieldFactory.GetIsoFieldLLL("AdditionalTransactionReferenceData", false);
            AdditionalDataNationalUse = fieldFactory.GetIsoFieldLLL("AdditionalDataNationalUse", false);
            RecordData = fieldFactory.GetIsoFieldLLL("RecordData", false);
            MemberDefinedData = fieldFactory.GetIsoFieldLLL("MemberDefinedData", false);
            NewPINData = fieldFactory.GetIsoFieldFixed("NewPINData", 8, false);
            PrivateData = fieldFactory.GetIsoFieldLLL("PrivateData", false);
        }
        public IField AcquiringInstitutionIdCode { get; private set; }
        public IField AdditionalAmounts { get; private set; }
        public IField AdditionalDataNationalUse { get; private set; }
        public IField AdditionalDataPrivateUse { get; private set; }
        public IField AdditionalTransactionReferenceData { get; private set; }
        public IField CardAcceptorIdCode { get; private set; }
        public IField CardAcceptorNameLocation { get; private set; }
        public IField CardAcceptorTerminalId { get; private set; }
        public IField CardHolderBillingAmount { get; private set; }
        public IField CardHolderBillingConversionRate { get; private set; }
        public IField CardHolderBillingCurrencyCode { get; private set; }
        public IField CardSequenceNumber { get; private set; }
        public IField ConversionDate { get; private set; }
        public IField DigitalPaymentData { get; private set; }
        public IField ExpirationDate { get; private set; }
        public IField ForwardingInstitutionIdCode { get; private set; }
        public IField ICCSystemRelatedData { get; private set; } //Integrated Circuit Card
        public IField INFData { get; private set; } //Intermediate Network Facility
        public bool IsAdviceMessage { get { return false; } }
        //TODO: Try to use Flagged enum for these 3 properties below
        public bool IsRepeatRequest { get { return false; } }
        public bool IsResponseMessage { get { return false; } }
        public IField LocalTransactionTime { get; private set; }
        public IField LocalTransationDate { get; private set; }
        public IField MemberDefinedData { get; private set; }
        public IField MerchantType { get; private set; }
        public string MessageId { get { return string.Concat(STAN.Value, MessageTypeIdentifier); } }
        public string MessageTypeIdentifier { get { return MessageTypeIdentifiers.AuthorizationRequest; } }
        public IField NetworkData { get; private set; }
        public IField NewPINData { get; private set; }
        public IField PINData { get; private set; }
        public IField POSData { get; private set; }
        public IField POSEntryMode { get; private set; }
        public IField POS_PIN_CaptureCode { get; private set; }
        public IField PaymentAccountData { get; private set; }
        public IField PrimaryAccountNumber { get; private set; }
        public IField PrimaryAccountNumberCountryCode { get; private set; }
        public IField PrivateData { get; private set; }
        public IField ProcessingCode { get; private set; }
        public IField RecordData { get; private set; }
        public IField RetrievalReferenceNumber { get; private set; }
        public IField STAN { get; private set; }
        public IField SecurityRelatedControlInformation { get; private set; }
        public IField SettlementAmount { get; private set; }
        public IField SettlementConversionRate { get; private set; }
        public IField SettlementCurrencyCode { get; private set; }
        public IField SettlementDate { get; private set; }
        public IField Track1Data { get; private set; }
        public IField Track2Data { get; private set; }
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
                { DataElement.DE6, SettlementAmount },
                { DataElement.DE7, CardHolderBillingAmount },
                { DataElement.DE9, SettlementConversionRate },
                { DataElement.DE10, CardHolderBillingConversionRate },
                { DataElement.DE11, STAN },
                { DataElement.DE12, LocalTransactionTime },
                { DataElement.DE13, LocalTransationDate },
                { DataElement.DE14, ExpirationDate },
                { DataElement.DE15, SettlementDate },
                { DataElement.DE16, ConversionDate },
                { DataElement.DE18, MerchantType },
                { DataElement.DE20, PrimaryAccountNumberCountryCode },
                { DataElement.DE22, POSEntryMode },
                { DataElement.DE23, CardSequenceNumber },
                { DataElement.DE26, POS_PIN_CaptureCode },
                { DataElement.DE28, TransactionFeeAmount },
                { DataElement.DE32, AcquiringInstitutionIdCode },
                { DataElement.DE33, ForwardingInstitutionIdCode },
                { DataElement.DE35, Track2Data },
                { DataElement.DE37, RetrievalReferenceNumber },
                { DataElement.DE41, CardAcceptorTerminalId },
                { DataElement.DE42, CardAcceptorIdCode },
                { DataElement.DE43, CardAcceptorNameLocation },
                { DataElement.DE45, Track1Data },
                { DataElement.DE48, AdditionalDataPrivateUse },
                { DataElement.DE49, TransactionCurrencyCode },
                { DataElement.DE50, SettlementCurrencyCode },
                { DataElement.DE51, CardHolderBillingCurrencyCode },
                { DataElement.DE52, PINData },
                { DataElement.DE53, SecurityRelatedControlInformation },
                { DataElement.DE54, AdditionalAmounts },
                { DataElement.DE55, ICCSystemRelatedData },
                { DataElement.DE56, PaymentAccountData },
                { DataElement.DE61, POSData },
                { DataElement.DE62, INFData },
                { DataElement.DE63, NetworkData },
                { DataElement.DE104, DigitalPaymentData },
                { DataElement.DE108, AdditionalTransactionReferenceData },
                { DataElement.DE112, AdditionalDataNationalUse },
                { DataElement.DE120, RecordData },
                { DataElement.DE124, MemberDefinedData },
                { DataElement.DE125, NewPINData },
                { DataElement.DE127, PrivateData }
            };

            return fields;
        }
    }
}
