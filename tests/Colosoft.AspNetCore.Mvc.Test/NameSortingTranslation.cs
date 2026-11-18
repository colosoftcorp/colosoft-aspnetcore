namespace Colosoft.AspNetCore.Mvc.Test
{
    public class NameSortingTranslation : BaseSortingTranslation
    {
        public NameSortingTranslation(string sorting)
            : base(sorting)
        {
        }

        protected override string DefaultSorting => "c.Id desc";

        protected override string TranslateField(string field) =>
            string.Equals(field, "name", StringComparison.OrdinalIgnoreCase) ? "c.Name" : null!;
    }
}
