using Colosoft.AspNetCore.Middleware.HttpHeaders;
using Colosoft.AspNetCore.Middleware.HttpHeaders.Configuration;

namespace Colosoft.AspNetCore.Middleware.Options
{
    public class XFrameOptions : IXFrameOptionsConfiguration, IFluentXFrameOptions
    {
        public XFrameOptions()
        {
            this.Policy = XfoPolicy.Disabled;
        }

        public XfoPolicy Policy { get; set; }

        public void Deny()
        {
            this.Policy = XfoPolicy.Deny;
        }

        public void SameOrigin()
        {
            this.Policy = XfoPolicy.SameOrigin;
        }
    }
}
