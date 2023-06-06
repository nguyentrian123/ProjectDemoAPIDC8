using Contracts.Logger;
using Contracts.Repository;
using Microsoft.AspNetCore.Mvc;

namespace TestWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

       
        private ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public WeatherForecastController(ILoggerManager logger, IRepositoryManager repositoryManager )
        {
            _logger = logger;
            _repository = repositoryManager;
        }

        [HttpGet("/getlog")]
        public IEnumerable<string> GetLog()
        {
            _logger.LogInfo("Here is info message from our values controller.");
            _logger.LogDebug("Here is debug message from our values controller.");
            _logger.LogWarn("Here is warn message from our values controller.");
            _logger.LogError("Here is an error message from our values controller.");
            return new string[] { "value12", "value22" };
        }

    }
}