using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Console;


//注意MySQL查询时时忽视数据的大小写的，
//比如说数据库中存储的某个用户名是"ADMIN",那么用户输入"admin"也是可以的
//值得注意的是MongoDB不是忽视大小写的

namespace _017DataRetrieveFromMongoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = client.GetDatabase("TestDb1");

            //往TestDb1数据库中的Persons Collection中批量插入一些实验数据
            //InsertMany(db);

            //查询
            //RetrieveFromMongoDB(db);

            //异步查询
            //RetrieveFromMongoDBAsync(db);

            //异步查询（查询的结果数据量不大）
            //RetrieveFromMongoDBAsync2(db);

            //分页查询
            PagedQuery(db);

            WriteLine("OK");
            ReadKey();
        }

        //批量查询实验数据
        static void InsertMany(IMongoDatabase db)
        {
            IMongoCollection<Person> persons = db.GetCollection<Person>("Persons");

            List<Person> listPerson = new List<Person>()
            {
                new Person () {Name ="张三",Age =25,Height =180 } ,
                new Person () {Name ="李四",Age =25,Height =180 },
                new Person() {Name ="王五",Age =26,Height =181 },
                new Person() {Name ="赵六", Age =27,Height =182 }
            };

            persons.InsertMany(listPerson);
        }

        //查询
        static void RetrieveFromMongoDB(IMongoDatabase db)
        {
            IMongoCollection<Person> persons = db.GetCollection<Person>("Persons");

            //构造查询条件
            var filter1 = Builders<Person>.Filter.Gt(p => p.Age, 25);//Person.Age>25,注意Gt即great than
            var filter2 = Builders<Person>.Filter.Gte(p => p.Age, 25);//Person.Age>=25,注意Gte即great than or equal
            var filter3 = Builders<Person>.Filter.Lt(p => p.Age, 25);//Person.Age<25,注意Lt即less than
            var filter4 = Builders<Person>.Filter.Lte(p => p.Age, 25);//Person.Age=<25,注意Lt即less than or equal
            var filter5 = Builders<Person>.Filter.Eq(p => p.Age, 25);//Person.Age=25,注意Eq即 equal
            var filter6 = Builders<Person>.Filter.Ne(p => p.Age, 25);//Person.Age!=25,注意Ne即 not equal
            var filter7 = Builders<Person>.Filter.Or(filter1, filter5);//过滤条件满足filter1或者是filter5，同样支持其他的and和not的逻辑运算
            var filter8 = Builders<Person>.Filter.Regex(p => p.Name, "^张.$");//即匹配姓“张”的人，第一个参数是返回值是Person对象属性名的委托，第二个参数是一个字符串，为正则表达式

            //最常用的还是我们自己使用where(),自己写lambda筛选条件
            var filter9 = Builders<Person>.Filter.Where(p => (p.Age > 25) && (p.Height > 180));//年龄大于25，身高大于180


            //todo:自己看where()定义,即可查看所有的Filter方法

            var result = persons.Find(filter9);

            Array.ForEach(result.ToList().ToArray(), n => Console.WriteLine($"姓名：{n.Name},年龄：{n.Age },身高：{n.Height}"));
        }

        //异步查询
        static void RetrieveFromMongoDBAsync(IMongoDatabase db)
        {
            IMongoCollection<Person> persons = db.GetCollection<Person>("Persons");

            var filter = Builders<Person>.Filter.Where(p => p.Name == "张三");

            using (IAsyncCursor<Person> result = persons.FindAsync(filter).Result)//注意IAsyncCursor<T>是实现了IDispost接口的
            {
                while (result.MoveNextAsync().Result)//注意和SQL中一样，这是一个指针，但是在MongoDB中指针每次指向一组数据（而不是查询出来的一条）
                                                     //为什么是一组数据，因为MongoDB不会一次性把所有的数据都读取，而是分组分批读取
                {
                    var groupPerosn = result.Current;//获取当前组（当前指针指向的那一组）的数据
                    Array.ForEach(groupPerosn.ToArray(), p => Console.WriteLine($"{p.Name},{p.Age }"));
                }
            }


        }


        //异步查询（查询的结果数据量不大）
        static void RetrieveFromMongoDBAsync2(IMongoDatabase db)
        {
            IMongoCollection<Person> persons = db.GetCollection<Person>("Persons");

            var filter = Builders<Person>.Filter.Where(p => p.Name == "张三");

            using (IAsyncCursor<Person> result = persons.FindAsync(filter).Result)//注意IAsyncCursor<T>是实现了IDispost接口的
            {
                //while (result.MoveNextAsync().Result)//注意和SQL一样，这是一个指针，但是在MongoDB中指针每次指向一组数据（而不是查询出来的一条）
                //{
                //    var groupPerosn = result.Current;//获取当前组（当前指针指向的那一组）的数据
                //    Array.ForEach(groupPerosn.ToArray(), p => Console.WriteLine($"{p.Name},{p.Age }"));
                //}
                List<Person> listResult = result.ToList<Person>();
                Array.ForEach(listResult.ToArray(), p => Console.WriteLine($"{p.Name},{p.Age }"));
            }


        }

        //分页查询
        static void PagedQuery(IMongoDatabase db)
        {
            IMongoCollection<Person> persons = db.GetCollection<Person>("Persons");

            FindOptions<Person, Person> findOpt = new FindOptions<Person, Person>();
            findOpt.Skip = 2;//跳过两个
            findOpt.Limit = 4;//取4个
            findOpt.Sort = Builders<Person>.Sort.Descending(p => p.Height).Ascending(p => p.Age);//先按照身高降序，再按照年龄升序,
                                                                                                 //注意这个排序是在查询前对整个表操作，而不是查询得出结果后在对结果排序
            var filter = Builders<Person>.Filter.Where(p => p.Age > 0);

            using (var personsCursor = persons.FindAsync(filter, findOpt).Result)
            {
                List<Person> listPeron = personsCursor.ToList();
                Array.ForEach(listPeron.ToArray(), p => Console.WriteLine($"{p.Name},{p.Age },{p.Height}"));
            }
        }
    }
}
;