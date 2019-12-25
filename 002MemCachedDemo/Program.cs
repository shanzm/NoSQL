using Enyim.Caching;
using Enyim.Caching.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _002MemCachedDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            //Test2();
            //Test3();
        }

        //配置&存入&读取&删除
        public static void Test1()
        {
            #region 说明
            //NuGet:PM> Install-Package EnyimMemcached
            //关于MemCached的程序包有很多，EnyimMemcached只是其一
            //Memcache 存入数据的 3 中模式 Set、Replace、Add，根据名字就能猜出来：
            //Set：存在则覆盖，不存在则新增
            //Replace：如果存在则覆盖，并且返回 true；如果不存在则不处理，并且返回 false；(Replace,为替换，能替换的意义就是先存在)
            //Add：如果不存在则新增，并且返回 true；如果存在则不处理，并且返回 false（Add，为新增，能新增的前提是之前不存在）
            //常用的就是Set方式

            //注意数据是存放在MemCached服务器中的，你重启程序依旧在服务器中存在你存入的数据
            //但是你一旦重启memcached服务器，则数据就消失了
            #endregion

            //创建配置对象
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            memConfig.AddServer("127.0.0.1:11211");

            //创建MemcachedClient对象
            using (MemcachedClient memClient = new MemcachedClient(memConfig))
            {
                //写入MemCached中
                memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "Name", "shanzm");
                memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "Age", "100");

                //读取数据
                // string name = (string)memClient.Get("Name");//Get<T>（）是一个泛型方法，你可直接使用Get（）函数得到一个Object类型的对象，或提供泛型类型
                string name = memClient.Get<string>("Name");
                if (name == null)
                {
                    Console.WriteLine("无缓存");
                }
                else
                {
                    Console.WriteLine(name);
                }

                //删除数据
                Console.WriteLine(memClient.Get<string>("Age"));
                memClient.Remove("Age");
                if (null == memClient.Get<string>("Age"))
                {
                    Console.WriteLine("已经将Key为Age的数据从MemCached服务器中清除");

                }
                Console.ReadKey();
            }
        }


        //存储对象的类型若是自定义类的，则其必须是可序列化的
        public static void Test2()
        {
            //注意若是想把自定义的类的对象保存在MemCached服务器中，则这个类必须是可以序列化的
            //看我们自己定义的Person类，注意是加了[Serializable]特性的,若是不加可序列化的特性，则是无法把Person类型的对象保存在MemCached中的
            Person p = new Person() { Id = 001, Name = "shanzm" };

            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            memConfig.AddServer("127.0.0.1:11211");

            using (MemcachedClient memClient = new MemcachedClient(memConfig))
            {

                memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "p", p);
                Person perosn = memClient.Get<Person>("p");

                if (null == perosn)
                {
                    Console.WriteLine("无perosn缓存");

                }
                else
                {
                    Console.WriteLine($"{ perosn.Name}的Id是 {perosn.Id}");
                }
            }
            Console.ReadKey();
        }


        //设置存储时间
        public static void Test3()
        {
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            memConfig.AddServer("127.0.0.1:11211");
            using (MemcachedClient memClient = new MemcachedClient(memConfig))
            {
                string id = "001";
                string Name = "shanzm";
                memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "id", id, TimeSpan.FromSeconds(10));//存储10s,10s后过期
                memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "Name", Name, DateTime.Now.AddSeconds(10));//当前时间加10s之后的那个时间点过期
                Console.WriteLine(memClient.Get<string>("id") + memClient.Get<string>("Name"));
                Thread.Sleep(TimeSpan.FromSeconds(10));
                if (memClient.Get<string>("id") == null)
                {
                    Console.WriteLine("已过期");
                }


            }
            Console.ReadKey();
        }
    }
}
