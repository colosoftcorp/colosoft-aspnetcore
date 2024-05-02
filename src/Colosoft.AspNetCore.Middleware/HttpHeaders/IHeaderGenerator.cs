using Colosoft.AspNetCore.Middleware.HttpHeaders.Configuration;

namespace Colosoft.AspNetCore.Middleware.HttpHeaders
{
    public interface IHeaderGenerator
    {
        HeaderResult? CreateXfoResult(
            IXFrameOptionsConfiguration xfoConfig,
            IXFrameOptionsConfiguration? oldXfoConfig = null);
    }
}
