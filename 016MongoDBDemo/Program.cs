using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

///注意项目的.net Framework版本是4.6.1
///MongoDB.Driver版本是2.5.0
///PM>Install-Package MongoDB.Driver -Version 2.5.0
///具体的使用版本是可以在package.config中查看的
///若是在安装MongoDB.Driver时使用默认的最新版会出现一些问题

///MongoDB 默认用 id 做主键，因此不用显式指定 id 是主键
///对象的Id属性在MongoDB中默认存为“_id”
///MongoDB 中没有内置“自增字段”，可以把 Id 声明为 ObjectId 类型（using MongoDB.Bson）这样插入以后就自动给字段赋值。

namespace _016MongoDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建MongoDB数据库对象，插入数据
            //InsertOneInMongoDB();


            //插入Josn类型的数据
            InsertJosnInMongoDB();

            Console.WriteLine("OK");
            Console.ReadKey();
        }

        //创建MongoDB数据库对象，插入数据
        static void InsertOneInMongoDB()
        {
            //连接MongoDB服务，创建对象
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            //获取名为：TestDb1的数据库，若是没有则创建！
            IMongoDatabase db = client.GetDatabase("TestDb1");
            //获取名为名为Personsde表（collection可以理解为表）若是没有则创建！
            IMongoCollection<Person> persons = db.GetCollection<Person>("Persons");
            IMongoCollection<Dog> dogs = db.GetCollection<Dog>("Dogs");

            Person p1 = new Person() { Id = 0001, Name = "shanzm", Age = 25 };
            Person p2 = new Person() { Id = 002, Name = "shanzm" };//MongoDB会对Age默认填充为0

            Dog d1 = new Dog() { Name = "史努比" };//注意因为Dog类的Id是ObjectId类型，所以MongoDB会自动生成一个Id值

            persons.InsertOne(p1);
            persons.InsertOne(p2);

            dogs.InsertOne(d1);
        }

        //插入Josn类型的数据
        static void InsertJosnInMongoDB()
        {
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = client.GetDatabase("TestDb1");
            IMongoCollection<BsonDocument> dogs = db.GetCollection<BsonDocument>("Dogs");

            string json = "{Name:'大黄',Age:10,Weight:50}";
            BsonDocument d1 = BsonDocument.Parse(json);
            dogs.InsertOne(d1);

        }
    }
}
