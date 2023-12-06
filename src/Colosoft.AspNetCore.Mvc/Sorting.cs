using System.Collections.Generic;

namespace Colosoft.AspNetCore.Mvc
{
    internal class Sorting : ISorting
    {
        public Sorting(IEnumerable<ISortField> campos)
        {
            this.Fields = campos;
        }

        public IEnumerable<ISortField> Fields { get; }

        public override string ToString() => string.Join(",", this.Fields);
    }
}
