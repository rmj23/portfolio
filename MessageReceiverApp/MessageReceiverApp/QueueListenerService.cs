using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MessageReceiverApp;

public class QueueListenerService : BackgroundService
{
    private readonly QueueClient _queueClient;

    public QueueListenerService(string connectionString, string queueName)
    {
        _queueClient = new QueueClient(connectionString, queueName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            QueueMessage[] messages = await _queueClient.ReceiveMessagesAsync(10, TimeSpan.FromMinutes(1));

            foreach (var message in messages)
            {
                // Process message

                // Delete the message after processing
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }

            await Task.Delay(5000, stoppingToken); // Check every 5 seconds
        }
    }
}

