using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndApi.Models
{
    public class Pet
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Species { get; set; } = string.Empty; // Perro, Gato, Conejo, etc.

        [StringLength(100)]
        public string? Breed { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [Range(0, 50)]
        public int? Age { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Propiedad calculada para información completa
        [NotMapped]
        public string Description => $"{Name} - {Species}" + 
            (string.IsNullOrEmpty(Breed) ? "" : $" ({Breed})") +
            (string.IsNullOrEmpty(Color) ? "" : $" - {Color}");
    }
}
