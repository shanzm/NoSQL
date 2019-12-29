using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _005使用Redis计算新闻点击量.Models
{
    public class New
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public long ClickCount { get; set; }
    }
}