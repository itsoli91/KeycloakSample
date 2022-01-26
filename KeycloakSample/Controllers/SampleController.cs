using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeycloakSample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;

        public SampleController(ILogger<SampleController> logger)
        {
            _logger = logger;
        }

        [Authorize(Policy = "User"), HttpGet, Route("UserResource")]
        public string GetUserRole()
        {
            _logger.LogInformation("I'm working fine with User policy and role");
            return "I'm working with User policy and role";
        }

        [Authorize(Policy = "Administrator"), HttpGet, Route("AdministratorResource")]
        public string GetAdministratorRole()
        {
            _logger.LogInformation("I'm working with administrator policy and role");

            return "I'm working with administrator policy and role";
        }
    }
}