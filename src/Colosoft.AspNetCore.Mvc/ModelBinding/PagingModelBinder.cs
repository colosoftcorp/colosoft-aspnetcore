using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    internal class PagingModelBinder : IModelBinder
    {
        private const string PageFieldName = "page";
        private const string PageSizeFieldName = "pageSize";

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(IPaging))
            {
                return Task.CompletedTask;
            }

            var pageField = bindingContext.ValueProvider.GetValue(PageFieldName);
            var pageSizeField = bindingContext.ValueProvider.GetValue(PageSizeFieldName);

            if (!int.TryParse(pageField.FirstValue ?? string.Empty, out var page))
            {
                page = 1;
            }

            if (!int.TryParse(pageSizeField.FirstValue ?? string.Empty, out var pageSize))
            {
                pageSize = int.MaxValue;
            }

            var pagingFactory = (IPagingFactory)bindingContext.HttpContext.RequestServices.GetService(typeof(IPagingFactory)) !;
            var paging = pagingFactory.Create(page, pageSize);
            bindingContext.Result = ModelBindingResult.Success(paging);

            return Task.CompletedTask;
        }
    }
}
