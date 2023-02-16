namespace EventsAPI.Dtos
{
    public record EventWriteDto
    {
        public DateTime Stamp { get; set; }
        public string? Reason { get; set; }
        public string? Type { get; set; }
        public Guid DeviceId { get; set; }

    }
}
