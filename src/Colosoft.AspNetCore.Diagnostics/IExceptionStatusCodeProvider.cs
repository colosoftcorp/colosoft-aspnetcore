namespace Colosoft.AspNetCore.Diagnostics
{
    public interface IExceptionStatusCodeProvider
    {
        int? GetStatusCode(Exception exception);
    }
}
