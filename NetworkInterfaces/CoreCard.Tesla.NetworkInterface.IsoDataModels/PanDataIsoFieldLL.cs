using System;
using CoreCard.Tesla.Common;

namespace CoreCard.Tesla.NetworkInterface.IsoDataModels
{
    internal class PanDataIsoFieldLL : IsoFieldLL
    {
        public PanDataIsoFieldLL(string name, bool isMandatory) : base(name, isMandatory)
        {
        }

        public override string ToString()
        {
            return base.ToString().MaskPan();
        }
    }
}
