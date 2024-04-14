using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Polly;
using System;
using System.Threading.Tasks;

namespace MessageSenderApp;
public class QueueService
{
    private readonly QueueClient _queueClient;
    private readonly IAsyncPolicy _retryPolicy;
    private readonly string _testQueueName = "webapptest";

    public QueueService(IConfiguration configurationManager, KeyVaultService keyVaultService)
    {
        var queueServiceKeyName = configurationManager.GetValue<string>("QueueServiceKeyName");

        var connectionString = keyVaultService.GetSecret(queueServiceKeyName);

        _queueClient = new QueueClient(connectionString, _testQueueName);
        
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<SendReceipt> SendMessageAsync(string message)
    {
        return await _retryPolicy.ExecuteAsync(async () => 
        {
            return await _queueClient.SendMessageAsync(message);
        });
    }
}
