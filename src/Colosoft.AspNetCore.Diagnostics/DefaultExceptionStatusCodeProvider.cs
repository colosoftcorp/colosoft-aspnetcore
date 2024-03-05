namespace Colosoft.AspNetCore.Diagnostics
{
    public class DefaultExceptionStatusCodeProvider : IExceptionStatusCodeProvider
    {
        public virtual int? GetStatusCode(Exception exception)
        {
            return null;
        }
    }
}
