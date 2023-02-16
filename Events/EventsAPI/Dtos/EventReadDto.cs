using System.ComponentModel.DataAnnotations;

namespace EventsAPI.Dtos
{
    public record EventReadDto
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public DateTime Stamp { get; set; }
        public string? Reason { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Manufacturer { get; set; }
    }
}
