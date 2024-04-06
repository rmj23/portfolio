﻿using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Polly;
using System;
using System.Threading.Tasks;

namespace MessageSenderApp;
public class QueueService
{
    private readonly QueueClient _queueClient;
    private readonly IAsyncPolicy _retryPolicy;

    public QueueService(string connectionString, string queueName)
    {
        _queueClient = new QueueClient(connectionString, queueName);
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
