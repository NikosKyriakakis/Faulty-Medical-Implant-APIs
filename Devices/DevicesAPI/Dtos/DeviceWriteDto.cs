using System.ComponentModel.DataAnnotations;

namespace DevicesAPI.Dtos
{
    public record DeviceWriteDto
    {
        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Slug { get; set; }

        [MaxLength(3)]
        public string? Country { get; set; }

        [Required]
        public string? Manufacturer { get; set; }
    }

    public record DeviceCreateDto : DeviceWriteDto { }

    public record DeviceUpdateDto : DeviceWriteDto { }
}
