using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.DataModels.Model
{
    public class BaseResponseDTO
    {
        public long DataLayerTime { get; set; }
        public long ControllerLayerTime { get; set; }

        public long TotalAPITime { get; set; }

        public Object BaseEntityInstance { get; set; }

    }
}
