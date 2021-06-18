using System;
using System.Globalization;
using System.Text;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    /// <summary>
    /// Represents an ISO-8583 message field of length upto 99.
    /// </summary>
    internal class IsoFieldLL : IsoField
    {
        #region Fields

        private string _value;

        #endregion Fields

        #region Public Constructors

        public IsoFieldLL(string name, bool isMandatory)
: base(name, IsoFieldType.LLVar, isMandatory)
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

                if (value.Length > 99)
                    throw new Exception(
                        string.Format(CultureInfo.InvariantCulture, "Attempt to set a string larger than 99 characters in LLVAR field {0}. Error Value: [{1}]", Name, value));

                _value = value;

                if (!string.IsNullOrEmpty(_value))
                    IsSet = true;
            }
        }

        public override int FromBytes(byte[] data, int index)
        {
            try
            {
                Length = Convert.ToInt32(Encoding.UTF8.GetString(data, index, 2), CultureInfo.InvariantCulture);
                Value = Encoding.UTF8.GetString(data, index + 2, Length);

                return Value.Length + 2;
            }
            catch (Exception e)
            {
                throw new Exception("Exception parsing LLVar field named " + Name + "Exception:" + e);
            }
        }
        public override byte[] GetDataBytes()
        {
            var data = Value.Length.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + Value;
            return Encoding.ASCII.GetBytes(data);
        }

        #endregion Public Methods
    }
}