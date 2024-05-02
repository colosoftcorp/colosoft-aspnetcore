using Colosoft.AspNetCore.Middleware.HttpHeaders.Configuration;

namespace Colosoft.AspNetCore.Middleware
{
    public class ColosoftMiddlewareContext
    {
        public static readonly string ContextKey = "colosoftmiddleware.Context";
        public static readonly string ContextKeyOverrides = "colosoftmiddleware.Context.Overrides";

        public IXFrameOptionsConfiguration? XFrameOptions { get; set; }
    }
}
