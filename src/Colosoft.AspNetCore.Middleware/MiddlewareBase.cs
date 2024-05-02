using Microsoft.AspNetCore.Http;

namespace Colosoft.AspNetCore.Middleware
{
    public class MiddlewareBase
    {
        private readonly RequestDelegate next;

        public MiddlewareBase(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            this.PreInvokeNext(context);

            if (this.next != null)
            {
                await this.next(context);
            }

            this.PostInvokeNext(context);
        }

        internal virtual void PreInvokeNext(HttpContext context)
        {
        }

        internal virtual void PostInvokeNext(HttpContext context)
        {
        }
    }
}
