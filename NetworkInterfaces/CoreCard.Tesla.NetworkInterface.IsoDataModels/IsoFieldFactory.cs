namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public class IsoFieldFactory : IIsoFieldFactory
    {
        public IField GetBinaryIsoFieldFixed(string name, int length, bool isMandatory)
        {
            return new BinaryIsoFieldFixed(name, length, isMandatory);
        }

        public IField GetBinaryIsoFieldLLL(string name, bool isMandatory)
        {
            return new BinaryIsoFieldLLL(name, isMandatory);
        }

        public IField GetIsoFieldFixed(string name, int length, bool isMandatory)
        {
            return new IsoFieldFixed(name, length, isMandatory);
        }

        public IField GetIsoFieldL(string name, bool isMandatory)
        {
            return new IsoFieldL(name, isMandatory);
        }

        public IField GetIsoFieldLL(string name, bool isMandatory)
        {
            return new IsoFieldLL(name, isMandatory);
        }

        public IField GetIsoFieldLLL(string name, bool isMandatory)
        {
            return new IsoFieldLLL(name, isMandatory);
        }

        public IField GetIsoFieldLLLL(string name, bool isMandatory)
        {
            return new IsoFieldLLLL(name, isMandatory);
        }

        public IField GetIsoFieldLLLLL(string name, bool isMandatory)
        {
            return new IsoFieldLLLLL(name, isMandatory);
        }

        public IField GetPanDataIsoFieldLL(string name, bool isMandatory)
        {
            return new PanDataIsoFieldLL(name, isMandatory);
        }

        public IField GetTrack2DataIsoFieldLL(string name, bool isMandatory)
        {
            return new Track2DataIsoFieldLL(name, isMandatory);
        }
    }
}