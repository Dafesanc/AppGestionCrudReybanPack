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
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(
            IPersonRepository personRepository, 
            IMapper mapper, 
            ILogger<PersonsController> logger)
        {
            _personRepository = personRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las personas
        /// </summary>
        /// <returns>Lista de personas</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PersonReadDto>>>> GetAllPersons()
        {
            try
            {
                var persons = await _personRepository.GetAllAsync();
                var personsDto = _mapper.Map<IEnumerable<PersonReadDto>>(persons);
                
                return Ok(ApiResponse<IEnumerable<PersonReadDto>>.SuccessResult(
                    personsDto, 
                    $"Se encontraron {personsDto.Count()} personas"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las personas");
                return StatusCode(500, ApiResponse<IEnumerable<PersonReadDto>>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudieron obtener las personas"));
            }
        }

        /// <summary>
        /// Obtiene una persona por ID
        /// </summary>
        /// <param name="id">ID de la persona</param>
        /// <returns>Datos de la persona</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PersonReadDto>>> GetPersonById(Guid id)
        {
            try
            {
                var person = await _personRepository.GetByIdAsync(id);
                
                if (person == null)
                {
                    return NotFound(ApiResponse<PersonReadDto>.ErrorResult(
                        "Persona no encontrada", 
                        $"No existe una persona con el ID: {id}"));
                }

                var personDto = _mapper.Map<PersonReadDto>(person);
                return Ok(ApiResponse<PersonReadDto>.SuccessResult(
                    personDto, 
                    "Persona encontrada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la persona con ID: {Id}", id);
                return StatusCode(500, ApiResponse<PersonReadDto>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo obtener la persona"));
            }
        }

        /// <summary>
        /// Crea una nueva persona
        /// </summary>
        /// <param name="personCreateDto">Datos de la persona a crear</param>
        /// <returns>Persona creada</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PersonReadDto>>> CreatePerson([FromBody] PersonCreateDto personCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(ApiResponse<PersonReadDto>.ErrorResult(
                        "Datos de entrada inválidos", errors));
                }

                // Verificar si el email ya existe
                if (await _personRepository.EmailExistsAsync(personCreateDto.Email))
                {
                    return Conflict(ApiResponse<PersonReadDto>.ErrorResult(
                        "Email ya registrado", 
                        $"Ya existe una persona con el email: {personCreateDto.Email}"));
                }

                var person = _mapper.Map<Person>(personCreateDto);
                var createdPerson = await _personRepository.CreateAsync(person);
                var personDto = _mapper.Map<PersonReadDto>(createdPerson);

                return CreatedAtAction(
                    nameof(GetPersonById), 
                    new { id = createdPerson.Id }, 
                    ApiResponse<PersonReadDto>.SuccessResult(
                        personDto, 
                        "Persona creada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la persona");
                return StatusCode(500, ApiResponse<PersonReadDto>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo crear la persona"));
            }
        }

        /// <summary>
        /// Actualiza una persona existente
        /// </summary>
        /// <param name="id">ID de la persona</param>
        /// <param name="personUpdateDto">Datos actualizados</param>
        /// <returns>Persona actualizada</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PersonReadDto>>> UpdatePerson(Guid id, [FromBody] PersonUpdateDto personUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(ApiResponse<PersonReadDto>.ErrorResult(
                        "Datos de entrada inválidos", errors));
                }

                // Verificar si la persona existe
                var existingPerson = await _personRepository.GetByIdAsync(id);
                if (existingPerson == null)
                {
                    return NotFound(ApiResponse<PersonReadDto>.ErrorResult(
                        "Persona no encontrada", 
                        $"No existe una persona con el ID: {id}"));
                }

                // Verificar si el email ya existe (excluyendo la persona actual)
                if (await _personRepository.EmailExistsAsync(personUpdateDto.Email, id))
                {
                    return Conflict(ApiResponse<PersonReadDto>.ErrorResult(
                        "Email ya registrado", 
                        $"Ya existe otra persona con el email: {personUpdateDto.Email}"));
                }

                var personToUpdate = _mapper.Map<Person>(personUpdateDto);
                personToUpdate.Id = id;
                personToUpdate.CreatedAt = existingPerson.CreatedAt;

                var updatedPerson = await _personRepository.UpdateAsync(id, personToUpdate);
                var personDto = _mapper.Map<PersonReadDto>(updatedPerson);

                return Ok(ApiResponse<PersonReadDto>.SuccessResult(
                    personDto, 
                    "Persona actualizada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la persona con ID: {Id}", id);
                return StatusCode(500, ApiResponse<PersonReadDto>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo actualizar la persona"));
            }
        }

        /// <summary>
        /// Elimina una persona
        /// </summary>
        /// <param name="id">ID de la persona</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse>> DeletePerson(Guid id)
        {
            try
            {
                var exists = await _personRepository.ExistsAsync(id);
                if (!exists)
                {
                    return NotFound(ApiResponse.ErrorResult(
                        "Persona no encontrada", 
                        $"No existe una persona con el ID: {id}"));
                }

                var deleted = await _personRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return StatusCode(500, ApiResponse.ErrorResult(
                        "Error al eliminar", 
                        "No se pudo eliminar la persona"));
                }

                return Ok(ApiResponse.SuccessResult("Persona eliminada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la persona con ID: {Id}", id);
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo eliminar la persona"));
            }
        }

        /// <summary>
        /// Busca personas por nombre o apellido
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de personas que coinciden</returns>
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PersonReadDto>>>> SearchPersons([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest(ApiResponse<IEnumerable<PersonReadDto>>.ErrorResult(
                        "Término de búsqueda requerido", 
                        "Debe proporcionar un término de búsqueda"));
                }

                var persons = await _personRepository.SearchByNameAsync(searchTerm);
                var personsDto = _mapper.Map<IEnumerable<PersonReadDto>>(persons);

                return Ok(ApiResponse<IEnumerable<PersonReadDto>>.SuccessResult(
                    personsDto, 
                    $"Se encontraron {personsDto.Count()} personas que coinciden con '{searchTerm}'"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar personas con término: {SearchTerm}", searchTerm);
                return StatusCode(500, ApiResponse<IEnumerable<PersonReadDto>>.ErrorResult(
                    "Error interno del servidor", 
                    "No se pudo realizar la búsqueda"));
            }
        }
    }
}
