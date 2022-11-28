using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace ConsoleAppCAp1
{
    internal class Program
    {
        //推荐 文章 https://mp.weixin.qq.com/s/9LhGPZDrKtW-MGWWsM2bOQ
        static void Main(string[] args)
        {
            IConnection connection;
            IModel channel;

            //发送一 简单模式(Simple),工作模式(Work) 模式
            //MqCmmon.SendChannel(out connection, out channel);
            //TestPub1(channel);

            //发送二 发布订阅模式(Fanout) 模式 (交换机)
            //MqCmmon.SendChanne2(out connection, out channel);
            //TestPub2(channel);

            //发送三 路由模式(Direct)
            //MqCmmon.SendChanne3(out connection, out channel);
            //TestPub3(channel);

            //主题模式
            //MqCmmon.SendChanne4(out connection, out channel);
            //TestPub4(channel);

            //参数模式（Headers）
            MqCmmon.SendChanne5(out connection, out channel);
            TestPub5(channel);
        }

        public static void TestPub1(IModel channel)
        {
            //发送1
            for (int i = 0; i < 20; i++)
            {
                //4,定义消息内容
                var msg = $"hellow {i}";
                var body = Encoding.UTF8.GetBytes(msg);
                //5.向队列中发送数据
                channel.BasicPublish("", "HelloQueue", null, body);

                //8.确定收到RabbitMQ服务端的确认消息
                var isOk = channel.WaitForConfirms();
                if (!isOk)
                {
                    throw new Exception("The message is not reached to the server!");
                }
                else
                {
                    Console.WriteLine($"Pub {i}");
                }
            }
        }

        public static void TestPub2(IModel channel)
        {
            //发送1
            for (int i = 0; i < 10; i++)
            {
                //4,定义消息内容
                var msg = $"hellow {i}";
                var body = Encoding.UTF8.GetBytes(msg);
                //5.向队列中发送数据
                channel.BasicPublish("HellowExchange", "", null, body);

                //8.确定收到RabbitMQ服务端的确认消息
                var isOk = channel.WaitForConfirms();
                if (!isOk)
                {
                    throw new Exception("The message is not reached to the server!");
                }
                else
                {
                    Console.WriteLine($"Pub {i}");
                }
            }
        }

        public static void TestPub3(IModel channel)
        {
            //指定路由发送消息
            while (true)
            {
                Console.WriteLine("请输入RoutingKey:");
                //4,定义消息内容
                string routingKey = Console.ReadLine();

                var msg = $"hellow 请输入RoutingKey: {routingKey}";
                var body = Encoding.UTF8.GetBytes(msg);
                //5.向队列中发送数据 路由交换机模式
                channel.BasicPublish("HellowExchange1", routingKey, null, body);
                //8.确定收到RabbitMQ服务端的确认消息
                var isOk = channel.WaitForConfirms();
                if (!isOk)
                {
                    throw new Exception("The message is not reached to the server!");
                }
                else
                {
                    Console.WriteLine($"Pub {routingKey} OK ");
                }
            }
        }

        public static void TestPub4(IModel channel)
        {
            //指定路由发送消息
            while (true)
            {
                Console.WriteLine("请输入RoutingKey:");
                //4,定义消息内容
                string routingKey = Console.ReadLine();

                var msg = $"hellow 请输入RoutingKey: {routingKey}";
                var body = Encoding.UTF8.GetBytes(msg);
                //5.向队列中发送数据 路由交换机模式
                channel.BasicPublish("HellowTopic", routingKey, null, body);
                //8.确定收到RabbitMQ服务端的确认消息
                var isOk = channel.WaitForConfirms();
                if (!isOk)
                {
                    throw new Exception("The message is not reached to the server!");
                }
                else
                {
                    Console.WriteLine($"Pub {routingKey} OK ");
                }
            }
        }

        public static void TestPub5(IModel channel)
        {
            //设置参数 这里匹配两种参数 后续进行选择发送

            //默认是全量All 匹配

            var propertiesl = channel.CreateBasicProperties();
            var headerParamsl = new Dictionary<string, object>
            {
                { "order", "111" },
                { "msg", "222" }
            };
            propertiesl.Headers = headerParamsl;

            var properties2 = channel.CreateBasicProperties();
            var headerParams2 = new Dictionary<string, object>
            {
                { "order", "111" }
            };
            properties2.Headers = headerParams2;

            //指定路由发送消息
            while (true)
            {
                Console.WriteLine("请输入sendType: 1(order-msg) 2(order)");
                //4,定义消息内容
                string sendType = Console.ReadLine();

                var msg = $"hellow Headers: {sendType}";
                var body = Encoding.UTF8.GetBytes(msg);
                if (sendType == "1")
                {
                    //5.向队列中发送数据 路由交换机模式
                    channel.BasicPublish("HellowHeaders", string.Empty, propertiesl, body);
                }
                else if (sendType == "2")
                {
                    //5.向队列中发送数据 路由交换机模式
                    channel.BasicPublish("HellowHeaders", string.Empty, properties2, body);
                }
                
                //8.确定收到RabbitMQ服务端的确认消息
                var isOk = channel.WaitForConfirms();
                if (!isOk)
                {
                    throw new Exception("The message is not reached to the server!");
                }
                else
                {
                    Console.WriteLine($"Pub {sendType} OK ");
                }
            }
        }
    }
}
