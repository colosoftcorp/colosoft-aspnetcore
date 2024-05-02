using Microsoft.AspNetCore.Http;

namespace Colosoft.AspNetCore.Middleware
{
    public static class HttpContextExtensions
    {
        public static ColosoftMiddlewareContext GetColosoftContext(this HttpContext context)
        {
            if (!context.Items.ContainsKey(ColosoftMiddlewareContext.ContextKey))
            {
                context.Items[ColosoftMiddlewareContext.ContextKey] = new ColosoftMiddlewareContext();
            }

            return (ColosoftMiddlewareContext)context.Items[ColosoftMiddlewareContext.ContextKey] !;
        }
    }
}
