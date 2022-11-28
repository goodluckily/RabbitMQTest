using ConsoleAppCAp1;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsoleAppSub2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConnection connection;
            IModel channel;
            //简单模式(Simple),工作模式(Work) 模式
            //MqCmmon.SendChannel(out connection, out channel);
            //TestSub2(channel

            //发布订阅模式(Fanout) 出消费 
            //MqCmmon.SendChanne2(out connection, out channel);
            //TestSub22(channel);

            //路由模式
            //MqCmmon.SendChanne3(out connection, out channel);
            //TestSub222(channel);

            //主题模式
            //MqCmmon.SendChanne4(out connection, out channel);
            //TestSub2222(channel);

            //参数模式（Headers）
            MqCmmon.SendChanne5(out connection, out channel);

            //TestSub22222(channel);//全量 All
            TestSub222222(channel);//任意 Any

        }




        public static void TestSub2(IModel channel)
        {
            //设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TestSub2-消费了--{message}");
                Thread.Sleep(1000);
                //判断是否消费成功 手动确认
                if (true)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };
            //5.向队列中发送数据, 直接队列进行消费 不要全部消费掉)
            //改为手动确认
            channel.BasicConsume(queue: "HelloQueue", autoAck: false, consumer: consumer);

            var aa2 = Console.ReadKey();
        }

        public static void TestSub22(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("HelloQueue1", true, false, false, null);
            //2.将队列和交换机绑定上,第三个参数 RoutingKey Fanout 暂不用管
            channel.QueueBind("HelloQueue2", "HellowExchange", "");

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TestSub22-扇出-消费了--{message}");

                Thread.Sleep(500);
                //判断是否消费成功
                if (true)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            //5.指定队列,进行消费信息
            channel.BasicConsume(queue: "HelloQueue2", consumer: consumer);

            var aa1 = Console.ReadKey();
        }

        public static void TestSub222(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("Queue2", true, false, false, null);
            //2.将队列和交换机绑定上,
            channel.QueueBind("Queue2", "HellowExchange1", "msg");

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TestSub222-msg路由-消费了--{message}");

                Thread.Sleep(500);
                //判断是否消费成功
                if (true)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            //5.指定队列,进行消费信息
            channel.BasicConsume(queue: "Queue2", consumer: consumer);

            var aa1 = Console.ReadLine();
        }

        public static void TestSub2222(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("TipicQ2", true, false, false, null);
            //2.将队列和交换机绑定上,
            channel.QueueBind("TipicQ2", "HellowTopic", "order.*");

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"order.*-消费了--{message}");

                Thread.Sleep(500);
                //判断是否消费成功
                if (true)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            //5.指定队列,进行消费信息
            channel.BasicConsume(queue: "TipicQ2", consumer: consumer);

            var aa1 = Console.ReadLine();
        }

        public static void TestSub22222(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("HeaderQ2", true, false, false, null);

            //1.1 指定参数匹配
            var headerParams = new Dictionary<string, object>
            {
                { "order", 111 },
            };

            //2.将队列和交换机绑定上,
            channel.QueueBind("HeaderQ2", "HellowHeaders", string.Empty, headerParams);

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"(order)-消费了--{message}");

                Thread.Sleep(500);
                //判断是否消费成功
                if (true)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            //5.指定队列,进行消费信息
            channel.BasicConsume(queue: "HeaderQ2", consumer: consumer);

            var aa1 = Console.ReadLine();
        }

        public static void TestSub222222(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("HeaderQ3", true, false, false, null);

            //1.1 指定参数匹配
            var headerParams = new Dictionary<string, object>
            {
                { "order", 111 },
                { "code", "cy" },
                { "x-match", "any" },//指定匹配模式 Any
            };

            //2.将队列和交换机绑定上,
            channel.QueueBind("HeaderQ3", "HellowHeaders", string.Empty, headerParams);

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"(order)-消费了--{message}");

                Thread.Sleep(500);
                //判断是否消费成功
                if (true)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            //5.指定队列,进行消费信息
            channel.BasicConsume(queue: "HeaderQ3", consumer: consumer);

            var aa1 = Console.ReadLine();
        }
    }
}
