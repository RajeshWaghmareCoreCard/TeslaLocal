using System;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;

namespace CoreCard.Tesla.Tokenization.DataModels.DtoTypes
{
    public class ModuleSessionModel
    {
        public ModuleSessionModel()
        {
            KeyDetails=new KeyDetails();
        }
        public string SessionId { get; set; }
        public string ModuleId { get; set; } 
        public KeyDetails KeyDetails { get; set; }
        public DateTime SessionExpiryDate { get; set; }
    }
}