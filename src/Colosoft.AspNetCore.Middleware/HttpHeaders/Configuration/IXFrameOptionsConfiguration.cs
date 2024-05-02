namespace Colosoft.AspNetCore.Middleware.HttpHeaders.Configuration
{
    public interface IXFrameOptionsConfiguration
    {
        XfoPolicy Policy { get; set; }
    }
}
