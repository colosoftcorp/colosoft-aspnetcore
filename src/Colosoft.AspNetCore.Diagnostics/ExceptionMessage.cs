using System.Text.Json.Serialization;

namespace Colosoft.AspNetCore.Diagnostics
{
    public class ExceptionMessage
    {
        private readonly Exception exception;
        private readonly DiagnosticsOptions options;

        public ExceptionMessage(
            Exception exception,
            DiagnosticsOptions options)
        {
            this.exception = exception;
            this.options = options;
        }

        public virtual string Message => this.exception.Message;

        public virtual string Type => this.exception.GetType().Name;

        public virtual IDictionary<string, string>? Metadata { get; set; }

        public virtual string? StackTrace =>
            this.options.ShowStackTrace && (this.options.ShowStackTraceFilter == null || this.options.ShowStackTraceFilter.Invoke(this.exception))
            ? this.exception.StackTrace
            : null;

        public virtual ExceptionMessage? Inner =>
            this.options.ShowInnerException &&
            this.exception.InnerException != null &&
            (this.options.ShowInnerExceptionFilter == null || this.options.ShowInnerExceptionFilter.Invoke(this.exception))
            ? new ExceptionMessage(this.exception.InnerException, this.options)
            : null;
    }
}
