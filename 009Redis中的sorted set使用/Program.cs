using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

///sorted set即zset ，是 set 的一个升级版本，它在 set 的基础上增加了一个顺序属性
///这一属性值也称分数
///这一属性在添加修改元素的时候可以指定，
///每次指定后，zset 会自动重新按新的值（分数）调整顺序。

///因为zset会自动根据分数排序，所以在实际应用于需要排序的遍历的地方，比如说：根据点赞量排名

namespace _009Redis中的sorted_set使用
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();

                //在zsetTest中添加数据，并给一个分数
                db.SortedSetAdd("zsetTest", "shanzm", 100);
                db.SortedSetAdd("zsetTest", "张三", 100);
                db.SortedSetAdd("zsetTest", "李四", 100);
                db.SortedSetAdd("zsetTest", "王五", 100);

                //给指定的元素加指定的分数
                db.SortedSetIncrement("zsetTest", "shanzm", 2);
                db.SortedSetIncrement("zsetTest", "张三", 1);

                //给指定的元素减指定的分数
                db.SortedSetDecrement("zsetTest", "李四", 1);
                db.SortedSetDecrement("zsetTest", "王五", 2);

                //若是在加减之前不存在则自动创建
                db.SortedSetIncrement("zsetTest", "单志铭", 200);



                Console.WriteLine("-------------------------------db.SortedSetRangeByRankWithScores(“zsetTest”);------------");
                //按照顺序查询处zsetTest中的所有元素
                // SortedSetEntry[] values = db.SortedSetRangeByRankWithScores("zsetTest",0,-1,Order.Descending );//默认是升序
                //返回的结果包含value和score
                SortedSetEntry[] values = db.SortedSetRangeByRankWithScores("zsetTest");

                foreach (var item in values)
                {
                    Console.WriteLine(item.Element + ":" + item.Score);
                }



                Console.WriteLine("-------------------------------db.SortedSetRangeByScore(“zsetTest”, 100, 102)------------------------------");
                //查询指定分数范围的value，返回的结果只有value,不包含分数
                RedisValue[] values1 = db.SortedSetRangeByScore("zsetTest", 100, 102);//100到102分
                foreach (var item in values1)
                {
                    Console.WriteLine(item.ToString());
                }



                Console.WriteLine("-------------------------------db.SortedSetRangeByRank(“zsetTest”, 0, -1);----------------------------");

                //查询指定行号范围的value，返回的结果只有value,不包含分数
                RedisValue[] values2 = db.SortedSetRangeByRank("zsetTest", 0, -1);//0到-1即查询出所有的数据
                foreach (var item in values2)
                {
                    Console.WriteLine(item.ToString());
                }


                Console.ReadKey();

            }
        }
    }
}
