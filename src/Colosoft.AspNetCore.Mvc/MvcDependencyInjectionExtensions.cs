using Colosoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;

namespace Colosoft.AspNetCore.Mvc
{
    public static class MvcDependencyInjectionExtensions
    {
        private static int FirstIndexOfOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var result = 0;

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return result;
                }

                result++;
            }

            return -1;
        }

        private static int FindModelBinderProviderInsertLocation(this IList<IModelBinderProvider> modelBinderProviders)
        {
            var index = modelBinderProviders.FirstIndexOfOrDefault(i => i is FloatingPointTypeModelBinderProvider);
            return index < 0 ? index : index + 1;
        }

        public static void InsertCommaSeparatedEnumerableModelBinderProvider(this IList<IModelBinderProvider> modelBinderProviders)
        {
            if (modelBinderProviders == null)
            {
                throw new ArgumentNullException(nameof(modelBinderProviders));
            }

            var providerToInsert = new CommaSeparatedEnumerableModelBinderProvider();

            var index = modelBinderProviders.FindModelBinderProviderInsertLocation();

            if (index != -1)
            {
                modelBinderProviders.Insert(index, providerToInsert);
            }
            else
            {
                modelBinderProviders.Add(providerToInsert);
            }
        }

        public static MvcOptions AddCommaSeparatedEnumerableModelBinderProvider(this MvcOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.ModelBinderProviders.InsertCommaSeparatedEnumerableModelBinderProvider();
            return options;
        }

        public static void ConfigureColosoftMvc(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new PagingModelBinderProvider());
            options.ModelBinderProviders.Insert(1, new SortingModelBinderProvider());
            options.AddCommaSeparatedEnumerableModelBinderProvider();
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
