using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Falcon.NpgRepository
{
    public static class RegisterNpgRepository
    {
        public static void RegisterNpgDatabase(this IServiceCollection services, string connectionString)
        {
            // var connStringBuilder = new NpgsqlConnectionStringBuilder() { IncludeErrorDetails = true };
            // connStringBuilder.Host = "localhost";
            // connStringBuilder.Port = 26257;6
            // connStringBuilder.SslMode = SslMode.Disable;
            // connStringBuilder.Username = "root";
            // connStringBuilder.Database = "falcon";
            services.AddTransient<IDatabaseConnectionResolver, DatabaseConnectionResolver>(x => new DatabaseConnectionResolver(connectionString));
        }
        public static IServiceCollection RegisterNpgRepositoryDI(this IServiceCollection serviceCollection)
        {

            //serviceCollection.AddScoped<IADOAccountRepository, ADOAccountRepository>();
            //serviceCollection.AddScoped<IADOTransactionRepository, ADOTransactionRepository>();
            //serviceCollection.AddScoped<IADOEmbossingRepository, ADOEmbossingRepository>();
            //serviceCollection.AddScoped<IADOPlansegmentRepository, ADOPlanSegmentRepository>();
            //serviceCollection.AddScoped<IADOCBLogRepository, ADOCBLogRepository>();
            //serviceCollection.AddScoped<IADOLogArTxnRepository, ADOLogArTxnRepository>();
            //serviceCollection.AddScoped<IADOTranInAcctRepository, ADOTranInAcctRepository>();
            //serviceCollection.AddScoped<IADOLoyaltyPlanRepository, ADOLoyaltyPlanRepository>();
            //serviceCollection.AddScoped<IBaseRepository, BaseCockroachADO>();
            //serviceCollection.AddScoped<IADOCustomerRepository, ADOCustomerRepository>();
            //serviceCollection.AddScoped<IADOAPILogRepository, ADOAPILogRepository>();
            //serviceCollection.AddScoped<IADOAddressRepository, ADOAddressRepository>();
            //serviceCollection.AddScoped<IADOPaymentRepository, ADOPaymentRepository>();
            serviceCollection.AddTransient<IADOAccountRepository, ADOAccountRepository>();
            serviceCollection.AddTransient<IADOTransactionRepository, ADOTransactionRepository>();
            serviceCollection.AddTransient<IADOEmbossingRepository, ADOEmbossingRepository>();
            serviceCollection.AddTransient<IADOPlansegmentRepository, ADOPlanSegmentRepository>();
            serviceCollection.AddTransient<IADOCBLogRepository, ADOCBLogRepository>();
            serviceCollection.AddTransient<IADOLogArTxnRepository, ADOLogArTxnRepository>();
            serviceCollection.AddTransient<IADOTranInAcctRepository, ADOTranInAcctRepository>();
            serviceCollection.AddTransient<IADOLoyaltyPlanRepository, ADOLoyaltyPlanRepository>();
            serviceCollection.AddTransient<IADOCustomerRepository, ADOCustomerRepository>();
            serviceCollection.AddTransient<IADOAPILogRepository, ADOAPILogRepository>();
            serviceCollection.AddTransient<IADOAddressRepository, ADOAddressRepository>();
            serviceCollection.AddTransient<IPurchaseUnit, PurchaseUnit>();
            //serviceCollection.AddScoped<CockroachDb.Repository.ITrans_In_AcctRepository,CockroachDb.Repository.Trans_In_AcctRepository > ();
            return serviceCollection;
        }
    }
}
