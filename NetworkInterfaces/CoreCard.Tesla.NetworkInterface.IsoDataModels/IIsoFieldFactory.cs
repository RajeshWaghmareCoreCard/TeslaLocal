namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IIsoFieldFactory
    {
        IField GetBinaryIsoFieldFixed(string name, int length, bool isMandatory);
        IField GetBinaryIsoFieldLLL(string name, bool isMandatory);
        IField GetIsoFieldFixed(string name, int length, bool isMandatory);
        IField GetIsoFieldL(string name, bool isMandatory);
        IField GetIsoFieldLL(string name, bool isMandatory);
        IField GetIsoFieldLLL(string name, bool isMandatory);
        IField GetIsoFieldLLLL(string name, bool isMandatory);
        IField GetIsoFieldLLLLL(string name, bool isMandatory);
        IField GetPanDataIsoFieldLL(string name, bool isMandatory);
        IField GetTrack2DataIsoFieldLL(string name, bool isMandatory);
    }
}

