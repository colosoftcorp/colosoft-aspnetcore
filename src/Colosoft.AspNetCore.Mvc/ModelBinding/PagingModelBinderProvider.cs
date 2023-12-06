using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    internal class PagingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(IPaging))
            {
                return new PagingModelBinder();
            }

            return null;
        }
    }
}
