using ConsoleAppCAp1;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsoleAppSub1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConnection connection;
            IModel channel;
            //简单模式(Simple),工作模式(Work) 模式
            //MqCmmon.SendChannel(out connection, out channel);
            //TestSub1(channel);

            //发布订阅模式(Fanout) 出消费 (模式)
            //MqCmmon.SendChanne2(out connection, out channel);
            //TestSub11(channel);

            //路由模式
            //MqCmmon.SendChanne3(out connection, out channel);
            //TestSub111(channel);

            //主题模式
            //MqCmmon.SendChanne4(out connection, out channel);
            //TestSub1111(channel);

            //参数模式（Headers）
            MqCmmon.SendChanne5(out connection, out channel);
            TestSub11111(channel);
        }
        public static void TestSub1(IModel channel)
        {
            //设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TestSub1-消费了--{message}");

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
            //5.向队列中发送数据, 直接队列进行消费 不要全部消费掉)
            channel.BasicConsume(queue: "HelloQueue", consumer: consumer);

            var aa1 = Console.ReadKey();
        }

        public static void TestSub11(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("HelloQueue1", true, false, false, null);
            //2.将队列和交换机绑定上,第三个参数 RoutingKey Fanout 暂不用管
            channel.QueueBind("HelloQueue1", "HellowExchange", "");

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TestSub11-扇出-消费了--{message}");

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
            channel.BasicConsume(queue: "HelloQueue1", consumer: consumer);

            var aa1 = Console.ReadKey();
        }

        public static void TestSub111(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("Queue1", true, false, false, null);
            //2.将队列和交换机绑定上,
            channel.QueueBind("Queue1", "HellowExchange1", "order");

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TestSub111-order路由-消费了--{message}");

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
            channel.BasicConsume(queue: "Queue1", consumer: consumer);

            var aa1 = Console.ReadLine();
        }

        public static void TestSub1111(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("TipicQ1", true, false, false, null);
            //2.将队列和交换机绑定上,
            channel.QueueBind("TipicQ1", "HellowTopic", "order.#");

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"order.#-消费了--{message}");

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
            channel.BasicConsume(queue: "TipicQ1", consumer: consumer);

            var aa1 = Console.ReadLine();
        }

        public static void TestSub11111(IModel channel)
        {
            //1.定义队列
            channel.QueueDeclare("HeaderQ1", true, false, false, null);

            //1.1 指定参数匹配
            var headerParams = new Dictionary<string, object>
            {
                { "order", 111 },
                { "msg", 222 }
            };

            //2.将队列和交换机绑定上,
            channel.QueueBind("HeaderQ1", "HellowHeaders", string.Empty, headerParams);

            //3.设置每次读取的消息条数,这里设置的是每次读1条处理
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"(order-msg)-消费了--{message}");

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
            channel.BasicConsume(queue: "HeaderQ1", consumer: consumer);

            var aa1 = Console.ReadLine();
        }

    }
}
