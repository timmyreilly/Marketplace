using System.Threading.Tasks;
using Marketplace.Domain;
using Marketplace.Framework;

namespace Marketplace.Api
{
    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly IEntityStore _store; 
        private ICurrencyLookup _currencyLookup; 

        public ClassifiedAdsApplicationService(IEntityStore store, ICurrencyLookup currencyLookup)
        {
            _store = store; 
            _currencyLookup = currencyLookup; 
        }

        public Task Handle(object command)
        {
            throw new System.NotImplementedException();
        }
    }
}