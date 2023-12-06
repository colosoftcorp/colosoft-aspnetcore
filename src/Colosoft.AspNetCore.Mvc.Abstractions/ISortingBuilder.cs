namespace Colosoft.AspNetCore.Mvc
{
    public interface ISortingBuilder
    {
        ISortingBuilder AddField(string name, bool isAscending);

        ISortingBuilder AddExpression(string expression);

        ISorting Build();
    }
}
