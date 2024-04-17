using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly ILogger<BaseApiController> _logger;

        public BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger;
        }

        // Common method for logging
        protected void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        // Common method for logging errors
        protected void LogError(string message)
        {
            _logger.LogError(message);
        }

        // Common method for handling bad requests with custom error message
        protected ActionResult BadRequest(string errorMessage)
        {
            LogError(errorMessage);
            return base.BadRequest(new { error = errorMessage });
        }
    }
}
