## 基于dotnet ,使用几种常见的NoSQL数据库 {ignore=true}

update :2020年1月13日 22:58:19-shanzm

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->
* 目录：
<!-- code_chunk_output -->

- [0 .net中的缓存对象](#0-net中的缓存对象)
- [1.MemCached](#1memcached)
- [2.Redis](#2redis)
- [3.MongoDB](#3mongodb)

<!-- /code_chunk_output -->
- [示例完整源码地址](https://github.com/shanzm/NoSQL)

<hr style="height:8px;border:none;border-top:5px double black;" />

  
### 0 .net中的缓存对象

* 因为需要向服务器多次请求相同的数据，为了减轻服务器压力，所以引入缓存。

* 在.net中的缓存类为`MemoryCache`（.net 4.0引入），其继承于`ObjectCache`抽象类，并且实现了`IEnumerable`和`IDisposable`接口。和ASP.NET中的`HttpContext.Cache`对象具有相同的功能！但是MemoryCache更加通用，也是可以使用在ASP.NET中的。

* MemoryCache对象的数据形式为键值对形式

* MemoryCache对象可以设置缓存的时间

* MemoryCache是存入到程序进程的内存中的，程序重启之后就没了

* 项目若是使用的是服务器集群，那么因为程序请求不同的服务器，缓存在各个服务器中不能共享，所以数据要在不同的服务器中缓存，所以需要占用大量的内存，此时使用程序内缓存就不合适了
所以，如果数据量比较大或者集群服务器比较多，就要用单独的分布式缓存了，也就是搞一台或者多台专门服务器保存缓存数据，所有服务器都访问分布式缓存服务器。

示例：（详见：[001MemoryCache](https://github.com/shanzm/NoSQL/tree/master/001MemoryCache)）

```cs

//添加引用：System.Runtime.Caching
//新建一个缓存对象，使用默认的缓存对象
MemoryCache memCache = MemoryCache.Default;
//缓存以键值对的形式存储，缓存的生命期是10s
memCache.Add("name", "shanzm", DateTimeOffset.Now.AddSeconds(10));
```
<br>
<hr style="height:8px;border:none;border-top:4px double black;" />

### 1.MemCached

* Memcached是一个自由开源的，高性能，分布式内存对象缓存系统。

* 使用Memcached的目的：通过缓存数据库查询结果，减少数据库访问次数，以提高动态Web应用的速度、提高可扩展性。

* MemCached存储的数据形式是键值对的形式，可以简单的理解为一个大`Dictionary<key ,value>`

* MemCached存储的数据在服务器的内存中，读取效率较高，但是客户端和服务器之间的通讯是通过网络通讯，所以效率不及使用.net中的缓存对象缓存

* 当然也是因为MemCached存储的数据写在内存中，所以服务器重启之后则数据全部即被清空。

* 官网上并未提供 Memcached 的 Windows 平台安装包，需要自行编译。安装包下载：[推荐一个编译好的安装包](http://static.runoob.com/download/memcached-win32-1.4.4-14.zip)

* 注意ADO.Net只支持关系型数据库，所以在.net中使用NoSQL数据库需要自行安装驱动。

* 注意.net下的MemCached驱动有很多，推荐使用EnyimMemcached
NuGet:`PM> Install-Package EnyimMemcached`

* MemCached中的Cas操作：（详见：[003Cas操作](https://github.com/shanzm/NoSQL/tree/master/003Cas%E6%93%8D%E4%BD%9C)）

简单示例：（详见：[002MemCachedDemo](https://github.com/shanzm/NoSQL/tree/master/002MemCachedDemo)）

```cs 
//创建配置对象
MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
memConfig.AddServer("127.0.0.1:11211");
//若是Memcached集群，此处可以同时添加多个服务器ip地址，然后客户端根据自己的算法决定把数据写入哪个 Memcached 实例

//创建MemcachedClient对象
using (MemcachedClient memClient = new MemcachedClient(memConfig))
{
    //写入MemCached中
    memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "Name", "shanzm");
    memClient.Store(Enyim.Caching.Memcached.StoreMode.Set, "Age", "100");

    //读取数据
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
```
> * 关于MemCached集群：
>   * memcached 重启之后短时间内大量的请求会涌入数据库，给数据库造成压力，解决这个的方法就是使用集群，有多台 Memcached 服务器提供服务
>   * Memcached 服务器的“雪崩”问题：如果所有缓存设置过期时间一样，那么每隔一段时间就会造成一次数据库访问的高峰。解决方法：缓存时间设置不一样，比如加上一个随机数。
>   * Memcached 的集群实现很简单，集群节点直接不进行通讯、同步，只要在多个服务器上启动多个Memcached 服务器即可，客户端决定把数据写入不同的实例，不搞主从复制，每个数据库实例保存一部分内容

<br>
<hr style="height:8px;border:none;border-top:4px double black;" />

### 2.Redis

* Redis是完全开源免费的，是一个高性能的key-value数据库

* 相对于MemCached等NoSQL数据库其有如下特点：
  * Redis支持数据的持久化，可以将内存中的数据保存在磁盘中，重启的时候可以再次加载进行使用。
  * Redis 中value数据类型有6种，同字符串(**String**), 哈希(**Hash**), 列表(**list**), 集合(**set**) 和 有序集合(**sorted set**)，经纬度(**geo**，仅限Redis3.2以上版本)
  * Redis支持数据的备份，即master-slave模式的数据备份。

* Redis的所有操作都是原子性的( Atomicity)，即和SQL中的事务一样：要么成功执行要么失败完全不执行。单个操作是原子性的。多个操作也支持事务，即原子性，通过MULTI和EXEC指令包起来。

* Redis与MemCached对比：

   * Redis是单线程的，因此单个Redis实例只能使用一个CPU核，无法发挥CPU的所有性能，亦是如此，所以一台服务器上可以运行多个Redis实例，不同实例监听不同的端口，再组成集群。对比于MemCached，MemCached是多线程的，可以充分的利用CPU多核的性能
   * MemCached保存的键值对的value只能是字符串类型，对象类型只能序列化为Json字符串
   * Redis会把数据写入磁盘，而Memcached只写入内存，重启即清空。
   *  **简而言之：Memcached 只能当缓存服务器用，也是最合适的；Redis 不仅可以做缓存服务器（性能没有 Memcached 好），还可以存储业务数据。**

* 虽然Redis是默认有16个数据库，但是因为单线程的原因，不同的项目使用同一个Redis实例的不同库，效率不高，所以一般一个项目使用一个Redis实例，使用默认的db0库，即该实例的第一个库（索引为0）

* 注意：Redis和MemCached一样，所有的键值对数据都是存放在一个库中（即数据不隔离），不同项目，不同程序，只要是使用的是同一个服务器，则他们的数据都存放在一个库中，所以存放数据的时候要注意`key`的命名方式，防止重复，造成对已存入的数据的覆盖。

* 安装Redis（官方无windows版，[微软自己维护一个开源版本](https://github.com/microsoftarchive/redis/releases)）

* 常用的命令：[参考](https://www.w3cschool.cn/redis_all_about/redis_all_about-pf4826ua.html)


* 安装Redis GUI客户端：RedisDesktopManager （[推荐一个cracked 2019.5版本](https://www.lanzous.com/i7jtkkf)）

* Redis在.net下的驱动也多个，ServiceStack.Redis 和StackExchange.Redis(推荐)
NuGet:`PM>Install-Package StackExchange.Redist`（注意其所支持的dotnet版本）
[官方地址](https://github.com/StackExchange/StackExchange.Redis)

* Redis中的六种数据类型的具体操作及使用案例

  [005使用Redis计算新闻点击量](https://github.com/shanzm/NoSQL/tree/master/005%E4%BD%BF%E7%94%A8Redis%E8%AE%A1%E7%AE%97%E6%96%B0%E9%97%BB%E7%82%B9%E5%87%BB%E9%87%8F)

  [006Redis中的list使用](https://github.com/shanzm/NoSQL/tree/master/006Redis%E4%B8%AD%E7%9A%84list%E4%BD%BF%E7%94%A8)

  [007模拟注册发送邮件验证](https://github.com/shanzm/NoSQL/tree/master/007%E6%A8%A1%E6%8B%9F%E6%B3%A8%E5%86%8C%E5%8F%91%E9%80%81%E9%82%AE%E4%BB%B6%E9%AA%8C%E8%AF%81)

  [008Redis中的set使用](https://github.com/shanzm/NoSQL/tree/master/008Redis%E4%B8%AD%E7%9A%84set%E4%BD%BF%E7%94%A8)

  [009Redis中的sorted set使用](https://github.com/shanzm/NoSQL/tree/master/009Redis%E4%B8%AD%E7%9A%84sorted%20set%E4%BD%BF%E7%94%A8)

  [010Redis中使用sorted set实现热搜](https://github.com/shanzm/NoSQL/tree/master/010Redis%E4%B8%AD%E4%BD%BF%E7%94%A8sorted%20set%E5%AE%9E%E7%8E%B0%E7%83%AD%E6%90%9C)

  [011Redis中使用hash使用](https://github.com/shanzm/NoSQL/tree/master/011Redis%E4%B8%AD%E4%BD%BF%E7%94%A8hash%E4%BD%BF%E7%94%A8)

  [012Redis中的geo使用](https://github.com/shanzm/NoSQL/tree/master/012Redis%E4%B8%AD%E7%9A%84geo%E4%BD%BF%E7%94%A8)(注意只支持最新版本的Redis)
  
  [015Redis实现随机分红包](https://github.com/shanzm/NoSQL/tree/master/015Redis%E5%AE%9E%E7%8E%B0%E9%9A%8F%E6%9C%BA%E5%88%86%E7%BA%A2%E5%8C%85)




简单示例：(详见：[004RedisDemo](https://github.com/shanzm/NoSQL/tree/master/004RedisDemo))

```cs
//注意此处我们使用异步方法
using (ConnectionMultiplexer conn = await ConnectionMultiplexer.ConnectAsync("127.0.0.1:6379"))
{
    //默认是0号数据库，若是其他数据库，如3号数据库，conn.GetDatabase(3)
    IDatabase db = conn.GetDatabase();

    //写入数据
    await db.StringSetAsync("Name", "张三", TimeSpan.FromSeconds(10));

    //批量写入(使用Redis中Batch对象 见013Redis的批量操作)
    KeyValuePair<RedisKey, RedisValue>[] kvs = new KeyValuePair<RedisKey, RedisValue>[3];

    kvs[0] = new KeyValuePair<RedisKey, RedisValue>("A", "a");
    kvs[1] = new KeyValuePair<RedisKey, RedisValue>("B", "b");
    kvs[2] = new KeyValuePair<RedisKey, RedisValue>("C", "c");
    await db.StringSetAsync(kvs);

    //读取数据(查询不到数据返回为null)
    string name = await db.StringGetAsync("Name");
    string A = await db.StringGetAsync("A");

    //删除数据
    db.KeyDelete("A");

    //判断是否存在某条数据
    if (!db.KeyExists("A"))
    {
        MessageBox.Show("已删除Key值为‘A’的数据");
    }

    //对已经存储的数据设置过期时间
    db.KeyExpire("B", TimeSpan.FromSeconds(10));
}
```
<br>
<hr style="height:8px;border:none;border-top:4px double black;" />

### 3.MongoDB

* MongoDB 是一个面向文档存储的数据库，即文档型数据库。存储方式也是key-value，其中value只能是“文档”，在MongoDB中的文档类似于Json对象。（其中的数据以"filed ：value"书写）

* 因为数据是和Json类似的字符串（其实我感觉就是Json字符串），所以不需要预先定义表结构，同一个“表”中可以保存多个格式的数据。

* Mongodb 没有“数据一致性检查”、“事务”等，不适合存储对数据事务要求高（比如金融）的数据；只适合放非关键性数据（比如日志或者缓存）。

* 注意MongoDB中我们所说的表即其中的Collection

* 安装MongoDB（[官方地址](https://www.mongodb.com/download-center/community)）

* 安装MongoDB GUI客户端：[Robo3T](https://robomongo.org/download)

* 注意.net中的MongoDB的驱动，有微软官方开发：
 NuGet:`PM>Install-Package MongoDB.Driver -Version 2.5.0`
(注意默认安装最新版本可能会报错
亲测2.5.0版本和 .net Framework版本是4.6.1完美支持)

* MongoDB中的完整的增删改查，见：[017MongoDB中的CURD](https://github.com/shanzm/NoSQL/tree/master/017MongoDB%E4%B8%AD%E7%9A%84CURD)

简单示例：（详见：[016MongoDBDemo](https://github.com/shanzm/NoSQL/tree/master/016MongoDBDemo)）

```cs
//连接MongoDB服务，创建对象
MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
//获取名为：TestDb1的数据库，若是没有则创建！
IMongoDatabase db = client.GetDatabase("TestDb1");
//获取名为名为Personsde表（collection可以理解为表）若是没有则创建！
IMongoCollection<Person> persons = db.GetCollection<Person>("Persons");


Person p1 = new Person() { Id = 0001, Name = "shanzm", Age = 25 };
Person p2 = new Person() { Id = 002, Name = "shanzm" };//MongoDB会对Age默认填充为0

persons.InsertOne(p1);
persons.InsertOne(p2);

```