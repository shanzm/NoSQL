using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _003Cas操作
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCas();
        }

        //Cas操作
        //Cas 类似数据库的乐观锁，但是我们使用Memcached一般只是用来缓存数据，一般是不需要使用Cas的
        public static void TestCas()
        {
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            memConfig.AddServer("127.0.0.1:11211");
            using (MemcachedClient memClient = new MemcachedClient(memConfig))
            {
                memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "Name", "shanzm");

                CasResult<string> nameWithCas = memClient.GetWithCas<string>("Name");
                Console.WriteLine($"读取的数据Name={nameWithCas.Result },Cas：{nameWithCas.Cas }");//第一个运行的程序此时的Cas=1

                Console.WriteLine("你此时在源文件中点击运行，“003Cas操作.exe”，抢先运行,点击回车进行下面的修改");//另外的程序读取Name的Cas=2
                Console.ReadKey();

                CasResult<bool> updateWithCas = memClient.Cas(StoreMode.Set, "Name", "shanzm修改版本", nameWithCas.Cas);

                if (updateWithCas.Result)
                {
                    Console.WriteLine("Name修改成功,现在的Cas：" + updateWithCas.Cas);//另外的程序此处的Cas为3
                }
                else
                {
                    Console.WriteLine("更新失败,被其他程序抢先了，现在的Cas：" + updateWithCas.Cas);//第一个运行的程序的Cas此时为0
                }
                Console.ReadKey();

            }
        }
    }
}
