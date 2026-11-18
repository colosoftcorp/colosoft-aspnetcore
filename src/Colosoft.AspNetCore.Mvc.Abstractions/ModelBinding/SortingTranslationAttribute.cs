using System;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Sets the translation of the sorting of an action, on the action method or on its <see cref="ISorting"/>
    /// parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public class SortingTranslationAttribute : Attribute
    {
        public SortingTranslationAttribute(Type translatorType)
        {
            this.TranslatorType = translatorType ?? throw new ArgumentNullException(nameof(translatorType));
        }

        public Type TranslatorType { get; }
    }
}
