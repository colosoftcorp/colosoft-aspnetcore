namespace Colosoft.AspNetCore.Mvc
{
    internal class Paging : IPaging
    {
        public Paging(int pagina, int numeroRegistros)
        {
            this.Page = pagina;
            this.PageSize = numeroRegistros;
        }

        public int Page { get; }

        public int PageSize { get; }
    }
}
