namespace module.redis;

public interface IRedisService
{
    void Set<T>(string key, T value, int expireMinute = 1440);
    T Get<T>(string key);
    void Delete(string key);
    long Increment(string key, int expireMinute = 1440);
    long Decrement(string key, int expireMinute = 1440);
    void AddToList<T>(string key, IList<T> values, int expireMinute = 1440);
    List<T> GetList<T>(string key);
    void AddToSet<T>(string key, T value, int expireMinute = 1440);
    bool KeyExists(string key);
    void SetTTL(string key, int expireMinute);
}
