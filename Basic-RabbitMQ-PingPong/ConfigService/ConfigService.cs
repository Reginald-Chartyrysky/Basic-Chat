namespace Basic_RabbitMQ_PingPong.ConfigService
{
    public class ConfigService : IConfigService
    {
        public string Host { get; private set; }

        public ConfigService(string hub)
        {
            Host = hub;
        }
    }
}
