using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;

namespace RabbitMQPingPongApiCommunication
{
    public sealed class ApiCommunicationService: IDisposable
    {
        private const string URL = "https://localhost:7038";
        private const string SEND_MESSAGE_URI = $"{URL}/api/RabbitMq/SendMessage/";
        private readonly HttpClient _httpClient;

        public delegate void MessageReceivedHandler(string message);

        public event MessageReceivedHandler MessageReceived;
        private HubConnection _connection;


        async public static Task<ApiCommunicationService> BuildApiCommunicationServiceAsync()
        {
            HubConnection connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:7038/Chat")
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            await connection.StartAsync();

            return new ApiCommunicationService(connection);
        }

        private ApiCommunicationService(HubConnection connection)
        {
            _connection = connection;
            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                MessageReceived?.Invoke(message);
            });
           
            _httpClient = new()
            {
                BaseAddress = new Uri(URL)
            };

            // Add an Accept header for JSON format.
            _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        }


        public async Task SendMessageAsync(string message)
        {
            string requestUri = new(SEND_MESSAGE_URI + $"[{_connection.ConnectionId}][{message}]");
            HttpRequestMessage requestMessage = new(HttpMethod.Get, requestUri);

            var response = await _httpClient.SendAsync(requestMessage);
            //Console.WriteLine(response?.Content);
        }

        public void Dispose()
        {

            // Dispose once all HttpClient calls are complete. 
            _httpClient.Dispose();
        }
    }
}
