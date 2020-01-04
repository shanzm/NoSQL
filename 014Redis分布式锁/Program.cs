using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Threading;

//调试的时候，你点击运行此程序后，立刻（3s)在点击运行一次就会发现进程被先运行的程序锁定

namespace _014Redis分布式锁
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                RedisValue token = Environment.MachineName;//锁定为本台机器，//秒杀项目此处可换成商品 ID
                if (db.LockTake("myLock", token, TimeSpan.FromSeconds(10)))//第三个参数为锁超时时间，锁占用最多 10 秒钟，
                                                                           // 超过 10 秒钟如果还没有 LockRelease，则也自动释放锁，避免了死锁
                {
                    try
                    {
                        Console.WriteLine("操作中");
                        Thread.Sleep(3000);
                        Console.WriteLine("操作完成");
                    }
                    finally
                    {
                        db.LockRelease("myLock", token);
                    }
                }
                else
                {
                    Console.WriteLine("获得锁失败");
                }

            }
        }
    }
}
