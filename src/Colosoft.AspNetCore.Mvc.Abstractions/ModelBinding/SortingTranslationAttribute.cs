using System;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SortingTranslationAttribute : Attribute
    {
        public SortingTranslationAttribute(Type translatorType)
        {
            this.TranslatorType = translatorType ?? throw new ArgumentNullException(nameof(translatorType));
        }

        public Type TranslatorType { get; }
    }
}
