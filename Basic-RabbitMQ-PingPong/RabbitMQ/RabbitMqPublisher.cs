using System.Text;
using RabbitMQ.Client;
namespace Basic_RabbitMQ_PingPong.RabbitMQ
{
    public class RabbitMqPublisher
    {
        public void SendMessage(IModel channel, string message, string queueName = "DefaultQueue")
        {
            channel.QueueDeclare(queue: queueName,
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                           routingKey: queueName,
                           basicProperties: null,
                           body: body);
        }
    }
}
