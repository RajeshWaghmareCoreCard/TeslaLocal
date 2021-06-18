namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    public interface IIsoMessageFactory
    {
        IIsoMessage GetIsoMessage(string MTI);

    }
}