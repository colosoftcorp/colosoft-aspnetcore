namespace Colosoft.AspNetCore.Mvc
{
    public interface IPaging
    {
        int Page { get; }

        int PageSize { get; }
    }
}
