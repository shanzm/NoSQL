using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

///Geo 是 Redis 3.2 版本后新增的数据类型，用来保存兴趣点（POI，point of interest）的坐标信息。
///可以实现计算两 POI 之间的距离、获取一个点周边指定距离的 POI


///注意啊，我在本机安装的Redis是3.0.504版本的，
///https://github.com/microsoftarchive/redis/releases 页面有最新的pre-release版本的3.2.100
///但是我没装，所以下面代码在本机无法运行，运行会报错"ERR unknown command 'GEOADD'"
namespace _012Redis中的geo使用
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();

                //向Redis数据库中插入geo类型数据
                //下面插入的兴趣点是商店，
                //最后一个参数中的”1”、”2”是点的主键，点的名称、地址、电话等存到其他表中
                //db.GeoAdd("ShopsGeo", new GeoEntry(116.34039, 39.94218, "1"));
                //db.GeoAdd("ShopsGeo", new GeoEntry(116.340934, 39.942221, "2"));
                //db.GeoAdd("ShopsGeo", new GeoEntry(116.341082, 39.941025, "3"));
                //db.GeoAdd("ShopsGeo", new GeoEntry(116.340848, 39.937758, "4"));
                //db.GeoAdd("ShopsGeo", new GeoEntry(116.342982, 39.937325, "5"));
                //db.GeoAdd("ShopsGeo", new GeoEntry(116.340866, 39.936827, "6"));

                //上面的插入数据的方式需要连接Redis服务器多次，下面只需要一次，注意这样可以提高性能
                GeoEntry[] geos = new GeoEntry[]
                {
                    new GeoEntry (116.34039, 39.94218, "1"),
                    new GeoEntry(116.340934, 39.942221, "2"),
                    new GeoEntry(116.341082, 39.941025, "3"),
                    new GeoEntry(116.340848, 39.937758, "4"),
                    new GeoEntry(116.342982, 39.937325, "5"),
                    new GeoEntry(116.340866, 39.936827, "6")
                };


                //计算两点之间的距离
                //查询key为"ShopsGeo"中“1”点和“2”点的距离
                double? dist = db.GeoDistance("ShopsGeo", "1", "2", GeoUnit.Kilometers);

                //查询某个点周围一定半径的兴趣点
                //以“1”点为中心，半径200米中的兴趣点
                //当然也可以以某个经纬度为中心点
                //GeoRadiusResult[] items = db.GeoRadius("ShopsGeo",34.59669,119.22295, 200, GeoUnit.Meters);
                GeoRadiusResult[] items = db.GeoRadius("ShopsGeo", "1", 200, GeoUnit.Meters);
                foreach (var item in items)
                {
                    Console.WriteLine($"距离{item.Member}点，有{item.Distance}米");
                }
                Console.ReadKey();
            }
        }
    }
}
