using System.Globalization;
using angular_vega.Core;
using angular_vega.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace angular_vega.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FeaturesController : Controller
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly ILogger<FeaturesController> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public FeaturesController(
            IFeatureRepository featureRepository,
            ILogger<FeaturesController> logger,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer)
        {
            _featureRepository = featureRepository;
            _logger = logger;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Feature>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeatures([FromQuery(Name = "lang")] string language)
        {
            try
            {
                language = string.IsNullOrWhiteSpace(language) ? CultureInfo.CurrentUICulture.Name : language;
                var features = await _featureRepository.GetAllFeaturesAsync(language);
                return Ok(features);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter características");
                return StatusCode(StatusCodes.Status500InternalServerError, _localizer["FeatureFetchError"]);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeature(int id, [FromQuery(Name = "lang")] string language)
        {
            try
            {
                language = string.IsNullOrWhiteSpace(language) ? CultureInfo.CurrentUICulture.Name : language;
                var feature = await _featureRepository.GetFeatureByIdAsync(id, language);
                if (feature == null)
                {
                    return NotFound(_localizer["FeatureNotFound"]);
                }
                return Ok(feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a característica com ID: {FeatureId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, _localizer["FeatureFetchError"]);
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
                return StatusCode(StatusCodes.Status500InternalServerError, _localizer["FeatureFetchError"]);
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
                    return BadRequest(_localizer["IdMismatch"]);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingFeature = await _featureRepository.GetFeatureByIdAsync(id);
                if (existingFeature == null)
                {
                    return NotFound(_localizer["FeatureNotFound"]);
                }

                await _featureRepository.UpdateFeatureAsync(feature);
                return Ok(feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar característica com ID: {FeatureId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, _localizer["FeatureFetchError"]);
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
                    return NotFound(_localizer["FeatureNotFound"]);
                }

                await _featureRepository.DeleteFeatureAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar característica com ID: {FeatureId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, _localizer["FeatureFetchError"]);
            }
        }
    }
}