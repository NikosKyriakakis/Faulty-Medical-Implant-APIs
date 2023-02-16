using GenericRepository;

namespace EventsAPI.Models
{
    public class Event : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime Stamp { get; set; }
        public string? Reason { get; set; }
        public string? Type { get; set; }
        public Guid DeviceId { get; set; }
    }
}
