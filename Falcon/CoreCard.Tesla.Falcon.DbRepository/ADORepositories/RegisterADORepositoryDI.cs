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

            serviceCollection.AddTransient<IADOAccountRepository, ADOAccountRepository>();
            serviceCollection.AddTransient<IADOTransactionRepository, ADOTransactionRepository>();
            serviceCollection.AddTransient<IADOEmbossingRepository, ADOEmbossingRepository>();
            serviceCollection.AddTransient<IADOPlansegmentRepository, ADOPlanSegmentRepository>();
            serviceCollection.AddTransient<IADOCBLogRepository, ADOCBLogRepository>();
            serviceCollection.AddTransient<IADOLogArTxnRepository, ADOLogArTxnRepository>();
            serviceCollection.AddTransient<IADOTranInAcctRepository, ADOTranInAcctRepository>();
            serviceCollection.AddTransient<IADOLoyaltyPlanRepository, ADOLoyaltyPlanRepository>();
            serviceCollection.AddTransient<IBaseCockroachADO, BaseCockroachADO>();
            serviceCollection.AddTransient<IADOCustomerRepository, ADOCustomerRepository>();
            serviceCollection.AddTransient<IADOAPILogRepository, ADOAPILogRepository>();
            serviceCollection.AddTransient<IADOAddressRepository, ADOAddressRepository>();
            //serviceCollection.AddTransient<IADOPaymentRepository, ADOPaymentRepository>();
            //serviceCollection.AddScoped<CockroachDb.Repository.ITrans_In_AcctRepository,CockroachDb.Repository.Trans_In_AcctRepository > ();
            return serviceCollection;
        }
    }
}
