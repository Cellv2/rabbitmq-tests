using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

const string message = "hiya!";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
Console.WriteLine($" [x] Sent: {message}");

Console.WriteLine(" Input [quit] to exit.");
//Console.ReadLine();

string? userInput;

do
{
    userInput = Console.ReadLine();

    if (userInput == null)
    {
        Console.WriteLine("Please enter a message to send to the queue, or enter 'quit' to exit.");
        continue;
    }

    if (userInput.Equals("quit"))
    {
        continue;
    }

    await PublishMessage(userInput);

} while (userInput != null && !userInput.Equals("quit", StringComparison.OrdinalIgnoreCase));

async Task PublishMessage(string message)
{
    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchange: string.Empty, routingKey : "hello", body: body);
}