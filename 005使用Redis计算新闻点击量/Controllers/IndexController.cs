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

                string hasClickKey = "New" + id + userId;//key中包含用户的IP地址(::1)，所以一个IP只能点击一次

                //Redis数据库中不存在，则创建并加1
                if (!db.KeyExists(hasClickKey))
                {
                    //db.StringSet(hasClickKey, 0, TimeSpan.FromDays(1));//在Redis数据库中初始化,只保存一天，一天只能点击一次
                    db.StringIncrement(hasClickKey, 1);//根据传来的id参数，给相应id的新闻的点击量加1
                }




                long count = Convert.ToInt64(db.StringGet(hasClickKey));//读取出最新的点击量

                New news = new New() { Id = id, ClickCount = count };

                return View(news);
            }


        }
    }
}