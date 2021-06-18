using System;
using System.Globalization;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    internal class BinaryIsoFieldFixed : BinaryIsoField
    {
        private byte[] _binaryData;

        public BinaryIsoFieldFixed(string name, int length, bool isMandatory) : base(name, IsoFieldType.Fixed, length, isMandatory)
        {
            BinaryData = new byte[length];
        }

        public override byte[] BinaryData
        {
            get { return _binaryData; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException(Name);

                if (value.Length != Length)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture,
                        "Attempt to set a fixed size (MaxLen {0}) data with a value (Len {1}) not matching the size of fixed field {2}. Error Value: [{3}]",
                        Length, value.Length, Name, value));

                _binaryData = value;

                if (_binaryData != null)
                    IsSet = true;
            }
        }

        public override int FromBytes(byte[] data, int index)
        {
            try
            {

                Buffer.BlockCopy(data, index, BinaryData, 0, Length);
                Value = Common.ByteHelper.GetHexRepresentationOfByteData(BinaryData);
                return Length; //because only Length number of fields were read...
            }
            catch (Exception e)
            {
                throw new Exception($"Exception parsing Fixed field named {Name} {e.Message}");
            }
        }

        public override byte[] GetDataBytes()
        {
            return BinaryData;
        }
    }
}
