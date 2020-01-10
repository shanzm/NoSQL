## 基于dotnet ,使用几种常见的NoSQL数据库 {ignore=true}

update :2020年1月10日 10:25:41-shanzm

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

* MemoryCache对象
* HttpContext.Cache (Asp.net)对象


示例：（详见：[001MemoryCache](https://github.com/shanzm/NoSQL/tree/master/001MemoryCache)）

```cs
//MemoryCache是存入到程序进程的内存中的，程序重启之后就没了
//添加引用：System.Runtime.Caching
//新建一个缓存对象，使用默认的缓存对象
MemoryCache memCache = MemoryCache.Default;
//缓存以键值对的形式存储，缓存的生命期是10s
memCache.Add("name", "shanzm", DateTimeOffset.Now.AddSeconds(10));
```

---

### 1.MemCached

* 安装包下载：[推荐一个编译好的安装包](http://static.runoob.com/download/memcached-win32-1.4.4-14.zip)

* NuGet:PM> Install-Package EnyimMemcached

* MemCached中的Cas操作：（详见：[003Cas操作](https://github.com/shanzm/NoSQL/tree/master/003Cas%E6%93%8D%E4%BD%9C)）

示例：（详见：[002MemCachedDemo](https://github.com/shanzm/NoSQL/tree/master/002MemCachedDemo)）

```cs 
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

---

### 2.Redis

* 安装Redis（官方为windows版，[微软自己维护一个开源版本](https://github.com/microsoftarchive/redis/releases)）

* 常用的命令：[参考](https://www.w3cschool.cn/redis_all_about/redis_all_about-pf4826ua.html)

* 安装Redis GUI客户端：RedisDesktopManager （[推荐一个cracked 2019.5版本](https://www.lanzous.com/i7jtkkf)）

* NuGet:PM>Install-Package StackExchange.Redist（注意其所支持的dotnet版本）[官方地址](https://github.com/StackExchange/StackExchange.Redis)

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
<br>




示例：(详见：[004RedisDemo](https://github.com/shanzm/NoSQL/tree/master/004RedisDemo))

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
---

### 3.MongoDB

* 安装MongoDB（[官方地址](https://www.mongodb.com/download-center/community)）

* 安装MongoDB GUI客户端：[Robo3T](https://robomongo.org/download

* NuGet:PM>Install-Package MongoDB.Driver -Version 2.5.0(注意默认安装最新版本可能会报错
亲测2.5.0版本和 .net Framework版本是4.6.1完美支持)

* MongoDB中的完整的增删改查，见：[017MongoDB中的CURD](https://github.com/shanzm/NoSQL/tree/master/017MongoDB%E4%B8%AD%E7%9A%84CURD)
<br>

示例：（详见：016MongoDBDemo）

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