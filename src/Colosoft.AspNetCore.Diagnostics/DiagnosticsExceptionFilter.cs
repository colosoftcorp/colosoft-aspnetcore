using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Colosoft.AspNetCore.Diagnostics
{
    public class DiagnosticsExceptionFilter : IExceptionFilter
    {
        private readonly IOptions<DiagnosticsOptions> options;
        private readonly IExceptionMessageFactory exceptionMessageFactory;
        private readonly IExceptionStatusCodeProvider statusCodeProvider;

        public DiagnosticsExceptionFilter(
            IOptions<DiagnosticsOptions> options,
            IExceptionMessageFactory exceptionMessageFactory,
            IExceptionStatusCodeProvider statusCodeProvider)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.exceptionMessageFactory = exceptionMessageFactory ?? throw new ArgumentNullException(nameof(exceptionMessageFactory));
            this.statusCodeProvider = statusCodeProvider ?? throw new ArgumentNullException(nameof(statusCodeProvider));
        }

        public void OnException(ExceptionContext context)
        {
            if (this.options.Value.ExceptionFilter == null ||
                this.options.Value.ExceptionFilter(context.Exception))
            {
                var message = this.exceptionMessageFactory.Create(context, this.options.Value);

                var statusCode = this.statusCodeProvider.GetStatusCode(context.Exception);

                context.Result = new ObjectResult(message)
                {
                    StatusCode = statusCode ?? this.options.Value.DefaultStatusCode,
                };

                context.ExceptionHandled = true;
            }
            else
            {
                context.ExceptionHandled = false;
            }
        }
    }
}
