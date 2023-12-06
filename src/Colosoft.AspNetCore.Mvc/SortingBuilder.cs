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

            var campos = expression.Split(',').Select(f => f.Trim());

            foreach (var campo in campos)
            {
                if (string.IsNullOrEmpty(campo))
                {
                    continue;
                }

                var partes = campo.Split(' ');
                if (partes.Length > 1)
                {
                    this.AddField(partes[0], !StringComparer.InvariantCultureIgnoreCase.Equals(partes[1], DescendingTerm));
                }
                else
                {
                    this.AddField(partes[0], true);
                }
            }

            return this;
        }

        public ISorting Build() =>
            new Sorting(this.fields.ToArray());
    }
}
