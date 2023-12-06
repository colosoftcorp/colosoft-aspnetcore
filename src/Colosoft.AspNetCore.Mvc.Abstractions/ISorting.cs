using System.Collections.Generic;

namespace Colosoft.AspNetCore.Mvc
{
    public interface ISorting
    {
        IEnumerable<ISortField> Fields { get; }
    }
}
