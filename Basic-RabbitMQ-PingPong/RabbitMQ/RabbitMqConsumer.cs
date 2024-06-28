using RabbitMQ.Client;
using System.Text;
using Microsoft.AspNetCore.SignalR;

namespace Basic_RabbitMQ_PingPong.RabbitMQ
{
    public class RabbitMqConsumer: DefaultBasicConsumer, IDisposable
    {
        private readonly IModel _channel;

        private readonly IHubContext<ClientsHub.ClientsHub> _clientsHub;
        public RabbitMqConsumer(IModel channel, IHubContext<ClientsHub.ClientsHub> clientsHub)
        {
            _channel = channel;
            _clientsHub = clientsHub;
        }
        
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            Console.WriteLine($"Consuming Message");
            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));
            Console.WriteLine(string.Concat("Routing tag: ", routingKey));
            string message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine(string.Concat("Message: ", message));
            _channel.BasicAck(deliveryTag, false);

            //  _clientsHub.Clients?.All.SendAsync("ReceiveMessage", "", message).Wait();

            string[] result = message.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);


            _clientsHub.Clients?.AllExcept(result[0]).SendAsync("ReceiveMessage", "", result[1]).Wait();
        }

        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}
