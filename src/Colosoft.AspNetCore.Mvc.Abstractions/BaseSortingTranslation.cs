using System;

namespace Colosoft.AspNetCore.Mvc
{
    public abstract class BaseSortingTranslation : ISortingTranslation
    {
        private readonly string[] sortingData;
        private readonly Lazy<string> sortingTranslated;

        protected BaseSortingTranslation(string sorting)
        {
            this.sortingData = string.IsNullOrWhiteSpace(sorting)
                ? new string[0]
                : sorting.Split(' ');

            this.sortingTranslated = new Lazy<string>(() => this.TranslateSorting());
        }

        protected abstract string DefaultSorting { get; }

        public string Translate()
        {
            return this.sortingTranslated.Value;
        }

        protected abstract string TranslateField(string field);

        private string TranslateSorting()
        {
            if (this.sortingData.Length == 0)
            {
                return this.DefaultSorting;
            }

            var traducao = this.TranslateField(this.sortingData[0])
                ?? this.DefaultSorting;

            return traducao != this.DefaultSorting
                ? this.AddDirection(traducao)
                : traducao;
        }

        private string AddDirection(string field)
        {
            string direcao = this.sortingData.Length > 1
                ? string.Format(" {0}", this.sortingData[1])
                : string.Empty;

            return field + direcao;
        }
    }
}
