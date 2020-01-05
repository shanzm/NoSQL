using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace _015Redis实现随机分红包
{
    class Program
    {
        static void Main(string[] args)
        {
            //抢红包的人
            string [] values = { "张三", "李四", "王五" };

            //10元红包分给3个人
            decimal[] arrayMoney = allotMoney(10, values.Length);

            decimal[] result = arrayMoney.Select(n => n / 100).ToArray();//将分转换为元
            Console.WriteLine(string.Join(" ", result));
            Console.WriteLine($"红包总金额{result.Aggregate((total, next) => total + next)}元");//验证一下总金额
            Console.WriteLine($"红包总金额{result.ToArray <decimal>().Sum()}元");//验证一下总金额
            
            //把红包push到Redis中的list中，强红包则一个一个pop出来
            using (ConnectionMultiplexer redis=ConnectionMultiplexer .Connect("127.0.0.1:6379"))
            {
                IDatabase db = redis.GetDatabase();
                RedisValue[] value = new RedisValue[result.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    value[i] = result[i].ToString();
                }
                db.ListRightPush("RedEnvelope",value);
            }

            Console.WriteLine("OK");
            Console.ReadKey();
        }


        /// <summary>
        /// 红包金额分配
        /// </summary>
        /// <param name="totalMoney">红包总金额</param>
        /// <param name="totalPerson">分红包的人数</param>
        /// <returns></returns>
        static decimal[] allotMoney(decimal totalMoney, int totalPerson)
        {
            totalMoney = totalMoney * 100;//将元转换为分，方便计算:10元变为1000分

            int avgM = Convert.ToInt16(totalMoney / totalPerson);//平均333分
            decimal loss = totalMoney - (avgM * totalPerson);//精度损耗1分
            Random random = new Random();
            decimal[] arrayMoney = new decimal[totalPerson];

            //先给每个人一个平均金额
            for (int i = 0; i < totalPerson; i++)
            {
                arrayMoney[i] = avgM;
            }

            //若是有损耗，则将损耗随机给某个人
            if (loss > 0)
            {
                arrayMoney[random.Next(1, totalPerson)] += loss;
            }

            //随机生成两个索引，第一个索引指向的对象给第二个索引指向的对象一个随机金额（该金额为随机生成一个小于二分之一平均金额的数额）
            //此过程循环totalPerosn次
            for (int i = 0; i < totalPerson; i++)
            {
                int indexP1 = random.Next(1, totalPerson);
                int indexP2 = random.Next(1, totalPerson);

                decimal randomMon = random.Next(0, Convert.ToInt16(avgM / 2));//随机生成一个小于二分之一平均金额的数额(可以自己设置)
                arrayMoney[indexP1] -= randomMon;
                arrayMoney[indexP2] += randomMon;
            }


            if (arrayMoney.Sum() == totalMoney)
            {
                return arrayMoney;
            }
            else
            {
                Exception e = new Exception("金额分配错误");
                throw e;
            }
        }
    }
}
