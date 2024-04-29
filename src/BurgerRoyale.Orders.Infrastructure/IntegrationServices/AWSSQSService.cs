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
                "Error sending message to AWS SQS Queue",
                exception
            );
        }
    }

    public async Task<string> SendMessageAsync(string queueName, dynamic messageBody)
    {
        string message = JsonSerializer.Serialize(messageBody);

        return await SendMessageAsync(queueName, message);
    }

    private IAmazonSQS CreateClient()
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
