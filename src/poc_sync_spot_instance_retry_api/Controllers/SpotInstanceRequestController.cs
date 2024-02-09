using Microsoft.AspNetCore.Mvc;
using poc_sync_spot_instance_retry_api.Models;
using poc_sync_spot_instance_retry_api.Resilience;

namespace poc_sync_spot_instance_retry_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotInstanceRequestController : ControllerBase
    {
        private readonly ILogger<SpotInstanceRequestController> _logger;
        private readonly IResilienceService _resilience;

        public SpotInstanceRequestController(ILogger<SpotInstanceRequestController> logger, IResilienceService resilience)
        {
            _logger = logger;
            _resilience = resilience;
        }

        [HttpGet(Name = "GetSpotInstanceRequest")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Início de execução de chamada spot instance request...");
                return Ok(_resilience.ExecuteAsync().Result);
            }
            catch(Exception ex) 
            {
                _logger.LogError("Erro na execução de spot instance.");
                return BadRequest(ex.Message);
            }
        }
    }
}