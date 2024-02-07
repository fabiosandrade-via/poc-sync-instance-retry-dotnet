using Microsoft.AspNetCore.Mvc;
using poc_sync_spot_service_api.Model;

namespace poc_sync_spot_service_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotInstanceResponseController : ControllerBase
    {
        private readonly ILogger<SpotInstanceResponseController> _logger;

        public SpotInstanceResponseController(ILogger<SpotInstanceResponseController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetSpotInstanceResponse")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("In�cio de execu��o de chamada spot instance response...");
                return Ok(new SpotInstanceModel() { Message = "Servi�o de spot intance dispon�vel para consumo.", StatusCode = System.Net.HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na execu��o de spot instance.");
                return BadRequest(new SpotInstanceModel() { Message = ex.Message, StatusCode = System.Net.HttpStatusCode.BadRequest });
            }
        }
    }
}