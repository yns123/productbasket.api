namespace core.settings;

public record MongoSettings
{
    public string? ConnectionString { get; set; }
    public string? Database { get; set; }
}
