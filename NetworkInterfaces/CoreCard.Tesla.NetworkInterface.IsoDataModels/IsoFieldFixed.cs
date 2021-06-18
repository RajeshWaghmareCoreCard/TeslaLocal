using System;
using System.Globalization;
using System.Text;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    internal class IsoFieldFixed : IsoField
    {
        #region Fields

        private string _value;

        #endregion Fields

        #region Public Constructors

        public IsoFieldFixed(string name, int length, bool isMandatory)
            : base(name, IsoFieldType.Fixed, length, isMandatory)
        {
        }

        #endregion Public Constructors

        #region Public Methods
        public override string Value
        {
            get { return IsSet ? _value : string.Empty; }
             set
            {
                if (value == null)
                    throw new ArgumentNullException(Name);

                if (value.Length != Length)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture,
                        "Attempt to set a fixed size (MaxLen {0}) string with a value (Len {1}) not matching the size of fixed field {2}. Error Value: [{3}]",
                        Length, value.Length, Name, value));

                _value = value;

                if (!string.IsNullOrEmpty(_value))
                    IsSet = true;
            }
        }

        public override int FromBytes(byte[] data, int index)
        {
            try
            {
                Value = Encoding.UTF8.GetString(data, index, Length);

                return Value.Length;
            }
            catch (Exception e)
            {
                throw new Exception("Exception parsing Fixed field named " + Name + " " + e.Message);
            }
        }

        public override byte[] GetDataBytes()
        {
            return Encoding.ASCII.GetBytes(Value);
        }

        #endregion Public Methods
    }
}