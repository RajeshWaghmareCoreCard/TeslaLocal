using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;

namespace CoreCard.Tesla.Tokenization.DataModels.DtoModels
{
    public class ModulePermissionModel
    {
        public string ModulePermissionId { get; set; }
        public string ModuleId { get; set; }
        public string TokenFamilyId { get; set; }
        public bool IsTokenizationAllowed { get; set; }
        public bool IsDetokenizationAllowed { get; set; }
        public bool NotifyTokenizationOperation { get; set; }
        public bool NotifyDetokenizationOperation { get; set; }
    }
}