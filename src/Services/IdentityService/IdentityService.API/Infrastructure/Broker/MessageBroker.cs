using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using IdentityService.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace IdentityService.Infrastructure.Broker
{
    public interface IMessageBroker
    {
        string CallJson(object json);
        string Call(string message);
        void Close();
    }
    
    [ExcludeFromCodeCoverage]
    public class MessageBroker: IMessageBroker
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _props;

        public MessageBroker(IOptions<AppSettings> appSettings)
        {
            var factory = new ConnectionFactory() { HostName = appSettings.Value.RabbitHost };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    _respQueue.Add(response);
                }
            };
        }

        public string CallJson(object obj)
        {
            return Call(JsonConvert.SerializeObject(obj));
        }

        public string Call(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            
            _channel.BasicPublish("","rpc_queue", _props, messageBytes);
            _channel.BasicConsume(consumer: _consumer, queue: _replyQueueName, autoAck: true);
            return _respQueue.Take();
        }

        public void Close()
        {
            _connection.Close();
        }
        
    }
}