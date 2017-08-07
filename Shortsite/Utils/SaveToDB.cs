using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;
using System.Web.Configuration;
using Shortsite.Utils;

namespace Shortsite.Utils
{
    public class SaveToDB
    {
        private string RedisServer = WebConfigurationManager.AppSettings["redisServer"];
        private string Domain = WebConfigurationManager.AppSettings["domain"];

        public void increment_access_count(string originalUrl) {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            int access_count = Convert.ToInt32(db.HashGet("ranking", originalUrl));
            access_count++;
            db.HashSet("ranking", originalUrl, access_count);           
        }

        public void saveRedis_ranking(string originalUrl)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            int access_count = 0;
            //db.HashSet(redis_hash, originalUrl, access_count);
            if (!db.HashKeys("ranking").Contains(originalUrl))
            {
                //db.StringSet(originalUrl, access_count);
                db.HashSet("ranking", originalUrl, access_count);
            } 
        }

        public void saveRedis_alias(string key, string originalUrl)
        {
            string redis_hash = "stored_sites_alias";
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            db.HashSet(redis_hash, key, originalUrl);
        }

        public void saveRedis_randomAlias(string key, string originalUrl)
        {
            string redis_hash = "stored_sites";
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            db.HashSet(redis_hash, key, originalUrl);
        }

        public string get_originalUrl_from_redis(string key)
        {
            //conexão
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            string redis_hash = "stored_sites";
            string redis_hash_alias = "stored_sites_alias";
            IDatabase db = redis.GetDatabase();
            //pegando um valor
            string originalUrl = (db.HashGet(redis_hash, key).IsNull) ? db.HashGet(redis_hash_alias, key) : db.HashGet(redis_hash, key);

            return originalUrl;
        }
    }
}