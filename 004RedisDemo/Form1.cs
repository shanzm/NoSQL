using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
                await db.StringSetAsync("Name", "张三");

                //批量写入
                KeyValuePair<RedisKey, RedisValue>[] kvs = new KeyValuePair<RedisKey, RedisValue>[3];

                kvs[0] = new KeyValuePair<RedisKey, RedisValue>("A", "a");
                kvs[1] = new KeyValuePair<RedisKey, RedisValue>("B", "b");
                kvs[2] = new KeyValuePair<RedisKey, RedisValue>("C", "c");
                await db.StringSetAsync(kvs);

                //读取数据
                string name = await db.StringGetAsync("Name");
                string A = await db.StringGetAsync("A");
                

                MessageBox.Show(name);
                MessageBox.Show(A);
            }
            MessageBox.Show("Ok");
        }
    }
}
