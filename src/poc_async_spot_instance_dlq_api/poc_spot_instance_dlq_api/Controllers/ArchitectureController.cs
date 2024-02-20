using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using poc_async_spot_instance_dlq_api.DTO;
using poc_async_spot_instance_dlq_api.Models;
using poc_async_spot_instance_dlq_api.Service;
using System.Net;

namespace poc_circuitbreake_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchitectureController : ControllerBase
    {
        private readonly ILogger<ArchitectureController> _logger;
        private readonly ISpotInstanceService _spotInstanceService;
        private readonly IMapper _mapper;

        public ArchitectureController(ILogger<ArchitectureController> logger, IMapper mapper, ISpotInstanceService spotInstanceService)
        {
            _logger = logger;
            _mapper = mapper;
            _spotInstanceService = spotInstanceService;
        }

        [HttpPost(Name = "PostArchitecture")]
        [ProducesResponseType(typeof(ArchitectureDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InsertArchitecture(ArchitectureDTO architectureDTO)
        {
            try
            {
                _logger.LogInformation("Inclusão de arquitetura...");
                var architectureModel = _mapper.Map<ArchitectureModel>(architectureDTO);
                return Ok(_spotInstanceService.ExecuteInsertAsync(architectureModel).Result);
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
                var architectureModel = _mapper.Map<ArchitectureModel>(architectureDTO);
                return Ok(_spotInstanceService.ExecuteUpdateAsync(architectureModel).Result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na atualização da arquitetura.");
                return BadRequest(ex.Message);
            }
        }
    }
}