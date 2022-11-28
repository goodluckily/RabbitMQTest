using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCAp1
{
    public static class MqCmmon
    {
        public static void SendChannel(out IConnection connection, out IModel channel)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.178",
                UserName = "cy",
                Password = "cy123123"
            };
            //设置断线自动恢复
            factory.AutomaticRecoveryEnabled = true;
            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            //3.定义队列
            channel.QueueDeclare("HelloQueue", false, false, false, null);

            //4.启动消息发送确认机制，即需要收到RabbitMQ服务端的确认消息
            channel.ConfirmSelect();

            //5.设置消息持久化
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            //properties.MessageId = Guid.NewGuid().ToString("N");
            //properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }


        public static void SendChanne2(out IConnection connection, out IModel channel)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.178",
                UserName = "cy",
                Password = "cy123123"
            };
            //设置断线自动恢复
            factory.AutomaticRecoveryEnabled = true;
            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            //3.定义交换机,指定名字和类型
            channel.ExchangeDeclare("HellowExchange", ExchangeType.Fanout, true);

            //4.启动消息发送确认机制，即需要收到RabbitMQ服务端的确认消息
            channel.ConfirmSelect();

            //5.设置消息持久化
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            properties.MessageId = Guid.NewGuid().ToString("N");
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }


        public static void SendChanne3(out IConnection connection, out IModel channel)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.178",
                UserName = "cy",
                Password = "cy123123"
            };
            //设置断线自动恢复
            factory.AutomaticRecoveryEnabled = true;
            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            //3.定义交换机,指定名字和类型
            channel.ExchangeDeclare("HellowExchange1", ExchangeType.Direct, true);

            //4.启动消息发送确认机制，即需要收到RabbitMQ服务端的确认消息
            channel.ConfirmSelect();

            //5.设置消息持久化
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            properties.MessageId = Guid.NewGuid().ToString("N");
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }


        public static void SendChanne4(out IConnection connection, out IModel channel)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.178",
                UserName = "cy",
                Password = "cy123123"
            };
            //设置断线自动恢复
            factory.AutomaticRecoveryEnabled = true;
            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            //3.定义交换机,指定名字和类型
            channel.ExchangeDeclare("HellowTopic", ExchangeType.Topic, true);

            //4.启动消息发送确认机制，即需要收到RabbitMQ服务端的确认消息
            channel.ConfirmSelect();

            //5.设置消息持久化
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            properties.MessageId = Guid.NewGuid().ToString("N");
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public static void SendChanne5(out IConnection connection, out IModel channel)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.178",
                UserName = "cy",
                Password = "cy123123"
            };
            //设置断线自动恢复
            factory.AutomaticRecoveryEnabled = true;
            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            //3.定义交换机,指定名字和类型
            channel.ExchangeDeclare("HellowHeaders", ExchangeType.Headers, true);

            //4.启动消息发送确认机制，即需要收到RabbitMQ服务端的确认消息
            channel.ConfirmSelect();

            //5.设置消息持久化
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            properties.MessageId = Guid.NewGuid().ToString("N");
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
    }
}
