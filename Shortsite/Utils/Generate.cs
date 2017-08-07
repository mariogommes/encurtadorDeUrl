using Shortsite.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Shortsite.Utils
{
    public class Generate
    {
        private string RedisServer = WebConfigurationManager.AppSettings["redisServer"];
        private string Domain = WebConfigurationManager.AppSettings["domain"];

        private string search_existent_site(string originalUrl, string redis_hash_name)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            string shortUrl = "";

            var allSites = db.HashGetAll("stored_sites");
            foreach (var item in allSites)
            {
                if (item.Value == originalUrl)
                {
                    shortUrl = Domain + "/" + item.Name;
                    break;
                }
            }
            return shortUrl;
        }

        private string search_existent_site(string originalUrl, string alias, string redis_hash_name)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            string shortUrl = "";

            var allsites = db.HashGetAll(redis_hash_name);
            foreach (var item in allsites)
            {
                if ((item.Value == originalUrl) && (item.Name == alias))
                {
                    shortUrl = Domain + "/" + item.Name;
                    break;
                }
            }
            return shortUrl;
        }

        public string shortSite(string originalUrl, string alias) {
            //conexão
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            SaveToDB save = new SaveToDB();
            RedisValue[] stored_sites_values = db.HashValues("stored_sites_alias");

            string shortUrl = "";

            //Criar ou não outro hash para alias customizados? Acho que sim 
            if ((!stored_sites_values.Contains(originalUrl)) && (!db.HashExists("stored_sites_alias", alias)))
            {
                string key = alias;
                shortUrl = Domain + "/" + key;
                save.saveRedis_alias(key, originalUrl);
                save.saveRedis_ranking(originalUrl);
                return shortUrl;
            }
            else if ((db.HashExists("stored_sites_alias", alias)) && (!stored_sites_values.Contains(originalUrl)))
            {
                throw new AliasExistsException();
            }
            else
            {
                shortUrl = search_existent_site(originalUrl, alias,"stored_sites_alias");

                if (string.IsNullOrEmpty(shortUrl))
                {
                    throw new AliasExistsException();
                }

                return shortUrl;
            }
        }

        public string shortSite(string originalUrl)
        {
            //conexão
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            SaveToDB save = new SaveToDB();
            byte MaxCodeLength =Convert.ToByte(WebConfigurationManager.AppSettings["randomAliasMaxLength"]);
            RedisValue[] stored_sites_values = db.HashValues("stored_sites");

            string shortUrl ="";

            if (!stored_sites_values.Contains(originalUrl))
            {
                Guid code = Guid.NewGuid();
                string key = code.ToString().Substring(0, MaxCodeLength);
                shortUrl = Domain + "/" + key;

                save.saveRedis_randomAlias(key, originalUrl);
                save.saveRedis_ranking(originalUrl);

                return shortUrl;
            }
            else {
                return search_existent_site(originalUrl, "stored_sites");
            }
        }
    }
}