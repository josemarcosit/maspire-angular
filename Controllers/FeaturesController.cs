using angular_vega.Core;
using angular_vega.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace angular_vega.Controllers
{
    [Route("api/[controller]")]   
    [Authorize]
    public class FeaturesController : Controller
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly ILogger<FeaturesController> _logger;
        private readonly IMapper _mapper;

        public FeaturesController(
            IFeatureRepository featureRepository,
            ILogger<FeaturesController> logger,
            IMapper mapper)
        {
            _featureRepository = featureRepository;
            _logger = logger;
            _mapper = mapper;
        }
     
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Feature>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeatures()
        {
            try
            {
                var features = await _featureRepository.GetAllFeaturesAsync();
                return Ok(features);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter características");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter características");
            }
        }
       
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeature(int id)
        {
            try
            {
                var feature = await _featureRepository.GetFeatureByIdAsync(id);
                if (feature == null)
                {
                    return NotFound("Característica não encontrada");
                }
                return Ok(feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a característica com ID: {FeatureId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter a característica");
            }
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateFeature([FromBody] Feature feature)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _featureRepository.AddFeatureAsync(feature);
                return CreatedAtAction(nameof(GetFeature), new { id = feature.Id }, feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar característica");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar característica");
            }
        }
       
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFeature(int id, [FromBody] Feature feature)
        {
            try
            {
                if (id != feature.Id)
                {
                    return BadRequest("ID não corresponde");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingFeature = await _featureRepository.GetFeatureByIdAsync(id);
                if (existingFeature == null)
                {
                    return NotFound("Característica não encontrada");
                }

                await _featureRepository.UpdateFeatureAsync(feature);
                return Ok(feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar característica com ID: {FeatureId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar característica");
            }
        }
      
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            try
            {
                var feature = await _featureRepository.GetFeatureByIdAsync(id);
                if (feature == null)
                {
                    return NotFound("Característica não encontrada");
                }

                await _featureRepository.DeleteFeatureAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar característica com ID: {FeatureId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar característica");
            }
        }
    }
}