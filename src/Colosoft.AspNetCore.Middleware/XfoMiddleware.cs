using Colosoft.AspNetCore.Middleware.HttpHeaders;
using Colosoft.AspNetCore.Middleware.HttpHeaders.Configuration;
using Colosoft.AspNetCore.Middleware.Options;
using Microsoft.AspNetCore.Http;

namespace Colosoft.AspNetCore.Middleware
{
    public class XfoMiddleware : MiddlewareBase
    {
        private readonly IXFrameOptionsConfiguration config;
        private readonly HeaderResult? headerResult;

        public XfoMiddleware(RequestDelegate next, XFrameOptions options)
            : base(next)
        {
            this.config = options;
            var headerGenerator = new HeaderGenerator();
            this.headerResult = headerGenerator.CreateXfoResult(this.config);
        }

        internal override void PreInvokeNext(HttpContext context)
        {
            context.GetColosoftContext().XFrameOptions = this.config;
            if (this.headerResult?.Action == HeaderResult.ResponseAction.Set)
            {
                context.Response.Headers[this.headerResult.Name] = this.headerResult.Value;
            }
        }
    }
}
