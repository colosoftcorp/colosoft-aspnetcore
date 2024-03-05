using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            this.logger.LogInformation("Test");
            throw new InvalidOperationException("Test");
        }
    }
}
