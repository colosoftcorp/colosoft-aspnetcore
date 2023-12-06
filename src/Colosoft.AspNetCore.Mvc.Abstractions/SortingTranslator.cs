namespace Colosoft.AspNetCore.Mvc
{
    public abstract class SortingTranslator : ISortingTranslator
    {
        public virtual void BuildDefaultSorting(ISortingBuilder builder)
        {
        }

        public bool Translate(ISorting sorting, ISortingBuilder builder)
        {
            var hasFields = false;
            foreach (var field in sorting.Fields)
            {
                var name = this.TranslateField(field.Name);
                builder.AddField(name, field.IsAscending);
                hasFields = true;
            }

            return hasFields;
        }

        protected abstract string TranslateField(string field);
    }
}
