namespace Colosoft.AspNetCore.Mvc
{
    internal class SortingTranslatorFactory : ISortingTranslatorFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SortingTranslatorFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ISortingTranslator? Create(Type translateType)
        {
            if (typeof(ISortingTranslation).IsAssignableFrom(translateType))
            {
                return new SortingTranslatorWrapper(translateType);
            }
            else
            {
                return this.serviceProvider.GetService(translateType) as ISortingTranslator;
            }
        }
    }
}
