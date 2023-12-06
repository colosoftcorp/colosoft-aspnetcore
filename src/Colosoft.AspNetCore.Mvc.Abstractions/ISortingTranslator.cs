namespace Colosoft.AspNetCore.Mvc
{
    public interface ISortingTranslator
    {
        void BuildDefaultSorting(ISortingBuilder builder);

        bool Translate(ISorting sorting, ISortingBuilder builder);
    }
}
