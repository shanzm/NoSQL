using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
namespace _013Redis的批量操作
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                //注意若是对于Redis数据库的操作过于频繁亦会影响性能

                //所以对于需要大量插入数据库的数据若是同一类型，则使用相应的XXXEntry[]数组插入
                //Redis中的6种数据类型的插入皆可以使用数据插入

                //批量插入<RedisKey key,string value>
                //InsertString(db);


                //对特定的list批量插入数据
                //InsertList(db);


                //对于特定的set批量插入数据
                //InsertSet(db);

                //一次性插入多种不同类型的数据到Redis数据库
                InsertByBatch(db);

                Console.ReadKey();


            }
        }


        //批量插入<RedisKey key,string value>
        public static void InsertString(IDatabase db)
        {

            KeyValuePair<RedisKey, RedisValue>[] values =
            {
                    new KeyValuePair<RedisKey, RedisValue> ("A","a" ),
                    new KeyValuePair<RedisKey, RedisValue> ("B","b"),
                    new KeyValuePair<RedisKey, RedisValue> ("C","c")
                };
            db.StringSet(values);
        }


        //对特定的list批量插入数据
        public static void InsertList(IDatabase db)
        {
            RedisValue[] values = { "shanzm", "shanzm2", "shanzm3" };
            db.ListRightPush("listTest", values);
        }

        //对于特定的set批量插入数据
        public static void InsertSet(IDatabase db)
        {
            RedisValue[] values = { "a", "a", "b", "c", "d" };
            db.SetAdd("setTest", values);
        }


        //一次性插入多种不同类型的数据到Redis数据库
        public static void InsertByBatch(IDatabase db)
        {
            IBatch batch = db.CreateBatch();
            db.SetAdd("A", "a");
            db.ListRightPush("listTest1", "shanzm");
            //db.GeoAdd("ShopsGeo1", new GeoEntry(116.34039, 39.94218, "1"));
            //连接Redis服务器把上面的所有操作，一次性执行
            //CreateBatch()、Execute()之间的操作一次性提交给服务器
            batch.Execute();
        }

    }
}
