using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StackExchange.Redis;

///我们使用这个项目和-----模拟注册时邮箱的验证
///如果在注册的时候，在用户输入邮箱等信息后直接连接邮箱验证程序，则会出现等待
///尤其是使用的邮箱、短信等服务都是租用其他服务商的，所以连接提供商会需要一段时间
///所以可以在用户提交了用户信息后，把数据写入数据库，把邮箱地址从左侧再写入Redis数据库的list中
///而发送邮件的程序，直接从Redis数据库的list中右侧弹出邮箱地址发送验证邮件
///在发送了验证邮件后，也就在Redis数据库中删除该邮箱地址


namespace _007模拟注册发送邮件验证
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                db.ListLeftPush("User_email", txtEmail.Text);//从左侧进入List中
                //在发送验证邮件的程序中，从右侧依次弹出
            }
            labSuc.Text = "注册成功！";
        }
    }
}