using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FeedService.Infrastructure.CQRS;
using FlueShared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FeedService.Infrastructure.Broker
{
    internal interface IRabbitListenerService
    {
        void DoWork(IServiceProvider services);
    }

    [ExcludeFromCodeCoverage]
    internal class RabbitListenerService : IRabbitListenerService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; }
        private IModel Channel { get; }

        public RabbitListenerService(ILogger<RabbitListenerService> logger, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Factory = new ConnectionFactory() {HostName = _appSettings.RabbitHost};
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
        }

        public void DoWork(IServiceProvider services)
        {
            _logger.LogInformation("Scoped Processing Service is working.");
            
            Channel.QueueDeclare(queue: "rpc_queue", durable: false,
                exclusive: false, autoDelete: false, arguments: null);
            Channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(Channel);


            consumer.Received += (model, ea) =>
            {
                string response = null;

                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = Channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var wrapper = JsonConvert.DeserializeObject<WrapperCommand>(message);
                    var cmd = JsonConvert.DeserializeObject(JObject.Parse(message)["Command"].ToString(), 
                        wrapper.TypeCommand);
                   
                    using (var scope = services.CreateScope())
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                        handler.SendCommand((Command) cmd);
                    }

                    response = cmd.ToString();
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    Channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                        basicProperties: replyProps, body: responseBytes);
                    Channel.BasicAck(deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
            };

            Channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
        }
    }
}