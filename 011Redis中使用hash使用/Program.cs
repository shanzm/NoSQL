using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///Hashes 有经过记忆体优化，效能是比较高的
///不用像string 会lock 整个entity ，可针对单一属性更新

///hash 特别适合用于存储对象，相较于将对象的每个字段存成单个 string 类型，
///将一个对象存储在 hash 类型中会占用更少的内存，并且可以更方便的存取整个对象。


namespace _011Redis中使用hash使用
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p1 = new Person() { Id = 0001, Name = "shanzm", Age = 25, Score = 100 };
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                HashEntry[] p = new HashEntry[]
                {
                   new HashEntry ("Id",p1.Id ),
                   new HashEntry ("Name",p1.Name ),
                   new HashEntry ("Age",p1.Age ),
                   new HashEntry ("Score",p1.Score)
               };
                db.HashSet("p1", p);

                var p1Name = db.HashGet("p1", "Name");//第一个参数是hash的key,第二个参数是hashEntry中的Name元素
                Console.WriteLine(p1Name.ToString());
                Console.ReadKey();
            }
        }
    }
}
