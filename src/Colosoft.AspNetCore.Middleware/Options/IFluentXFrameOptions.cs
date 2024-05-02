namespace Colosoft.AspNetCore.Middleware.Options
{
    public interface IFluentXFrameOptions : IFluentInterface
    {
        void Deny();

        void SameOrigin();
    }
}
