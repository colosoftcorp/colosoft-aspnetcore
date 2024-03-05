namespace Colosoft.AspNetCore.Mvc
{
    internal class SortingBuilder : ISortingBuilder
    {
        private const string DescendingTerm = "desc";

        private readonly IList<ISortField> fields = new List<ISortField>();

        public ISortingBuilder AddField(string name, bool isAscending)
        {
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

                var parts = expressionField.Split(':');
                if (parts.Length > 1)
                {
                    this.AddField(parts[0], !StringComparer.InvariantCultureIgnoreCase.Equals(parts[1], DescendingTerm));
                }
                else
                {
                    this.AddField(parts[0], true);
                }
            }

            return this;
        }

        public ISorting Build() =>
            new Sorting(this.fields.ToArray());
    }
}
