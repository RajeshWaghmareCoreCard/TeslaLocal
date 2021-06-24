using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DataModels.Entity;
using CoreCard.Tesla.Falcon.DataModels.Model;
//using CockroachDb.Repository;
using DBAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.Services
{
    public class CustomerBAL :  ICustomerBAL
    {
       // private readonly ICustomerRepository _customerRepository;
        private readonly IAddressBAL _addressBAL;
        private readonly IADOCustomerRepository _iADOCustomerRepository;
        public CustomerBAL( IAddressBAL addressBAL, IADOCustomerRepository iADOCustomerRepository) //: base(customerRepository)
        {
            //_customerRepository = customerRepository;
            _addressBAL = addressBAL;
            _iADOCustomerRepository = iADOCustomerRepository;
        }

        //public async Task<Customer> AddCustomerAsync(CustomerAddDTO customerDTO)
        //{
        //    Customer customer = CustomerAddDTO.MapToCustomer(customerDTO);
        //    //customer.customerid = Guid.NewGuid();
        //    customer.ssn = SSNGenerator.GenerateSSN();
        //    Customer newCustomer = await _customerRepository.AddAsync(customer);
        //    await _addressBAL.AddAddressAsync(customerDTO.CustomerAddress, newCustomer.customerid);
        //    return newCustomer;
        //}

        public Guid Insert(Customer t, IDataBaseCommand databaseCommand)
        {
            return _iADOCustomerRepository.Insert(t, databaseCommand);
        }

    }
}
