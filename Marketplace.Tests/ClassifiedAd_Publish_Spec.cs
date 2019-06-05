using System;
using Marketplace.Domain;
using Xunit;

namespace Marketplace.Tests
{
    public class ClassifiedAd_Publish_Spec
    {
        private readonly ClassifiedAd _classifiedAd;

        public ClassifiedAd_Publish_Spec()
        {
            _classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(Guid.NewGuid()),
                new UserId(Guid.NewGuid())
            );
        }

        [Fact]
        public void Can_publish_a_valid_ad()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            _classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy stuff"));
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            );

            _classifiedAd.RequestToPublish(); 

            Assert.Equal(ClassifiedAd.ClassifiedAdState.PendingReview, _classifiedAd.State); 
        }

        [Fact]
        public void Cannot_publish_without_title() 
        {
            _classifiedAd.UpdateText(ClassifiedAdText.FromString("Please by my stuff")); 
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            ); 

            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish()); 
        }

        [Fact]
        public void Cannot_public_with_text() 
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad")); 
            _classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())); 

            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish()); 
        }

        [Fact]
        public void Cannot_publish_without_price() 
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad")); 
            _classifiedAd.UpdateText(ClassifiedAdText.FromString("HOLA")); 

            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish()); 
        }


    }
}