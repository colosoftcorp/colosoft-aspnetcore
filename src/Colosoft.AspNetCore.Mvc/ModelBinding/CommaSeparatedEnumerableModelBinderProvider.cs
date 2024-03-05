using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    public class CommaSeparatedEnumerableModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return CommaSeparatedEnumerableModelBinder.IsSupportedModelType(context.Metadata.ModelType)
                ? new CommaSeparatedEnumerableModelBinder()
                : null;
        }
    }
}
