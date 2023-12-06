using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    internal class SortingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(ISorting))
            {
                return new SortingModelBinder();
            }

            return null;
        }
    }
}
