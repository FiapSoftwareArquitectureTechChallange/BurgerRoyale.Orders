using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.Exceptions;
using BurgerRoyale.Orders.Domain.Interface.IntegrationServices;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BurgerRoyale.Orders.Infrastructure.IntegrationServices;

public class AWSSQSService : IMessageService
{
    private readonly AWSConfiguration _awsConfiguration;
    private readonly IAmazonSQS _amazonSQSClient;

    public AWSSQSService(IOptions<AWSConfiguration> awsConfiguration)
    {
        _awsConfiguration = awsConfiguration.Value;
        _amazonSQSClient = CreateClient();
    }

    public async Task<string> SendMessageAsync(string queueName, string message)
    {
        try
        {
            string queueUrl = await GetQueueUrl(queueName);

            var response = await _amazonSQSClient.SendMessageAsync(queueUrl, message);

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

            var response = await _amazonSQSClient.ReceiveMessageAsync(request);

            List<TResponse> messages = new();

            foreach (var message in response.Messages)
            {
                messages.Add(JsonSerializer.Deserialize<TResponse>(message.Body)!);

                await _amazonSQSClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
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

    private AmazonSQSClient CreateClient()
    {
        var credentials = new SessionAWSCredentials(
            _awsConfiguration.AccessKey,
            _awsConfiguration.SecretKey,
            _awsConfiguration.SessionToken
        );

        var region = RegionEndpoint.GetBySystemName(_awsConfiguration.Region);

        return new AmazonSQSClient(
            credentials,
            region
        );
    }

    private async Task<string> GetQueueUrl(string queueName)
    {
        try
        {
            var response = await _amazonSQSClient.GetQueueUrlAsync(
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
        var response = await _amazonSQSClient.CreateQueueAsync(
            new CreateQueueRequest(queueName)
        );

        return response.QueueUrl;
    }    
}
