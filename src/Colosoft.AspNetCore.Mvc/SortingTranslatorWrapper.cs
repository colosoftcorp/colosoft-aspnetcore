using System;

namespace Colosoft.AspNetCore.Mvc
{
    internal class SortingTranslatorWrapper : ISortingTranslator
    {
        private readonly Type sortingTranslationType;

        public SortingTranslatorWrapper(Type sortingTranslationType)
        {
            this.sortingTranslationType = sortingTranslationType;
        }

        public void BuildDefaultSorting(ISortingBuilder builder)
        {
        }

        public bool Translate(ISorting sorting, ISortingBuilder builder)
        {
            var sortingTranslation = this.CreateSortingTranslation(sorting?.ToString());
            var expression = sortingTranslation?.Translate();

            if (!string.IsNullOrEmpty(expression))
            {
                builder.AddExpression(expression);
                return true;
            }

            return false;
        }

        private ISortingTranslation? CreateSortingTranslation(string? expression) =>
            Activator.CreateInstance(this.sortingTranslationType, expression) as ISortingTranslation;
    }
}
