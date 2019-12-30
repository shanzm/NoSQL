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
        }


        //从Redis中获取key=list1的所有行，添加在Listbox中
        private void LoadListBox()
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                RedisValue[] values = db.ListRange("list1");
                listBox1.Items.Clear();
                foreach (RedisValue item in values)
                {
                    listBox1.Items.Add(item);
                }
            }
        }


        private void btnLeftPush_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                db.ListLeftPush("list1", txtInput.Text);
            }
            LoadListBox();
        }

        private void btnRightPush_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                db.ListRightPush("list1", txtInput.Text);
            }
            LoadListBox();
        }

        private void btnLeftPop_Click(object sender, EventArgs e)
        {
            using (ConnectionMultiplexer conn = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {
                IDatabase db = conn.GetDatabase();
                db.ListLeftPop("list1");
            }
            LoadListBox();
        }

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
