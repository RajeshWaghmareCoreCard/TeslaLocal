namespace CoreCard.Tesla.Tokenization.DataModels.DtoTypes
{
    public class ModuleKey
    {
        public string ModuleKeyId { get; set; }
        public string PublicKey { get; set; }
        public string ModuleId { get; set; }
        public bool Active { get; set; }

        public ModuleKey ToModel()
        {
            ModuleKey toObject = new ModuleKey();
            toObject.ModuleKeyId = ModuleKeyId;
            toObject.PublicKey = PublicKey;
            toObject.ModuleId = ModuleId;
            return toObject;
        }
    }
}