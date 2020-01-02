using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

///我们在搜索框中输入搜索的字符串，会以sorted set类型被存储在Redis数据库的
///没搜索一次，则给该字符串打分加1
///在点击搜索之后，跳转到~/Index/Index,把搜索过的字符串按照打分降序排列的前5个查询出来，显示到页面


namespace _010Redis中使用sorted_set实现热搜.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {

            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                RedisValue[] values = db.SortedSetRangeByRank("Search_User_word", 0, -1, Order.Descending);

                string[] model = values.Select(w => w.ToString()).ToArray();
                return View(model);
            }

        }

        public ActionResult Search(string word)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                db.SortedSetIncrement("Search_User_word", word, 1);
                //todo:通过IP，防止作弊
            }
            return Redirect("Index/index");
        }
  
    }
}