using Microsoft.AspNetCore.Mvc;
using poc_async_spot_instance_dlq_api.DTO;
using System.Net;

namespace poc_circuitbreake_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchitectureController : ControllerBase
    {
        private readonly ILogger<ArchitectureController> _logger;

        public ArchitectureController(ILogger<ArchitectureController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostArchitecture")]
        [ProducesResponseType(typeof(ArchitectureDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InsertArchitecture(ArchitectureDTO architectureDTO)
        {
            try
            {
                _logger.LogInformation("Inclus�o de arquitetura...");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na inclus�o da arquitetura.");
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(Name = "PutArchitecture")]
        [ProducesResponseType(typeof(ArchitectureDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateArchitecture(ArchitectureDTO architectureDTO)
        {
            try
            {
                _logger.LogInformation("Atualiza��o de arquitetura...");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na atualiza��o da arquitetura.");
                return BadRequest(ex.Message);
            }
        }
    }
}