namespace Colosoft.AspNetCore.Mvc
{
    public class PagingFactory : IPagingFactory
    {
        public IPaging Create(int page, int pageSize) =>
            new Paging(page, pageSize);
    }
}
