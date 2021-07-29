﻿using CoreCard.Tesla.Falcon.DataModels.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public interface IADOEmbossingRepository
    {
        Embossing GetEmbossingByCardNumber(string cardnumber);
        void Insert(Embossing embossing);
    }
}
