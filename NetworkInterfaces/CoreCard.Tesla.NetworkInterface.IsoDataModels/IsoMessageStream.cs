using System;
using System.Collections.Generic;
using System.Text;
using CoreCard.Tesla.Common;
using Microsoft.Extensions.Logging;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    internal class IsoMessageStream : IIsoMessageStream
    {
        private readonly IIsoMessageFactory _isoMessageFactory;
        #region Private Fields
        private readonly ILogger<IsoMessageStream> _logger;

        #endregion Private Fields

        public IsoMessageStream(ILogger<IsoMessageStream> logger, IIsoMessageFactory isoMessageFactory)
        {
            _logger = logger;
            _isoMessageFactory = isoMessageFactory;
        }

        #region Public Methods

        public IIsoMessage BuildIsoMessageFromByteData(byte[] data, out string logMessage, int skipBytes = 0)
        {
            var logBuilder = new StringBuilder();
            logMessage = "";
            try
            {
                string messageType = Encoding.ASCII.GetString(data, 0, 4);
                _logger.LogInformation($"MTI of received message:{messageType}");

                IIsoMessage message = _isoMessageFactory.GetIsoMessage(messageType);
                if (message == null)
                    return null;

                var primaryBitMap = new byte[8];
                var secondaryBitMap = new byte[8];

                logBuilder.AppendFormat("MTI: {0}", message.MessageTypeIdentifier);

                Dictionary<DataElement, IField> fields = message.Fields();
                //check if secondary bitmap is present if yes
                int offset = skipBytes + 4;

                Buffer.BlockCopy(data, offset, primaryBitMap, 0, 8);
                offset += 8;

                // var primaryBitmapString = Encoding.ASCII.GetString(data, offset, 16);
                // _primaryBitMap = ByteHelper.GetBitmapFromHexString(primaryBitmapString);
                // offset += 16;

                logBuilder.AppendFormat("\nPrimary Bitmap: {0}", BitConverter.ToString(primaryBitMap));

                int maxElements;
                if (((primaryBitMap[0] >> 7) & 0x01) == 0x01)
                {
                    maxElements = 128;
                    Buffer.BlockCopy(data, offset, secondaryBitMap, 0, 8);
                    offset += 8;

                    // var secondaryBitmapString = Encoding.ASCII.GetString(data, offset, 16);
                    // _secondaryBitMap = ByteHelper.GetBitmapFromHexString(secondaryBitmapString);
                    // offset += 16;

                    logBuilder.AppendFormat("\nSecondary Bitmap: {0}", BitConverter.ToString(secondaryBitMap));
                }
                else
                    maxElements = 64;

                for (int i = 1; i < maxElements; i++)
                {
                    int len = 0;
                    DataElement fieldName = (DataElement)i;

                    if (fields.ContainsKey(fieldName))
                    {
                        if (i <= 64)
                        {
                            if ((primaryBitMap[(i - 1) / 8] >> (8 - (i - 8 * ((i - 1) / 8))) & 0x01) == 0x01)
                            {
                                len = fields[fieldName].FromBytes(data, offset);
                                logBuilder.AppendFormat("\nDE-[{0,-3}]:[{1,-39}]=> Value: [{2}] ", i, fields[fieldName].Name, fields[fieldName]);
                            }
                        }
                        else if ((secondaryBitMap[(i - 64 - 1) / 8] >> (8 - (i - 64 - 8 * ((i - 64 - 1) / 8))) & 0x01) == 0x01)
                        {
                            len = fields[fieldName].FromBytes(data, offset);
                            logBuilder.AppendFormat("\nDE-[{0,-3}]:[{1,-39}]=> Value: [{2}] ", i, fields[fieldName].Name, fields[fieldName]);
                        }

                        offset += len;
                    }
                }

                logMessage = logBuilder.ToString();
                return message;

            }
            catch (Exception e)
            {
                logMessage = logBuilder.ToString();
                _logger.LogError($"Error in BuildIsoMessageFromByteData:{e}\n DumpBuilder State: {logBuilder}");
            }
            return null;
        }

        public byte[] ConvertIsoMessageToByteData(IIsoMessage message, out string logMessage)
        {
            var primaryBitMap = new byte[8];
            var secondaryBitMap = new byte[8];

            _logger.LogDebug("In ToDataBytes: Message being parsed: {@message}", message);

            var logBuilder = new StringBuilder();
            var dataElementsBuilder = new StringBuilder();
            Dictionary<DataElement, IField> fields = message.Fields();
            List<byte> rawDataList = new();
            try
            {
                logMessage = "";
                logBuilder.AppendFormat("\nMTI: {0}", message.MessageTypeIdentifier);

                //StringBuilder tempBytes = new();
                for (int i = 2; i < 128; i++)
                {
                    DataElement fieldName = (DataElement)i;
                    if (fields.ContainsKey(fieldName))
                    {
                        if (fields[fieldName].IsMandatory)
                        {
                            if (!fields[fieldName].IsSet)
                                throw new Exception($"No value set for Mandatory field {i}: { fields[fieldName].Name}");
                        }
                        else
                        {
                            if (!fields[fieldName].IsSet)
                                continue;
                        }
                        if (i <= 64)
                        {
                            primaryBitMap[(i - 1) / 8] |= (byte)(0x80 >> ((i - 1) % 8));
                        }
                        else
                        {
                            primaryBitMap[0] |= 0x80;
                            secondaryBitMap[(i - 64 - 1) / 8] |= (byte)(0x80 >> ((i - 64 - 1) % 8));
                        }

                        //tempBytes = tempBytes.Append(fields[fieldName].ToString());

                        rawDataList.AddRange(fields[fieldName].GetDataBytes());

                        dataElementsBuilder.AppendFormat("\nDE-[{0,-3}]:[{1,-39}]=> Value: [{2}]", i, fields[fieldName].Name, fields[fieldName]);
                    }
                }

                byte[] rawDataBytes = rawDataList.ToArray();

                //var primaryBitmapString = ByteHelper.GetHexRepresentationOfBitmap(_primaryBitMap);
                //var primaryBitmapBytes = Encoding.ASCII.GetBytes(primaryBitmapString);

                logBuilder.AppendFormat("\nPrimary Bitmap: {0}", BitConverter.ToString(primaryBitMap));

                byte[] returnBytes;
                if (((primaryBitMap[0] >> 7) & 0x01) != 0x01)
                    returnBytes = ByteHelper.CombineByteArrays(primaryBitMap, rawDataBytes);
                else
                {
                    //var secondaryBitmapString = ByteHelper.GetHexRepresentationOfBitmap(_secondaryBitMap);
                    //var secondaryBitmapBytes = Encoding.ASCII.GetBytes(secondaryBitmapString);

                    logBuilder.AppendFormat("\nSecondary Bitmap: {0}", BitConverter.ToString(secondaryBitMap));

                    returnBytes = ByteHelper.CombineByteArrays(primaryBitMap, secondaryBitMap, rawDataBytes);
                }

                logBuilder.AppendLine(dataElementsBuilder.ToString());

                logMessage = logBuilder.ToString();

                //After appending the bitmaps and the data elements append the MTI bytes at the beginning.
                byte[] mtiBytes = Encoding.ASCII.GetBytes(message.MessageTypeIdentifier);

                returnBytes = ByteHelper.CombineByteArrays(mtiBytes, returnBytes);

                return returnBytes;

            }
            catch (Exception e)
            {
                _logger.LogError($"Error in ToBytes:{e}\n DumpBuilder State: {logBuilder}\nDataElementBuilderState: {dataElementsBuilder}");
                throw;
            }
        }

        #endregion Public Methods
    }
}