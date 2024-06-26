using Microsoft.AspNetCore.Mvc;
using poc_sync_spot_instance_retry_api.Service;

namespace poc_sync_spot_instance_retry_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotInstanceRequestController : ControllerBase
    {
        private readonly ILogger<SpotInstanceRequestController> _logger;
        private readonly ISpotInstanceService _spotInstanceService;

        public SpotInstanceRequestController(ILogger<SpotInstanceRequestController> logger, ISpotInstanceService spotInstanceService)
        {
            _logger = logger;
            _spotInstanceService = spotInstanceService;
        }

        [HttpGet(Name = "GetSpotInstanceRequest")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("In�cio de execu��o de chamada spot instance request...");
                return Ok(_spotInstanceService.ExecuteAsync().Result);
            }
            catch(Exception ex) 
            {
                _logger.LogError("Erro na execu��o de spot instance.");
                return BadRequest(ex.Message);
            }
        }
    }
}