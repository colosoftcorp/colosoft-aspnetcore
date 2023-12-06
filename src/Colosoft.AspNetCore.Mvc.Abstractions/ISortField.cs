namespace Colosoft.AspNetCore.Mvc
{
    public interface ISortField
    {
        string Name { get; }

        bool IsAscending { get; }
    }
}