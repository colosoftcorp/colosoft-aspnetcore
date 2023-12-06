using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Colosoft.AspNetCore.Mvc
{
    public static class MvcDependencyInjectionExtensions
    {
        public static void ConfigureColosoftMvc(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new ModelBinding.PagingModelBinderProvider());
            options.ModelBinderProviders.Insert(1, new ModelBinding.SortingModelBinderProvider());
        }

        public static IServiceCollection AddColosoftMvc(this IServiceCollection services)
        {
            services
                .AddSingleton<IPagingFactory, PagingFactory>()
                .AddSingleton<ISortingBuilderFactory, SortingBuilderFactory>()
                .AddSingleton<ISortingTranslatorFactory, SortingTranslatorFactory>();

            return services;
        }
    }
}
