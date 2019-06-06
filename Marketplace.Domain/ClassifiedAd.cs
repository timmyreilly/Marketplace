using System;
using System.Collections.Generic;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity
    {
        public ClassifiedAdId Id { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();

            Raise(new Events.ClassifiedAdCreated
            {
                Id = id, 
                OwnerId = ownerId
            }); 
        }

        // making the implicit explicit! Refactoring the argument exceptions into value objects. 

        public void SetTitleOld(ClassifiedAdTitle title) => Title = title;

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();

            Raise(new Events.ClassifiedAdTitleChanged
            {
                Id = Id, 
                Title = title 
            }); 
        }
        public void UpdateTextOld(ClassifiedAdText text) => Text = text;

        public void UpdateText(ClassifiedAdText text)
        {
            Text = text; 
            EnsureValidState(); 
        }
        public void UpdatePriceOld(Price price) => Price1 = price;

        public void UpdatePrice(Price price) 
        {
            Price1 = price; 
            EnsureValidState(); 
        }

        public void RequestToPublish()
        {
            if (Title == null)
            {
                throw new InvalidEntityStateException(this, "title cannot be empty");
            }

            if (Text == null)
            {
                throw new InvalidEntityStateException(this, "text cannot be empty");
            }
            if (Price1?.Amount == 0)
            {
                throw new InvalidEntityStateException(this, "price cannot be zero");
            }

            State = ClassifiedAdState.PendingReview;

            // Remove all the null checks above and do it this way? 
            EnsureValidState();
        }

        private void EnsureValidState()
        {
            bool valid = Id != null && OwnerId != null;

            switch (State)
            {
                case ClassifiedAdState.PendingReview:
                    valid = valid
                        && Title != null
                        && Text != null
                        && Price1?.Amount > 0;
                    break;
                case ClassifiedAdState.Active:
                    valid = valid
                        && Title != null
                        && Text != null
                        && Price1?.Amount > 0
                        && ApprovedBy != null;
                    break;
            }

            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
            }


        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
        }

        public UserId OwnerId { get; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price1 { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }
    }
}