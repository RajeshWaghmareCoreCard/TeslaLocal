using System;
using System.Globalization;
using System.Text;
using CoreCard.Tesla.Common;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    internal class BinaryIsoFieldLLL : BinaryIsoField
    {
        private byte[] _binaryData;

        public BinaryIsoFieldLLL(string name, bool isMandatory) : base(name, IsoFieldType.LLLVar, isMandatory)
        {
        }

        public override byte[] BinaryData
        {
            get { return _binaryData; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException(Name);

                if (value.Length > 999)
                    throw new Exception(
                        string.Format(CultureInfo.InvariantCulture, "Attempt to set a string larger than 999 characters in LLLVAR field {0}. Error Value: [{1}]", Name, value));

                _binaryData = value;

                if (_binaryData != null)
                    IsSet = true;
            }
        }

        public override int FromBytes(byte[] data, int index)
        {
            try
            {
                string strLen = Encoding.UTF8.GetString(data, index, 3);
                Length = Convert.ToInt32(strLen, CultureInfo.InvariantCulture);

                BinaryData = new byte[Length];

                Buffer.BlockCopy(data, index + 3, BinaryData, 0, Length);
                Value = ByteHelper.GetHexRepresentationOfByteData(BinaryData);

                return Length + 3;
            }
            catch (Exception e)
            {
                throw new Exception("Exception parsing LLLVar field named " + Name + "Exception:" + e);
            }
        }

        public override byte[] GetDataBytes()
        {
            var dataByteLength = Encoding.ASCII.GetBytes(BinaryData.Length.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0'));

            return ByteHelper.CombineByteArrays(dataByteLength, BinaryData);
        }
    }
}
