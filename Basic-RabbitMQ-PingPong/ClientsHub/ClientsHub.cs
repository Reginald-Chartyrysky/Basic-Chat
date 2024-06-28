using Microsoft.AspNetCore.SignalR;

namespace Basic_RabbitMQ_PingPong.ClientsHub
{
    public class ClientsHub: Hub
    {
        public async Task Send(string username, string message)
        {
            await Clients.All.SendAsync("Receive", username, message);
        }
    }
}
