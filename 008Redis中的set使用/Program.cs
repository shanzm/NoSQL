using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;


///set 是集合，他是 string 类型的无序集合。Set 是通过 hash table 实现的，对集 、交集、差集。
///通过这些操作我们可以实现社交网站中的好友推荐和 blog 的 tag 功能。
///集合不允许有重复值。

///注意 set 不是按照插入顺序遍历的，而是按照自己的一个存储方式来遍历，因为没有保存插入的顺序。

///Set因为其保存的数据是无重复值的，所以可以用来保存某些id,比如论坛中关入小黑屋的用户id


namespace _008Redis中的set使用
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();

                //添加数据到集合"setTest"
                db.SetAdd("setTest", 12);
                db.SetAdd("setTest", 13);
                db.SetAdd("setTest", 14);
                db.SetAdd("setTest", 15);
                db.SetAdd("setTest", 16);
                db.SetAdd("setTest", 17);
                db.SetAdd("setTest", 17);//你要注意，即使你插入17两次，但是因为Set中的数据是不重复的，所以数据库中的17只有一个
                                         //整体setTest的长度还是6
                                         //注意数据中set中存储的顺序和你插入的顺序无关（也正是因此，才有的sorted set类型）
                Console.WriteLine("插入数据到setTest集合中成功");


                //集合的长度
                long length = db.SetLength("setTest");
                Console.WriteLine($"集合setTest的长度是：{length}");

                //判断集合“setTest”中是否存在14
                if (db.SetContains("setTest", 14))
                {
                    Console.WriteLine("集合setTest中包含14");
                }


                //查询集合中的的所有元素
                RedisValue[] values = db.SetMembers("setTest");
                foreach (var item in values)
                {
                    Console.WriteLine(item.ToString());
                }

                //删除指定的元素
                db.SetRemove("setTest", 12);
                if (!db.SetContains("setTest", 12))
                {
                    Console.WriteLine("已删除12");
                }

                Console.ReadKey();
            }
        }
    }
}
