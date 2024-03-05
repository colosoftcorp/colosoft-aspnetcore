using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;

namespace Colosoft.AspNetCore.Mvc.ModelBinding
{
    public class CommaSeparatedEnumerableModelBinder : IModelBinder
    {
        private static readonly Type[] SupportedElementTypes =
        {
            typeof(int),
            typeof(long),
            typeof(short),
            typeof(byte),
            typeof(uint),
            typeof(ulong),
            typeof(ushort),
            typeof(Guid),
            typeof(string),
        };

        internal static bool IsSupportedModelType(Type modelType)
        {
            if (modelType.IsArray && modelType.GetArrayRank() == 1 && modelType.HasElementType)
            {
                var elementType = modelType.GetElementType() !;
                return SupportedElementTypes.Contains(elementType) || elementType.IsEnum;
            }
            else if (modelType.IsGenericType &&
                modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var argumentType = modelType.GetGenericArguments()[0];

                return SupportedElementTypes.Contains(argumentType) || argumentType.IsEnum;
            }

            return false;
        }

        private static Type GetElementType(Type modelType)
        {
            if (modelType.IsArray && modelType.GetArrayRank() == 1 && modelType.HasElementType)
            {
                return modelType.GetElementType() !;
            }
            else if (modelType.IsGenericType &&
                modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return modelType.GetGenericArguments()[0];
            }

            throw new InvalidOperationException("Not found elementType");
        }

        private static Array CopyAndConvertArray(IList<string> sourceArray, Type elementType)
        {
            var targetArray = Array.CreateInstance(elementType, sourceArray.Count);
            if (sourceArray.Count > 0)
            {
                if (elementType.IsEnum)
                {
                    for (var i = 0; i < sourceArray.Count; i++)
                    {
                        if (Enum.TryParse(elementType, sourceArray[i], true, out var value1))
                        {
                            targetArray.SetValue(value1, i);
                        }
                        else if (int.TryParse(sourceArray[i], out var value2))
                        {
                            targetArray.SetValue(value2, i);
                        }
                    }
                }
                else
                {
                    var converter = TypeDescriptor.GetConverter(elementType);
                    for (var i = 0; i < sourceArray.Count; i++)
                    {
                        targetArray.SetValue(converter.ConvertFromString(sourceArray[i]), i);
                    }
                }
            }

            return targetArray;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!IsSupportedModelType(bindingContext.ModelType))
            {
                return Task.CompletedTask;
            }

            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (providerValue == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var parts = providerValue.Values.SelectMany(s =>
                !string.IsNullOrEmpty(s) ? s.Split(',', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>()).ToList();

            if (!parts.Any() || parts.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                return Task.CompletedTask;
            }

            var elementType = GetElementType(bindingContext.ModelType);
            if (elementType == null)
            {
                return Task.CompletedTask;
            }

            var realResult = CopyAndConvertArray(parts, elementType);

            bindingContext.Result = ModelBindingResult.Success(realResult);

            return Task.CompletedTask;
        }
    }
}
