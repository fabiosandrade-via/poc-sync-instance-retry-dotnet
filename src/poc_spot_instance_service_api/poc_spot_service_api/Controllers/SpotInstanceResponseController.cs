using Microsoft.AspNetCore.Mvc;
using poc_spot_service_api.DTO;
using poc_sync_spot_service_api.Model;
using System.Net;

namespace poc_sync_spot_service_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                _logger.LogInformation("Consulta spot instance response...");
                return Ok(new SpotInstanceModel() { Message = "Consulta de arquiteturas recomendáveis na spot intance disponível para consumo.", StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na consulta de serviço na execução de spot instance.");
                return BadRequest(new SpotInstanceModel() { Message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }
        }
        [HttpPost(Name = "PostSpotInstanceResponse")]
        [ProducesResponseType(typeof(ArchitectureDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post(ArchitectureDTO architectureDTO)
        {
            try
            {
                _logger.LogInformation("Inclusão de arquitetura recomendável no serviço de spot instance response...");
                return Ok(new SpotInstanceModel() { Message = "Inclusão de arquiteturas recomendáveis na spot intance disponível para consumo.", StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na inclusão de serviço na execução de spot instance.");
                return BadRequest(new SpotInstanceModel() { Message = ex.Message, StatusCode = System.Net.HttpStatusCode.BadRequest });
            }
        }
        [HttpPut(Name = "PutSpotInstanceResponse")]
        [ProducesResponseType(typeof(ArchitectureDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put(ArchitectureDTO architectureDTO)
        {
            try
            {
                _logger.LogInformation("Alteração de arquitetura recomendável no serviço de spot instance response...");
                return Ok(new SpotInstanceModel() { Message = "Alteração de arquiteturas recomendáveis na spot intance disponível para consumo.", StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na alteração de serviço na execução de spot instance.");
                return BadRequest(new SpotInstanceModel() { Message = ex.Message, StatusCode = System.Net.HttpStatusCode.BadRequest });
            }
        }
    }
}