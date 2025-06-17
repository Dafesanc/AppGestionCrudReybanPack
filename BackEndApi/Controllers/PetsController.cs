using AutoMapper;
using BackEndApi.Common;
using BackEndApi.DTOs;
using BackEndApi.Interfaces;
using BackEndApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PetsController> _logger;

        public PetsController(
            IPetRepository petRepository, 
            IMapper mapper, 
            ILogger<PetsController> logger)
        {
            _petRepository = petRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las mascotas
        /// </summary>
        /// <returns>Lista de mascotas</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetReadDto>>>> GetAllPets()
        {
            try
            {
                var pets = await _petRepository.GetAllAsync();
                var petsDto = _mapper.Map<IEnumerable<PetReadDto>>(pets);
                
                return Ok(ApiResponse<IEnumerable<PetReadDto>>.SuccessResult(
                    petsDto, 
                    $"Se encontraron {petsDto.Count()} mascotas"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las mascotas");
                return StatusCode(500, ApiResponse<IEnumerable<PetReadDto>>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudieron obtener las mascotas"));
            }
        }

        /// <summary>
        /// Obtiene una mascota por ID
        /// </summary>
        /// <param name="id">ID de la mascota</param>
        /// <returns>Datos de la mascota</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PetReadDto>>> GetPetById(Guid id)
        {
            try
            {
                var pet = await _petRepository.GetByIdAsync(id);
                
                if (pet == null)
                {
                    return NotFound(ApiResponse<PetReadDto>.ErrorResult(
                        "Mascota no encontrada", 
                        $"No existe una mascota con el ID: {id}"));
                }

                var petDto = _mapper.Map<PetReadDto>(pet);
                return Ok(ApiResponse<PetReadDto>.SuccessResult(
                    petDto, 
                    "Mascota encontrada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la mascota con ID: {Id}", id);
                return StatusCode(500, ApiResponse<PetReadDto>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo obtener la mascota"));
            }
        }

        /// <summary>
        /// Crea una nueva mascota
        /// </summary>
        /// <param name="petCreateDto">Datos de la mascota a crear</param>
        /// <returns>Mascota creada</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PetReadDto>>> CreatePet([FromBody] PetCreateDto petCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(ApiResponse<PetReadDto>.ErrorResult(
                        "Datos de entrada inválidos", errors));
                }

                var pet = _mapper.Map<Pet>(petCreateDto);
                var createdPet = await _petRepository.CreateAsync(pet);
                var petDto = _mapper.Map<PetReadDto>(createdPet);

                return CreatedAtAction(
                    nameof(GetPetById), 
                    new { id = createdPet.Id }, 
                    ApiResponse<PetReadDto>.SuccessResult(
                        petDto, 
                        "Mascota creada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la mascota");
                return StatusCode(500, ApiResponse<PetReadDto>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo crear la mascota"));
            }
        }

        /// <summary>
        /// Actualiza una mascota existente
        /// </summary>
        /// <param name="id">ID de la mascota</param>
        /// <param name="petUpdateDto">Datos actualizados</param>
        /// <returns>Mascota actualizada</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PetReadDto>>> UpdatePet(Guid id, [FromBody] PetUpdateDto petUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(ApiResponse<PetReadDto>.ErrorResult(
                        "Datos de entrada inválidos", errors));
                }

                // Verificar si la mascota existe
                var existingPet = await _petRepository.GetByIdAsync(id);
                if (existingPet == null)
                {
                    return NotFound(ApiResponse<PetReadDto>.ErrorResult(
                        "Mascota no encontrada", 
                        $"No existe una mascota con el ID: {id}"));
                }                var petToUpdate = _mapper.Map<Pet>(petUpdateDto);
                petToUpdate.Id = id;

                var updatedPet = await _petRepository.UpdateAsync(id, petToUpdate);
                var petDto = _mapper.Map<PetReadDto>(updatedPet);

                return Ok(ApiResponse<PetReadDto>.SuccessResult(
                    petDto, 
                    "Mascota actualizada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la mascota con ID: {Id}", id);
                return StatusCode(500, ApiResponse<PetReadDto>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo actualizar la mascota"));
            }
        }

        /// <summary>
        /// Elimina una mascota
        /// </summary>
        /// <param name="id">ID de la mascota</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse>> DeletePet(Guid id)
        {
            try
            {
                var exists = await _petRepository.ExistsAsync(id);
                if (!exists)
                {
                    return NotFound(ApiResponse.ErrorResult(
                        "Mascota no encontrada", 
                        $"No existe una mascota con el ID: {id}"));
                }

                var deleted = await _petRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return StatusCode(500, ApiResponse.ErrorResult(
                        "Error al eliminar", 
                        "No se pudo eliminar la mascota"));
                }

                return Ok(ApiResponse.SuccessResult("Mascota eliminada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la mascota con ID: {Id}", id);
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo eliminar la mascota"));
            }
        }

        /// <summary>
        /// Busca mascotas por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de mascotas que coinciden</returns>
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetReadDto>>>> SearchPets([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest(ApiResponse<IEnumerable<PetReadDto>>.ErrorResult(
                        "Término de búsqueda requerido", 
                        "Debe proporcionar un término de búsqueda"));
                }

                var pets = await _petRepository.SearchByNameAsync(searchTerm);
                var petsDto = _mapper.Map<IEnumerable<PetReadDto>>(pets);

                return Ok(ApiResponse<IEnumerable<PetReadDto>>.SuccessResult(
                    petsDto, 
                    $"Se encontraron {petsDto.Count()} mascotas que coinciden con '{searchTerm}'"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar mascotas con término: {SearchTerm}", searchTerm);
                return StatusCode(500, ApiResponse<IEnumerable<PetReadDto>>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo realizar la búsqueda"));
            }
        }

        /// <summary>
        /// Obtiene mascotas por especie
        /// </summary>
        /// <param name="species">Especie de la mascota</param>
        /// <returns>Lista de mascotas de la especie especificada</returns>
        [HttpGet("species/{species}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetReadDto>>>> GetPetsBySpecies(string species)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(species))
                {
                    return BadRequest(ApiResponse<IEnumerable<PetReadDto>>.ErrorResult(
                        "Especie requerida", 
                        "Debe proporcionar una especie"));
                }

                var pets = await _petRepository.GetBySpeciesAsync(species);
                var petsDto = _mapper.Map<IEnumerable<PetReadDto>>(pets);

                return Ok(ApiResponse<IEnumerable<PetReadDto>>.SuccessResult(
                    petsDto, 
                    $"Se encontraron {petsDto.Count()} mascotas de la especie '{species}'"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener mascotas por especie: {Species}", species);
                return StatusCode(500, ApiResponse<IEnumerable<PetReadDto>>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudieron obtener las mascotas"));
            }
        }

        /// <summary>
        /// Obtiene mascotas por rango de edad
        /// </summary>
        /// <param name="minAge">Edad mínima</param>
        /// <param name="maxAge">Edad máxima</param>
        /// <returns>Lista de mascotas en el rango de edad</returns>
        [HttpGet("age-range")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetReadDto>>>> GetPetsByAgeRange([FromQuery] int minAge, [FromQuery] int maxAge)
        {
            try
            {
                if (minAge < 0 || maxAge < 0 || minAge > maxAge)
                {
                    return BadRequest(ApiResponse<IEnumerable<PetReadDto>>.ErrorResult(
                        "Rango de edad inválido", 
                        "El rango de edad debe ser válido (minAge >= 0, maxAge >= 0, minAge <= maxAge)"));
                }

                var pets = await _petRepository.GetByAgeRangeAsync(minAge, maxAge);
                var petsDto = _mapper.Map<IEnumerable<PetReadDto>>(pets);

                return Ok(ApiResponse<IEnumerable<PetReadDto>>.SuccessResult(
                    petsDto, 
                    $"Se encontraron {petsDto.Count()} mascotas entre {minAge} y {maxAge} años"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener mascotas por rango de edad: {MinAge}-{MaxAge}", minAge, maxAge);
                return StatusCode(500, ApiResponse<IEnumerable<PetReadDto>>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudieron obtener las mascotas"));
            }
        }
    }
}
