namespace Basic_RabbitMQ_PingPong.RabbitMQ
{
    public interface IRabbitMqService
    {
        public void SendMessage(object obj, string queueName);
        public void SendMessage(string message, string queueName);
    }
}
