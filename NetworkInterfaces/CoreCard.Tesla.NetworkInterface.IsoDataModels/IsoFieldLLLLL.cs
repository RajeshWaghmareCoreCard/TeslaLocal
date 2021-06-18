using System;
using System.Globalization;
using System.Text;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    /// <summary>
    ///     Represents an ISO-8583 message field of length upto 99999.
    /// </summary>
    [Serializable]
    internal class IsoFieldLLLLL : IsoField
    {
        #region Fields

        private string _value;

        #endregion Fields

        #region Public Constructors

        public IsoFieldLLLLL(string name, bool isMandatory)
: base(name, IsoFieldType.LLLLLVar, isMandatory)
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

                if (value.Length > 99999)
                    throw new Exception(
                        string.Format(CultureInfo.InvariantCulture, "Attempt to set a string larger than 99999 characters in LLLLLVAR field {0}. Error Value: [{1}]", Name, value));

                _value = value;

                if (!string.IsNullOrEmpty(_value))
                    IsSet = true;
            }
        }

        public override int FromBytes(byte[] data, int index)
        {
            try
            {
                string strLen = Encoding.UTF8.GetString(data, index, 5);
                Length = Convert.ToInt32(strLen, CultureInfo.InvariantCulture);
                Value = Encoding.UTF8.GetString(data, index + 5, Length);

                return Value.Length + 5;
            }
            catch (Exception e)
            {
                throw new Exception("Exception parsing LLLVar field named " + Name + "Exception:" + e);
            }
        }
        public override byte[] GetDataBytes()
        {
            var data = Value.Length.ToString(CultureInfo.InvariantCulture).PadLeft(5, '0') + Value;
            return Encoding.ASCII.GetBytes(data);
        }

        #endregion Public Methods
    }
}