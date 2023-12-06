namespace Colosoft.AspNetCore.Mvc
{
    public interface IPagingFactory
    {
        IPaging Create(int page, int pageSize);
    }
}
