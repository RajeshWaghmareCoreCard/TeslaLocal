using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.ADORepository
{
    public static class RegisterADORepository
    {
        public static IServiceCollection RegisterADORepositoryDI(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddScoped<IADOAccountRepository, ADOAccountRepository>();
            serviceCollection.AddScoped<IADOTransactionRepository, ADOTransactionRepository>();
            serviceCollection.AddScoped<IADOEmbossingRepository, ADOEmbossingRepository>();
            serviceCollection.AddScoped<IADOPlansegmentRepository, ADOPlanSegmentRepository>();
            serviceCollection.AddScoped<IADOCBLogRepository, ADOCBLogRepository>();
            serviceCollection.AddScoped<IADOLogArTxnRepository, ADOLogArTxnRepository>();
            serviceCollection.AddScoped<IADOTranInAcctRepository, ADOTranInAcctRepository>();
            serviceCollection.AddScoped<IADOLoyaltyPlanRepository, ADOLoyaltyPlanRepository>();
            serviceCollection.AddScoped<IBaseCockroachADO, BaseCockroachADO>();
            serviceCollection.AddScoped<IADOCustomerRepository, ADOCustomerRepository>();
            serviceCollection.AddScoped<IADOAPILogRepository, ADOAPILogRepository>();
            serviceCollection.AddScoped<IADOAddressRepository, ADOAddressRepository>();
            //serviceCollection.AddScoped<CockroachDb.Repository.ITrans_In_AcctRepository,CockroachDb.Repository.Trans_In_AcctRepository > ();
            return serviceCollection;
        }
    }
}
