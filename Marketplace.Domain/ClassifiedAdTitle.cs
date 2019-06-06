using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value
    {
        public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title); 

        private readonly string _value; 

        private ClassifiedAdTitle(string value)
        {
            if (value.Length > 100) 
            {
                throw new ArgumentOutOfRangeException("Title cannot be longer than 100 characters", nameof(value)); 
            }

            _value = value; 
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new System.NotImplementedException();
        }

        public static ClassifiedAdTitle FromHtml(string htmlTitle) 
        {
            var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "**")
                .Replace("</b>", "**"); 

            return new ClassifiedAdTitle(Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty)); 
        }

        public static implicit operator string(ClassifiedAdTitle self) => self._value; 
    }
}