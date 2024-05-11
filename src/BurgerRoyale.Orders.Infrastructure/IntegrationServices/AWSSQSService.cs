using Amazon.SQS;
using Amazon.SQS.Model;
using BurgerRoyale.Orders.Domain.Exceptions;
using BurgerRoyale.Orders.Domain.Interface.IntegrationServices;
using System.Text.Json;

namespace BurgerRoyale.Orders.Infrastructure.IntegrationServices;

public class AwsSqsService : IMessageService
{
    private readonly IAmazonSQS _amazonSqsClient;

    public AwsSqsService(IAmazonSQS amazonSqsClient)
    {
        _amazonSqsClient = amazonSqsClient;
    }

    public async Task<string> SendMessageAsync(string queueName, string message)
    {
        try
        {
            string queueUrl = await GetQueueUrl(queueName);

            var response = await _amazonSqsClient.SendMessageAsync(queueUrl, message);

            return response.MessageId;
        }
        catch (Exception exception)
        {
            throw new IntegrationException(
                $"Error sending messages to AWS SQS Queue ({queueName})",
                exception
            );
        }
    }

    public async Task<string> SendMessageAsync(string queueName, dynamic messageBody)
    {
        string message = JsonSerializer.Serialize(messageBody);

        return await SendMessageAsync(queueName, message);
    }

    public async Task<IEnumerable<TResponse>> ReadMessagesAsync<TResponse>(string queueName, int? maxNumberOfMessages)
    {
        try
        {
            string queueUrl = await GetQueueUrl(queueName);

            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = maxNumberOfMessages ?? 10
            };

            var response = await _amazonSqsClient.ReceiveMessageAsync(request);

            List<TResponse> messages = new();

            foreach (var message in response.Messages)
            {
                messages.Add(JsonSerializer.Deserialize<TResponse>(message.Body)!);

                await _amazonSqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
            }

            return messages;
        }
        catch (Exception exception)
        {
            throw new IntegrationException(
                $"Error reading messages from AWS SQS Queue ({queueName})",
                exception
            );
        }
    }

    private async Task<string> GetQueueUrl(string queueName)
    {
        try
        {
            var response = await _amazonSqsClient.GetQueueUrlAsync(
                new GetQueueUrlRequest(queueName)
            );

            return response.QueueUrl;
        }
        catch (QueueDoesNotExistException)
        {
            return await CreateQueue(queueName);
        }
    }

    private async Task<string> CreateQueue(string queueName)
    {
        var response = await _amazonSqsClient.CreateQueueAsync(
            new CreateQueueRequest(queueName)
        );

        return response.QueueUrl;
    }    
}
