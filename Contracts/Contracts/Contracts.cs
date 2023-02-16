namespace Contracts;

public class Contracts
{
    public record DeviceWritten
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
    }

    public record DeviceCreated : DeviceWritten { }
    public record DeviceUpdated : DeviceWritten { }

    public record DeviceDeleted
    {
        public Guid Id { get; set; }
    }
}