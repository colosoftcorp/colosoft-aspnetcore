using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    internal class SortingModelBinder : IModelBinder
    {
        private const string SortingFieldName = "sort";

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(ISorting))
            {
                return Task.CompletedTask;
            }

            var actionContext = bindingContext.ActionContext;

            var sortingField = bindingContext.ValueProvider.GetValue(SortingFieldName);
            var expression = sortingField.FirstValue;
            var translatorType = ((SortingTranslationAttribute?)actionContext.ActionDescriptor.Parameters
                .FirstOrDefault(f => f.Name == bindingContext.ModelName)
                ?.ParameterType
                ?.GetCustomAttributes(typeof(SortingTranslationAttribute), true)
                ?.FirstOrDefault())
                ?.TranslatorType;

            var sortingBuilderFactory = (ISortingBuilderFactory)bindingContext.HttpContext.RequestServices.GetService(typeof(ISortingBuilderFactory)) !;
            var sortingTranslatorFactory = (ISortingTranslatorFactory)bindingContext.HttpContext.RequestServices.GetService(typeof(ISortingTranslatorFactory)) !;

            var sorting = this.GetSorting(
                expression,
                translatorType,
                sortingBuilderFactory,
                sortingTranslatorFactory);

            bindingContext.Result = ModelBindingResult.Success(sorting);

            return Task.CompletedTask;
        }

        private ISorting GetSorting(
            string? expression,
            Type? translatorType,
            ISortingBuilderFactory sortingBuilderFactory,
            ISortingTranslatorFactory sortingTranslatorFactory)
        {
            var builder = sortingBuilderFactory.Create();
            builder.AddExpression(expression);

            var sorting = builder.Build();

            if (translatorType != null)
            {
                sorting = this.Translate(sorting, translatorType, sortingBuilderFactory, sortingTranslatorFactory);
            }

            return sorting;
        }

        private ISorting Translate(
            ISorting sorting,
            Type translatorType,
            ISortingBuilderFactory sortingBuilderFactory,
            ISortingTranslatorFactory sortingTranslatorFactory)
        {
            var translator = sortingTranslatorFactory.Create(translatorType);
            if (translator != null)
            {
                var builder = sortingBuilderFactory.Create();
                translator.Translate(sorting, builder);
                return builder.Build();
            }

            return sorting;
        }
    }
}
