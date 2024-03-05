using Microsoft.AspNetCore.Mvc.Filters;

namespace Colosoft.AspNetCore.Diagnostics
{
    public interface IExceptionMessageFactory
    {
        ExceptionMessage Create(ExceptionContext context, DiagnosticsOptions options);
    }
}
