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
                _logger.LogInformation("Inclusão de arquitetura...");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na inclusão da arquitetura.");
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(Name = "PutArchitecture")]
        [ProducesResponseType(typeof(ArchitectureDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateArchitecture(ArchitectureDTO architectureDTO)
        {
            try
            {
                _logger.LogInformation("Atualização de arquitetura...");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na atualização da arquitetura.");
                return BadRequest(ex.Message);
            }
        }
    }
}