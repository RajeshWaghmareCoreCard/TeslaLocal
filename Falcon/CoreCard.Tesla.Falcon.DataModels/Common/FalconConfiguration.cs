using System;

namespace CoreCard.Tesla.Falcon.DataModels.Common
{
    public class FalconAppSetting
    {
        public string TokenizationURL { get; set; }
        public string ModuleKeyId { get; set; }
        public string CacheType { get; set; }
        public double AesRotationMinutes { get; set; }
    }
}
