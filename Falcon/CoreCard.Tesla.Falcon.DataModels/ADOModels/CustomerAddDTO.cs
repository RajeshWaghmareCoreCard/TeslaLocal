using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;

namespace CoreCard.Tesla.Falcon.DataModels.Model
{
    public class CustomerAddDTO
    {
        public string ssn { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string ccregion { get; set; }
        public AddressAddDTO CustomerAddress { get; set; }

        public static Customer MapToCustomer(CustomerAddDTO customerAddDTO)
        {
            Customer customer = new Customer();
            customer.ssn = customerAddDTO.ssn;
            customer.firstname = customerAddDTO.firstname;
            customer.lastname = customerAddDTO.lastname;
            customer.ccregion = customerAddDTO.ccregion;
            return customer;
        }
    }
}
