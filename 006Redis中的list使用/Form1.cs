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

namespace _006Redis中的list使用
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadListBox();
        }


        //从Redis中获取key=list1的所有行，添加在Listbox中
        private void LoadListBox()
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                RedisValue[] values = db.ListRange("list1");
                //注意：使用ListRange（RedisKey listKey，long start，long stop）可以获取相应list中的一定范围的数据，不加范围参数则默认是查询所有的数据

                listBox1.Items.Clear();
                foreach (RedisValue item in values)
                {
                    listBox1.Items.Add(item);
                }
            }
        }

        //点击按钮实现输入的数据从左进入List
        private void btnLeftPush_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                db.ListLeftPush("list1", txtInput.Text);
            }
            LoadListBox();
        }

        //点击按钮实现输入的数据从右进入List
        private void btnRightPush_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                db.ListRightPush("list1", txtInput.Text);
            }
            LoadListBox();
        }

        //点击按钮实现从左弹出List中的数据
        private void btnLeftPop_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                string val = db.ListLeftPop("list1");//注意弹出函数在弹出的同时，返回弹出的value
                MessageBox.Show($"已经把{val}弹出");
            }
            LoadListBox();
        }

        //点击按钮实现从右弹出List中的数据
        private void btnRightPop_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                db.ListRightPop("list1");
            }
            LoadListBox();
        }
    }
}
