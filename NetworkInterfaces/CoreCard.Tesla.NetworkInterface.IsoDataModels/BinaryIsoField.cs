using System;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public abstract class BinaryIsoField : IsoField
    {
        protected BinaryIsoField(string name, IsoFieldType type, bool isMandatory) : base(name, type, isMandatory)
        {
        }

        protected BinaryIsoField(string name, IsoFieldType type, int length, bool isMandatory) : base(name, type, length, isMandatory)
        {
        }

        public abstract byte[] BinaryData { get; protected set; }

        /// <summary>
        /// Returns the string representation of the binary data held by this data element.
        /// </summary>
        /// <remarks>In case of binary data elements, the Value property should return
        /// only the string representation of the byte data and the <see cref="BinaryData"/>
        /// property should be used to access the actual binary data.</remarks>
        /// TODO: Confirm if we need this...
        public override string Value
        {
            get
            {
                return ToString();
            }
            set
            {
                BinaryData = Common.ByteHelper.GetBytesFromHexString(value);
            }
        }

        public override string ToString()
        {
            return IsSet ? BitConverter.ToString(BinaryData) : string.Empty;
        }
    }
}

