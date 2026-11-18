using System;

namespace Colosoft.AspNetCore.Mvc
{
    public abstract class BaseSortingTranslation : ISortingTranslation
    {
        private readonly string sorting;
        private readonly Lazy<string> sortingTranslated;

        protected BaseSortingTranslation(string sorting)
        {
            this.sorting = sorting;
            this.sortingTranslated = new Lazy<string>(() => this.TranslateSorting());
        }

        protected abstract string DefaultSorting { get; }

        /// <summary>
        /// Translates the first field of the sorting and keeps its direction.
        /// </summary>
        /// <returns>The translated sorting.</returns>
        /// <exception cref="SortingFormatException">The field or the direction of the sorting is not valid.</exception>
        public string Translate()
        {
            return this.sortingTranslated.Value;
        }

        protected abstract string TranslateField(string field);

        private string AddDirection(string field, bool isAscending) =>
            string.Format(
                "{0} {1}",
                field,
                isAscending ? SortingSyntax.AscendingTerm : SortingSyntax.DescendingTerm);

        private string TranslateSorting()
        {
            if (string.IsNullOrWhiteSpace(this.sorting))
            {
                return this.DefaultSorting;
            }

            // Only the first field is translated, as before; the direction is asc or desc, never the client text.
            var item = this.sorting.Split(',')[0];

            if (string.IsNullOrWhiteSpace(item))
            {
                return this.DefaultSorting;
            }

            SortingSyntax.ParseItem(item, out var field, out var isAscending);

            var traducao = this.TranslateField(field)
                ?? this.DefaultSorting;

            return traducao != this.DefaultSorting
                ? this.AddDirection(traducao, isAscending)
                : traducao;
        }
    }
}
