using CoreCard.Tesla.Falcon.Services.Purchase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CockroachDb.Business.Payment;
namespace CoreCard.Tesla.Falcon.Services
{
  public static   class RegisterBAL
    {

        public static IServiceCollection RegisterBALDI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICustomerBAL, CustomerBAL>();
            serviceCollection.AddScoped<IAccountBAL, AccountBAL>();
            serviceCollection.AddScoped<IAddressBAL, AddressBAL>();
            serviceCollection.AddScoped<IEmbossingBAL, EmbossingBAL>();
            serviceCollection.AddScoped<ILoyaltyPlanBAL, LoyaltyPlanBAL>();
            serviceCollection.AddScoped<IAPILogBAL, APILogBAL>();
            //serviceCollection.AddScoped<IPaymentBAL, PaymentBAL>();
            serviceCollection.AddScoped<IPlanSegmentBAL, PlanSegmentBAL>();
            serviceCollection.AddScoped<ILogArTxnBAL, LogArTxnBAL>();
            serviceCollection.AddScoped<ICBLogBAL, CBLogBAL>();
            serviceCollection.AddScoped<IPurchaseBAL, PurchaseBAL>();
            serviceCollection.AddScoped<ITransInAcctBAL, TransInAcctBAL>();
            serviceCollection.AddScoped<ITransactionBAL, TransactionBAL>();
            //serviceCollection.AddScoped<ITansInAcctBAL, TransInAcctBAL>();
            //serviceCollection.AddScoped<IPlansegmentBAL, PlansegmentBAL>();
            //serviceCollection.AddScoped<ILogArTxnBAL, LogArTxnBAL>();
            //serviceCollection.AddScoped<ICBLogBAL, CBLogBAL>();
            //serviceCollection.AddScoped < ILogArTxnBAL, LogArTxnBAL>();
            //serviceCollection.AddScoped<ICBLogBAL,CBLogBAL > ();
            //serviceCollection.AddScoped<CockroachDb.Business.Payment.ITransInAcctBAL,CockroachDb.Business.Payment.TransInAcctBAL> ();
            return serviceCollection;

        }
    }
}
