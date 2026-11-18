namespace Colosoft.AspNetCore.Mvc
{
    internal class SortingBuilder : ISortingBuilder
    {
        private readonly IList<ISortField> fields = new List<ISortField>();

        public ISortingBuilder AddField(string name, bool isAscending)
        {
            SortingSyntax.EnsureValidFieldName(name);
            this.fields.Add(new SortField(name, isAscending));
            return this;
        }

        public ISortingBuilder AddExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return this;
            }

            var expressionFields = expression.Split(',').Select(f => f.Trim());

            foreach (var expressionField in expressionFields)
            {
                if (string.IsNullOrEmpty(expressionField))
                {
                    continue;
                }

                SortingSyntax.ParseItem(expressionField, out var name, out var isAscending);
                this.AddField(name, isAscending);
            }

            return this;
        }

        public ISorting Build() =>
            new Sorting(this.fields.ToArray());
    }
}
