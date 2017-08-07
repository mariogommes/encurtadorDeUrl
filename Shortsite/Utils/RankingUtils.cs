using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Shortsite.Utils
{
    public class RankingUtils
    {
        public List<HashEntry> get_topAccess_list() {            
            string RedisServer = WebConfigurationManager.AppSettings["redisServer"];
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();

            var top = db.HashGetAll("ranking").OrderByDescending(x => x.Value).Where(x => x.Value != 0);
            List<HashEntry> top10 = new List<HashEntry>();
            int i = 1;

            foreach (var item in top)
            {
                top10.Add(item);
                if (i == 10) { break; }
                ++i;
            }

            return top10;
        }
    }
}