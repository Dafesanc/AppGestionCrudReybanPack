using System.ComponentModel.DataAnnotations;

namespace BackEndApi.DTOs
{
    public class PetCreateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especie es requerida")]
        [StringLength(50, ErrorMessage = "La especie no puede exceder 50 caracteres")]
        public string Species { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "La raza no puede exceder 100 caracteres")]
        public string? Breed { get; set; }

        [StringLength(50, ErrorMessage = "El color no puede exceder 50 caracteres")]
        public string? Color { get; set; }

        [Range(0, 50, ErrorMessage = "La edad debe estar entre 0 y 50 años")]
        public int? Age { get; set; }
    }

    public class PetUpdateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especie es requerida")]
        [StringLength(50, ErrorMessage = "La especie no puede exceder 50 caracteres")]
        public string Species { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "La raza no puede exceder 100 caracteres")]
        public string? Breed { get; set; }

        [StringLength(50, ErrorMessage = "El color no puede exceder 50 caracteres")]
        public string? Color { get; set; }

        [Range(0, 50, ErrorMessage = "La edad debe estar entre 0 y 50 años")]
        public int? Age { get; set; }
    }

    public class PetReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string? Breed { get; set; }
        public string? Color { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
