using Colosoft.AspNetCore.Middleware.HttpHeaders.Configuration;

namespace Colosoft.AspNetCore.Middleware.HttpHeaders
{
    public class HeaderGenerator : IHeaderGenerator
    {
        public HeaderResult? CreateXfoResult(
            IXFrameOptionsConfiguration xfoConfig,
            IXFrameOptionsConfiguration? oldXfoConfig = null)
        {
            if (oldXfoConfig != null && oldXfoConfig.Policy != XfoPolicy.Disabled &&
                xfoConfig.Policy == XfoPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XFrameOptionsHeader);
            }

            switch (xfoConfig.Policy)
            {
                case XfoPolicy.Disabled:
                    return null;

                case XfoPolicy.Deny:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "Deny");

                case XfoPolicy.SameOrigin:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "SameOrigin");

                default:
                    throw new NotImplementedException(
                        """Apparently someone forgot to implement support for: """ + xfoConfig.Policy);
            }
        }
    }
}
