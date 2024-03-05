using Microsoft.AspNetCore.Mvc.Filters;

namespace Colosoft.AspNetCore.Diagnostics
{
    public class DefaultExceptionMessageFactory : IExceptionMessageFactory
    {
        public virtual ExceptionMessage Create(ExceptionContext context, DiagnosticsOptions options)
        {
            return new ExceptionMessage(context.Exception, options);
        }
    }
}
