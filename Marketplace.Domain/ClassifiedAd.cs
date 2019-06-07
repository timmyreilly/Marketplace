using System;
using System.Collections.Generic;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity
    {
        public ClassifiedAdId Id { get; private set; }
        public UserId OwnerId { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }

        // public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        // {
        //     Id = id;
        //     OwnerId = ownerId;
        //     State = ClassifiedAdState.Inactive;
        //     EnsureValidState();

        //     Raise(new Events.ClassifiedAdCreated
        //     {
        //         Id = id,
        //         OwnerId = ownerId
        //     });
        // }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId) =>
            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });

        public void SetTitle(ClassifiedAdTitle title) =>
            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });

        // making the implicit explicit! Refactoring the argument exceptions into value objects. 


        // public void SetTitle(ClassifiedAdTitle title)
        // {
        //     Title = title;
        //     EnsureValidState();

        //     Raise(new Events.ClassifiedAdTitleChanged
        //     {
        //         Id = Id,
        //         Title = title
        //     });
        // }

        public void SetTitleOld(ClassifiedAdTitle title) =>
            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        public void SetTitleOlder(ClassifiedAdTitle title) => Title = title;

        public void UpdateText(ClassifiedAdText text) =>
        Apply(new Events.ClassifiedAdTextUpdated
        {
            Id = Id,
            AdText = text // This is implementation details! This is where bugs show up. Or that's what I'm assuming. 
        });

        public void UpdateTextOld(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();
        }
        public void UpdateTextOlder(ClassifiedAdText text) => Text = text;

        public void UpdatePrice(Price price) =>
            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                Price = price.Amount,
                CurrencyCode = price.Currency.CurrencyCode
            });
        public void UpdatePriceOlder(Price price) => Price = price;
        public void UpdatePriceOld(Price price)
        {
            Price = price;
            EnsureValidState();
        }

        public void RequestToPublishOld()
        {
            if (Title == null)
            {
                throw new InvalidEntityStateException(this, "title cannot be empty");
            }

            if (Text == null)
            {
                throw new InvalidEntityStateException(this, "text cannot be empty");
            }
            if (Price?.Amount == 0)
            {
                throw new InvalidEntityStateException(this, "price cannot be zero");
            }

            State = ClassifiedAdState.PendingReview;

            // Remove all the null checks above and do it this way? 
            EnsureValidState();
        }

        public void RequestToPublish() =>
            Apply(new Events.ClassifiedAdSentForReview { Id = Id });

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    // This is Jaw dropping. 
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            bool valid = Id != null && OwnerId != null;

            switch (State)
            {
                case ClassifiedAdState.PendingReview:
                    valid = valid
                        && Title != null
                        && Text != null
                        && Price?.Amount > 0;
                    break;
                case ClassifiedAdState.Active:
                    valid = valid
                        && Title != null
                        && Text != null
                        && Price?.Amount > 0
                        && ApprovedBy != null;
                    break;
            }

            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
            }


        }

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }
    }
}