using GenericRepository.Settings;

namespace MongoRepository.Settings;

public class MongoSettings : DbSettings
{
    public new string? Host { get; init; }
    public new int Port { get; init; }
    public new string ConnectionString => $"mongodb://{Host}:{Port}";
}