using System;
using System.Globalization;
using System.Text;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    /// <summary>
    /// Represents an ISO-8583 message field of length upto 9 .
    /// </summary>
    internal class IsoFieldL : IsoField
    {
        #region Fields

        private string _value;

        #endregion Fields

        #region Public Constructors

        public IsoFieldL(string name, bool isMandatory)
            : base(name, IsoFieldType.LVar, isMandatory)
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

                if (value.Length > 9)
                    throw new Exception(
                        string.Format(CultureInfo.InvariantCulture, "Attempt to set a string larger than 9 characters in LVAR field {0}. Error Value: [{1}]", Name, value));

                _value = value;

                if (!string.IsNullOrEmpty(_value))
                    IsSet = true;
            }
        }
        public override int FromBytes(byte[] data, int index)
        {
            try
            {
                Length = Convert.ToInt32(Encoding.UTF8.GetString(data, index, 1), CultureInfo.InvariantCulture);
                Value = Encoding.UTF8.GetString(data, index + 1, Length);

                return Value.Length + 1;
            }
            catch (Exception e)
            {
                throw new Exception("Exception parsing IsoFieldLV field named " + Name + "Exception:" + e);
            }
        }

        public override byte[] GetDataBytes()
        {
            string data = Value.Length + Value;
            return Encoding.ASCII.GetBytes(data);
        }

        #endregion Public Methods
    }
}