using Colosoft.AspNetCore.Middleware;
using Colosoft.AspNetCore.Middleware.Options;

namespace Microsoft.AspNetCore.Builder
{
    public static class ColosoftMiddlewareApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseXfo(this IApplicationBuilder app, Action<IFluentXFrameOptions> configurer)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configurer == null)
            {
                throw new ArgumentNullException(nameof(configurer));
            }

            var options = new XFrameOptions();
            configurer(options);
            return app.UseMiddleware<XfoMiddleware>(options);
        }
    }
}
