using core.settings;
using ServiceStack.Redis;
using Microsoft.Extensions.Options;
using core.helpers;

namespace module.redis;

///<summary>
/// TODO: Eklenen sepetler müşteri bazlı olmalı
///</summary>
public class RedisService : IRedisService
{
    readonly IOptions<RedisCacheSettings> _options;
    readonly RedisManagerPool _redisManager;

    public RedisService(IOptions<RedisCacheSettings> options)
    {
        _options = options;
        _redisManager = new RedisManagerPool(options.Value.Master);
    }

    public void Set<T>(string key, T value, int expireMinute = 1440)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();
        redis.Set(key, value!.ToJson(), TimeSpan.FromMinutes(expireMinute));
    }

    public T Get<T>(string key)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();
        var strValue = redis.Get<string>(key);
        return strValue.FromJson<T>()!;
    }

    public void Delete(string key)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();
        redis.Remove(key);
    }

    public long Increment(string key, int expireMinute = 1440)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();

        var value = redis.Increment(key, 1);
        redis.ExpireEntryIn(key, TimeSpan.FromMinutes(expireMinute));
        return value;
    }

    public long Decrement(string key, int expireMinute = 1440)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();

        var value = redis.Decrement(key, 1);
        redis.ExpireEntryIn(key, TimeSpan.FromMinutes(expireMinute));
        return value;

    }

    public void AddToList<T>(string key, IList<T> values, int expireMinute = 1440)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();

        foreach (var item in values)
        {
            redis.AddItemToList(key, item!.ToJson());
        }

        redis.ExpireEntryIn(key, TimeSpan.FromMinutes(expireMinute));
    }

    public List<T> GetList<T>(string key)
    {
        key = GetKeyWithInstanceName(key);
        var result = new List<T>();

        using var redis = _redisManager.GetClient();

        var listResult = redis.GetAllItemsFromList(key);

        foreach (var item in listResult)
        {
            var model = item.FromJson<T>();
            result.Add(model);
        }

        return result;
    }

    public void AddToSet<T>(string key, T value, int expireMinute = 1440)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();
        redis.AddItemToSet(key, value!.ToJson());
        redis.ExpireEntryIn(key, TimeSpan.FromMinutes(expireMinute));
    }

    public bool KeyExists(string key)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();

        return redis.ContainsKey(key);
    }

    public void SetTTL(string key, int expireMinute)
    {
        key = GetKeyWithInstanceName(key);
        using var redis = _redisManager.GetClient();

        redis.ExpireEntryIn(key, TimeSpan.FromMinutes(expireMinute));
    }

    //TODO: GetSet, AddToSortedSet, GetSortedSet gibi diğer methodlar da yazılacak

    #region Helpers

    /// <summary>
    /// Key'lerin başına belirli key ekler
    /// </summary>
    string GetKeyWithInstanceName(string key)
    {
        return _options.Value.InstanceName + "_" + key;
    }

    #endregion
}