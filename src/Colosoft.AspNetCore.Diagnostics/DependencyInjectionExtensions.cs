using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Colosoft.AspNetCore.Diagnostics
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddColosoftDiagnostics(
            this IServiceCollection services,
            Action<DiagnosticsOptions> setup)
        {
            services.TryAddSingleton<IExceptionMessageFactory, DefaultExceptionMessageFactory>();
            services.TryAddSingleton<IExceptionStatusCodeProvider, DefaultExceptionStatusCodeProvider>();
            services.AddOptions<DiagnosticsOptions>().Configure(setup);
            return services;
        }
    }
}
