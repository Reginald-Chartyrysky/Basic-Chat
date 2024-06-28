using Basic_RabbitMQ_PingPong.ConfigService;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using System.Text.Json;

namespace Basic_RabbitMQ_PingPong.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService, IDisposable
    {
        public ConnectionFactory ConnectionFactory { get; private set; }
        private IConnection? _connection;
        private IModel? _channel;
        private RabbitMqPublisher _publisher;
        private RabbitMqConsumer _consumer;
        public RabbitMqService(IConfigService configService, IHubContext<ClientsHub.ClientsHub> clientsHub)
        {
            ConnectionFactory = new ConnectionFactory() { HostName = configService.Host };
            

            OpenConnection();
            _publisher = new();
            _consumer = new(_channel!, clientsHub);
            _channel.BasicConsume("DefaultQueue", false, _consumer);
        }
        ~RabbitMqService()
        {
            Dispose();
        }


        public void SendMessage(object obj, string queueName = "DefaultQueue")
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message, queueName);
        }

        public void SendMessage(string message, string queueName = "DefaultQueue")
        {
            // Не забудьте вынести значение "DefaultQueue"
            // в файл конфигурации
            OpenConnection();

            using var channel = _connection!.CreateModel();
            _publisher.SendMessage(channel!, message, queueName);
        }

        private void OpenConnection()
        {
            _connection ??= ConnectionFactory.CreateConnection();
            _channel ??= _connection.CreateModel();
        }

        public void Dispose()
        {
            _consumer?.Dispose();
            _connection?.Dispose();
        }
    }
}
