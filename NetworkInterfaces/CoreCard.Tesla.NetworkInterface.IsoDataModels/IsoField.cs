namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public abstract class IsoField : IField
    {

        #region Protected Constructors

        protected IsoField(string name, IsoFieldType type, bool isMandatory)
        {
            Name = name;
            Type = type;
            IsMandatory = isMandatory;
        }

        protected IsoField(string name, IsoFieldType type, int length, bool isMandatory)
        {
            Name = name;
            Type = type;
            Length = length;
            IsMandatory = isMandatory;
        }

        #endregion Protected Constructors

        #region Public Properties

        public bool IsMandatory { get; protected set; }

        public virtual bool IsSet { get; protected set; }

        public int Length { get; protected set; }

        public string Name { get; protected set; }

        public IsoFieldType Type { get; protected set; }

        public abstract string Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public abstract int FromBytes(byte[] data, int index);

        public abstract byte[] GetDataBytes();

        public override string ToString()
        {
            return Value;
        }

        #endregion Public Methods
    }
}