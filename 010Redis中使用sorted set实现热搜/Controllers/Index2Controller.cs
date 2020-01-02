using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace _010Redis中使用sorted_set实现热搜.Controllers
{
    public class Index2Controller : Controller
    {



        //可以把搜索的词汇即分数数据在存入Redis数据库中同时也在内存中存储5分钟
        //这样在查询的时候，先去内存中读取，若是内存中没有，则再去Redis数据库中读取
        //此举可以降低redis服务器的压力
        #region 存入Redis数据库并且在内存中保存5分钟，先从内存读取，若无则再从Redis数据库读
        public ActionResult Index()
        {
            MemoryCache memCache = MemoryCache.Default;

            string[] hotwords = null;
            hotwords = (string[])memCache["hotwords"];
            if (hotwords != null)
            {
                Array.ForEach(hotwords, t => System.Diagnostics.Debug.WriteLine(t.ToString()));
                return View(hotwords);
            }

            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                RedisValue[] values = db.SortedSetRangeByRank("Search_User_word", 0, -1, Order.Descending);

                hotwords = values.Select(w => w.ToString()).ToArray();

                memCache.Add("hotwords", hotwords, DateTimeOffset.Now.AddSeconds(10));//添加到内存中

                return View(hotwords);
            }

        }

        public ActionResult Search(string word)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();

                db.SortedSetIncrement("Search_User_word", word, 1);
            }
            return Redirect("/Index2/Index");
        }
        #endregion
    }
}