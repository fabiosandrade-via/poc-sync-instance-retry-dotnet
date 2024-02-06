using Microsoft.AspNetCore.Mvc;
using poc_sync_spot_instance_retry_api.Background;
using poc_sync_spot_instance_retry_api.Models;

namespace poc_sync_spot_instance_retry_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotInstanceRequestController : ControllerBase
    {
        private readonly ILogger<SpotInstanceRequestController> _logger;
        private readonly IWorker _worker;

        public SpotInstanceRequestController(ILogger<SpotInstanceRequestController> logger, IWorker worker)
        {
            _logger = logger;
            _worker = worker;
        }

        [HttpGet(Name = "GetSpotInstanceRequest")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("In�cio de execu��o de chamada spot instance request...");
                return Ok(_worker.ExecuteAsync().Result);
            }
            catch(Exception ex) 
            {
                _logger.LogError("Erro na execu��o de spot instance.");
                return BadRequest(ex.Message);
            }
        }
    }
}