namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IField
    {
        #region Public Properties

        bool IsMandatory { get; }

        bool IsSet { get; }

        int Length { get; }

        string Name { get; }

        IsoFieldType Type { get; }

        string Value { get; set; }

        #endregion Public Properties

        #region Methods

        /// <summary>
        ///     Frombytes Method.
        /// </summary>
        /// <param name="data">The byte[] data received from client</param>
        /// <param name="index">Starting point to read from.</param>
        /// <returns></returns>
        int FromBytes(byte[] data, int index);

        /// <summary>
        /// Returns the byte representation of the data that should be used to
        /// create the iso byte value for this datafield.
        /// </summary>
        /// <returns></returns>
        byte[] GetDataBytes();

        string ToString();

        #endregion Methods
    }
}