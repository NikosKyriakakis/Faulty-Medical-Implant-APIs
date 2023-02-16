namespace GenericRepository.Settings;

public class DbSettings
{
    public string? Host { get; init; }
    public int Port { get; init; }
    public string ConnectionString { get; set; } = string.Empty;
}