namespace CoreCard.Tesla.Falcon.DataModels.Repository
{
    public class CustomerModel
    {
        public string CustomerId { get; set; }
        public string InstitutionId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string SSN { get; set; }
        public bool IsActive { get; set; }

    }
}