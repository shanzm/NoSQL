using System;
using StackExchange.Redis;
using System.Threading;

namespace _007模拟注册发送邮件验证_发送邮件模块
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();


                while (true)
                {
                    string eamil = db.ListRightPop("User_email").ToString();//从右侧弹出（在注册页面是左侧压入）
                    if (eamil != null)
                    {
                        Console.WriteLine($"正在给{eamil}邮箱发送验证码！");
                        Console.WriteLine($"{eamil}邮箱验证码已发送！");
                    }
                    else
                    {
                        Console.WriteLine("无邮件可发送！");
                        Thread.Sleep(1000);//防止CPU空转
                    }

                }
            }

        }
    }
}
