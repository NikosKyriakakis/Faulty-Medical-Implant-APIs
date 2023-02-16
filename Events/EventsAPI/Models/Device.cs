using GenericRepository;

namespace EventsAPI.Models
{
    public class Device : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Manufacturer { get; set; }
    }
}