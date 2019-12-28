using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _004RedisDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //注意此处我们使用异步方法，我们也是可以使用非异步方法ConnectionMultiplexer.Connect("127.0.0.1:6379")
            //尽量使用异步
            using (ConnectionMultiplexer conn = await ConnectionMultiplexer.ConnectAsync("127.0.0.1:6379"))
            {
                //默认是0号数据库，若是其他数据库，如3号数据库，conn.GetDatabase(3)
                IDatabase db = conn.GetDatabase();
                #region 说明
                ///注意看，所有的Key参数是RedisKey类型，所有的value参数是RedisValue类型
                ///这其中是运算符的重载，RedisKey和Redisvalue可以和string隐式转换
                #endregion

                //写入数据
                await db.StringSetAsync("Name", "张三", TimeSpan.FromSeconds(10));

                //批量写入
                KeyValuePair<RedisKey, RedisValue>[] kvs = new KeyValuePair<RedisKey, RedisValue>[3];

                kvs[0] = new KeyValuePair<RedisKey, RedisValue>("A", "a");
                kvs[1] = new KeyValuePair<RedisKey, RedisValue>("B", "b");
                kvs[2] = new KeyValuePair<RedisKey, RedisValue>("C", "c");
                await db.StringSetAsync(kvs);

                //读取数据(查询不到数据返回为null)
                string name = await db.StringGetAsync("Name");
                string A = await db.StringGetAsync("A");

                MessageBox.Show(name);
                MessageBox.Show(A);

                //删除数据
                db.KeyDelete("A");

                //判断是否存在某条数据
                //不建议开发中这么使用，会有并发问题
                //虽然Redis服务器是单线程的，
                //但是程序在运行的时候，有可能在你查询是否存在之后，有删除程序，之后又有根据你之前查询存在后查询
                if (!db.KeyExists("A"))
                {
                    MessageBox.Show("已删除Key值为‘A’的数据");
                }


                //对已经存储的数据设置过期时间
                db.KeyExpire("B", TimeSpan.FromSeconds(10));
               
            }
            MessageBox.Show("Ok");
        }
    }
}
