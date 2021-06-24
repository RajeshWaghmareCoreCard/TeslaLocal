using CoreCard.Tesla.Falcon.DataModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.DataModels.Model
{
    public class AddressAddDTO
    {
        public int AddressType { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        public static Address MapToAddress(AddressAddDTO addressAddDTO)
        {
            Address address = new Address();
            address.addresstype = addressAddDTO.AddressType;
            address.housenumber = addressAddDTO.HouseNumber;
            address.street = addressAddDTO.Street;
            address.city = addressAddDTO.City;
            address.state = addressAddDTO.State;
            address.zipcode = addressAddDTO.Zipcode;
            return address;
        }
    }
}
