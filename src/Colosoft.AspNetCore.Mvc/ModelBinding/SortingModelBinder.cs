using Microsoft.AspNetCore.Mvc.Controllers;
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

            var sortingField = bindingContext.ValueProvider.GetValue(SortingFieldName);
            var expression = sortingField.FirstValue;
            var translatorType = this.GetSortingTranslationAttribute(bindingContext)?.TranslatorType;

            var sortingBuilderFactory = (ISortingBuilderFactory)bindingContext.HttpContext.RequestServices.GetService(typeof(ISortingBuilderFactory)) !;
            var sortingTranslatorFactory = (ISortingTranslatorFactory)bindingContext.HttpContext.RequestServices.GetService(typeof(ISortingTranslatorFactory)) !;

            ISorting sorting;

            try
            {
                sorting = this.GetSorting(
                    expression,
                    translatorType,
                    sortingBuilderFactory,
                    sortingTranslatorFactory);
            }
            catch (SortingFormatException exception)
            {
                // The text never reaches the query: the request fails the model validation (400 with [ApiController]).
                bindingContext.ModelState.TryAddModelError(SortingFieldName, exception.Message);
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(sorting);

            return Task.CompletedTask;
        }

        // The attribute is read from the ISorting parameter and then from the action method; it was read from the
        // parameter type before, where AttributeUsage(Method) never lets it be, so the translation never ran.
        private SortingTranslationAttribute? GetSortingTranslationAttribute(ModelBindingContext bindingContext)
        {
            var actionDescriptor = bindingContext.ActionContext.ActionDescriptor;

            var parameter = actionDescriptor?.Parameters
                .FirstOrDefault(f => f.Name == bindingContext.ModelMetadata?.ParameterName || f.Name == bindingContext.FieldName)
                ?? actionDescriptor?.Parameters.FirstOrDefault(f => f.ParameterType == typeof(ISorting));

            var attribute = (parameter as ControllerParameterDescriptor)?.ParameterInfo
                ?.GetCustomAttributes(typeof(SortingTranslationAttribute), true)
                .OfType<SortingTranslationAttribute>()
                .FirstOrDefault();

            if (attribute == null && actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                attribute = controllerActionDescriptor.MethodInfo
                    ?.GetCustomAttributes(typeof(SortingTranslationAttribute), true)
                    .OfType<SortingTranslationAttribute>()
                    .FirstOrDefault();
            }

            return attribute ?? actionDescriptor?.EndpointMetadata?
                .OfType<SortingTranslationAttribute>()
                .LastOrDefault();
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
