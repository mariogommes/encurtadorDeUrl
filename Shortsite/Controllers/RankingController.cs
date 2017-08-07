using Shortsite.Models;
using Shortsite.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Shortsite.Controllers
{
    public class RankingController : Controller
    {
        // GET: Ranking
        public ActionResult Index()
        {
            string RedisServer = WebConfigurationManager.AppSettings["redisServer"];
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(RedisServer);
            IDatabase db = redis.GetDatabase();
            RankingModel model = new RankingModel();
            RankingUtils ranking = new RankingUtils();
            model.Ranking = ranking.get_topAccess_list();

            return View(model);
        }
    }
}