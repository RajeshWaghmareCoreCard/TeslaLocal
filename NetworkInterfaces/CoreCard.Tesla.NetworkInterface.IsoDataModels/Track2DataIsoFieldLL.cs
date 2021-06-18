using System;
using CoreCard.Tesla.Common;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    internal class Track2DataIsoFieldLL : IsoFieldLL
    {
        public Track2DataIsoFieldLL(string name, bool isMandatory) : base(name, isMandatory)
        {
        }

        public override string ToString()
        {
            return base.ToString().MaskTrack2Data();
        }
    }
}
