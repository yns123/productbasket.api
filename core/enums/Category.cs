namespace core.enums;

[Flags]
public enum Category
{
    Smartphone = 1,
    Tablet = 2,
    Laptop = 4,
    Desktop = 8,
    GamingConsole = 16,
    SmartTV = 32,
    Headphone = 64,
    SmartHomeDevice = 128,
    Camera = 256,
    SoundSystem = 512
}
