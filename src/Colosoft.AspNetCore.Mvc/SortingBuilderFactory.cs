namespace Colosoft.AspNetCore.Mvc
{
    internal class SortingBuilderFactory : ISortingBuilderFactory
    {
        public ISortingBuilder Create() => new SortingBuilder();
    }
}
