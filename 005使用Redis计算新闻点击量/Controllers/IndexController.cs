using _005使用Redis计算新闻点击量.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _005使用Redis计算新闻点击量.Controllers
{
    public class IndexController : Controller
    {

        //调试的时候随便给一个参数2：http://localhost:61530/Index/Index/2
        public ActionResult Index(int id)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("localhost:6379"))
            {
                IDatabase db = conn.GetDatabase();
                string userId = Request.UserHostAddress;

                string hasClickKey = "New" + id;//key为"New"+新闻的Id

                //若是当前的IP地址没有记录，则把IP地址记录在Redis数据库中，设置周期为1天
                //同时该新闻的点击量加1
                if (!db.KeyExists(userId))
                {
                    db.StringSet(userId, 1, TimeSpan.FromDays(1));
                    db.StringIncrement(hasClickKey, 1);//根据传来的id参数，给相应id的新闻的点击量加1
                                                       //注意哦我们不需要单独去在数据库中添加一个键值对，使用StringIncrement（）时若是数据库尚无该key则自动创建
                }


                long count = Convert.ToInt64(db.StringGet(hasClickKey));//读取出最新的点击量

                New news = new New() { Id = id, ClickCount = count };

                return View(news);
            }


        }
    }
}