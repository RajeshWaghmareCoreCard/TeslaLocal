using CoreCard.Tesla.Falcon.Services;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAdapter;
using CoreCard.Tesla.Falcon.ADORepository;

namespace CoreCard.Tesla.Falcon.Services
{
    public class AddressBAL : IAddressBAL
    {
        IADOAddressRepository _iADOAddressRepository;
        public AddressBAL(IADOAddressRepository iADOAddressRepository)
        {
            _iADOAddressRepository = iADOAddressRepository;
        }

        //public async Task<Address> AddAddressAsync(AddressAddDTO addressAddDTO, Guid customerId)
        //{
        //    Address newAddres = AddressAddDTO.MapToAddress(addressAddDTO);
        //    newAddres.customerid = customerId;
        //    newAddres.addresstype = 0;
        //    //newAddres.addressid = Guid.NewGuid();
        //    return await _addressRepository.AddAsync(newAddres);
        //}

        public void Insert(Address t, IDataBaseCommand dataBaseCommand)
        {
            _iADOAddressRepository.Insert(t, dataBaseCommand);
        }
    }
}
