namespace core.settings;

public record RedisCacheSettings
{
    public string? Master { get; set; }
    public string? Slave { get; set; }
    public string? Password { get; set; }
    public string? InstanceName { get; set; }
    public int ConnectTimeout { get; set; }
    public int Db { get; set; }
}
