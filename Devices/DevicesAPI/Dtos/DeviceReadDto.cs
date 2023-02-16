using System.ComponentModel.DataAnnotations;

namespace DevicesAPI.Dtos
{
    public record DeviceReadDto
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public string? Name { get; set; }

        [MaxLength(3)]
        public string? Country { get; set; }

        public string? Manufacturer { get; set; }
    }
}
