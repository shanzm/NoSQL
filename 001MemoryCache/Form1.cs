using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _001MemoryCache
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //添加引用：System.Runtime.Caching
            //新建一个缓存对象，使用默认的缓存对象
            //MemoryCache是存入到程序进程的内存中的，程序重启之后就没了
            MemoryCache memCache = MemoryCache.Default;
            //缓存以键值对的形式存储，缓存的生命期是10s
            memCache.Add("name", "shanzm", DateTimeOffset.Now.AddSeconds(10));


            //在Asp.net中的HttpContext.Cache就是对MemoryCache的封装
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MemoryCache memCache = MemoryCache.Default;
            //string mem = memCache["name"].ToString();
            string name = (string)memCache["name"];
            if (name == null)
            {
                MessageBox.Show("无缓存");
            }
            else
            {
                MessageBox.Show(name);
            }
        }
    }
}
