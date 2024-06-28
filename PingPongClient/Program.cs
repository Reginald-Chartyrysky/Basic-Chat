using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQPingPongApiCommunication;

namespace PingPongClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var apiCommunicationService = ApiCommunicationService.BuildApiCommunicationServiceAsync().Result;
            apiCommunicationService.MessageReceived += (message) =>
            {
                Console.WriteLine(message);
            };

            while (true)
            {
                string message = Console.ReadLine();
                apiCommunicationService.SendMessageAsync(message).Wait();
            }
        }
    }
}
