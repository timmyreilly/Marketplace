using System;
using System.Threading.Tasks;
using Marketplace.Contracts;
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

        public async Task Handle(object command)
        {
            ClassifiedAd classifiedAd;
            switch (command)
            {
                case ClassifiedAds.V1.Create cmd:
                    if (await _store.Exists<ClassifiedAd>(cmd.Id.ToString()))
                    {
                        throw new InvalidOperationException($"Entity with id {cmd.Id } already exists");
                    }

                    classifiedAd = new ClassifiedAd(new ClassifiedAdId(cmd.Id), new UserId(cmd.OwnerId));

                    await _store.Save(classifiedAd);
                    break;

                case ClassifiedAds.V1.SetTitle cmd:
                    classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
                    if (classifiedAd == null)
                    {
                        throw new InvalidOperationException($"Entity with id {cmd.Id} cannot be found");
                    }
                    classifiedAd.SetTitle(ClassifiedAdTitle.FromString(cmd.Title));
                    await _store.Save(classifiedAd);
                    break;

                case ClassifiedAds.V1.UpdateText cmd:
                    classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
                    if (classifiedAd == null)
                    {
                        throw new InvalidOperationException($"Entity with id {cmd.Id} cannot be found");
                    }
                    classifiedAd.UpdateText(ClassifiedAdText.FromString(cmd.Text));
                    await _store.Save(classifiedAd);
                    break;

                // Let's not repeat ourselves! 

                case ClassifiedAds.V1.UpdatePrice cmd:
                    await HandleUpdate(cmd.Id, c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup)));
                    break;

                case ClassifiedAds.V1.RequestToPublish cmd:
                    await HandleUpdate(cmd.Id, c => c.RequestToPublish());
                    break;

                default:
                    throw new InvalidOperationException($"Command type { command.GetType().FullName } is unknown");
            }
        }

        private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
        {
            var classifiedAd = await _store.Load<ClassifiedAd>(classifiedAdId.ToString());
            if (classifiedAd == null)
            {
                throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
            }

            operation(classifiedAd);

            await _store.Save(classifiedAd);

        }
    }
}