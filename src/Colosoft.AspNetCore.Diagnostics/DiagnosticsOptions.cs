using Microsoft.AspNetCore.Http;

namespace Colosoft.AspNetCore.Diagnostics
{
    public class DiagnosticsOptions
    {
        public bool ShowStackTrace { get; set; }

        public bool ShowInnerException { get; set; }

        public Func<Exception, bool>? ShowStackTraceFilter { get; set; }

        public Func<Exception, bool>? ShowInnerExceptionFilter { get; set; }

        public Func<Exception, bool>? ExceptionFilter { get; set; }

        public int DefaultStatusCode { get; set; } = StatusCodes.Status500InternalServerError;
    }
}
