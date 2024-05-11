using Amazon.SQS;
using Amazon.SQS.Model;
using BurgerRoyale.Orders.Domain.Exceptions;
using BurgerRoyale.Orders.Infrastructure.IntegrationServices;
using FluentAssertions;
using Moq;
using System.Text.Json;
using Xunit;

namespace BurgerRoyale.Orders.UnitTests.Infrastructure.IntegrationServices;

public class AwsSqsServiceTests
{
    private readonly Mock<IAmazonSQS> _amazonSqsClientMock;

    private readonly AwsSqsService _awsSqsService;

    public AwsSqsServiceTests()
    {
        _amazonSqsClientMock = new Mock<IAmazonSQS>();

        _awsSqsService = new AwsSqsService(_amazonSqsClientMock.Object);
    }

    [Fact]
    public async Task GivenStringMessage_WhenExceptionOccursOnSendMessage_ThenShouldThrowIntegrationException()
    {
        // arrange
        string queueName = "queue";

        _amazonSqsClientMock
            .Setup(x => x.GetQueueUrlAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ThrowsAsync(new Exception("Exception message"));

        // act
        Func<Task> task = async () => await _awsSqsService.SendMessageAsync(queueName, "");

        // assert
        await task.Should()
            .ThrowAsync<IntegrationException>()
            .WithMessage($"Error sending messages to AWS SQS Queue ({queueName})");
    }

    [Fact]
    public async Task GivenStringMessageAndQueueDoesNotExist_WhenSendMessage_ThenShouldCreateQueueAndSendMessage()
    {
        // arrange
        string queueName = "queue";
        string queueUrl = $"http://localhost/{queueName}";

        string message = "Message";
        string messageId = Guid.NewGuid().ToString();

        _amazonSqsClientMock
            .Setup(x => x.GetQueueUrlAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ThrowsAsync(new QueueDoesNotExistException("Exception message"));

        _amazonSqsClientMock
            .Setup(x => x.CreateQueueAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new CreateQueueResponse() {
                QueueUrl = queueUrl
            });

        _amazonSqsClientMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new SendMessageResponse()
            {
                MessageId = messageId
            });

        // act
        var response = await _awsSqsService.SendMessageAsync(queueName, message);

        // assert
        response.Should().Be(messageId);

        _amazonSqsClientMock
            .Verify(
                x => x.CreateQueueAsync(
                    queueName,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

        _amazonSqsClientMock
            .Verify(
                x => x.SendMessageAsync(
                    queueUrl,
                    message,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    [Fact]
    public async Task GivenDynamicMessageAndExistingQueueName_WhenSendMessage_ThenShouldSerializeMessageAndSendMessageToExistingQueue()
    {
        // arrange
        string queueName = "queue";
        string queueUrl = $"http://localhost/{queueName}";

        var messageBody = new { MessageProperty = "Value" };
        string messageId = Guid.NewGuid().ToString();

        _amazonSqsClientMock
            .Setup(x => x.GetQueueUrlAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new GetQueueUrlResponse() {
                QueueUrl = queueUrl
            });

        _amazonSqsClientMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new SendMessageResponse()
            {
                MessageId = messageId
            });

        // act
        var response = await _awsSqsService.SendMessageAsync(queueName, messageBody);

        // assert
        response.Should().Be(messageId);

        _amazonSqsClientMock
            .Verify(
                x => x.CreateQueueAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Never
            );

        _amazonSqsClientMock
            .Verify(
                x => x.SendMessageAsync(
                    queueUrl,
                    It.Is<string>(m => m.Contains("MessageProperty")),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    [Fact]
    public async Task GivenQueueName_WhenExceptionOccursReadingMessages_ThenShouldThrowIntegrationException()
    {
        // arrange
        string queueName = "queue";

        _amazonSqsClientMock
            .Setup(x => x.GetQueueUrlAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ThrowsAsync(new Exception("Exception message"));

        // act
        Func<Task> task = async () => await _awsSqsService.ReadMessagesAsync<string>(queueName, 10);

        // assert
        await task.Should()
            .ThrowAsync<IntegrationException>()
            .WithMessage($"Error reading messages from AWS SQS Queue ({queueName})");
    }

    [Fact]
    public async Task GivenQueueName_WhenReadingMessages_ThenShouldReturnMessagesAndRemoveFromQueue()
    {
        // arrange
        string queueName = "queue";
        string queueUrl = $"http://localhost/{queueName}";

        _amazonSqsClientMock
            .Setup(x => x.GetQueueUrlAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new GetQueueUrlResponse()
            {
                QueueUrl = queueUrl
            });

        _amazonSqsClientMock
            .Setup(
                x => x.ReceiveMessageAsync(
                    It.IsAny<ReceiveMessageRequest>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ReceiveMessageResponse() {
                Messages = [
                    new() { Body = JsonSerializer.Serialize(new { MessageProperty = "Message 1" }) },
                    new() { Body = JsonSerializer.Serialize(new { MessageProperty = "Message 2" }) },
                    new() { Body = JsonSerializer.Serialize(new { MessageProperty = "Message 3" }) }
                ]
            });

        // act
        var response = await _awsSqsService.ReadMessagesAsync<dynamic>(queueName, null);

        // assert
        response.Should().HaveCount(3);

        _amazonSqsClientMock
            .Verify(
                x => x.ReceiveMessageAsync(
                    It.Is<ReceiveMessageRequest>(r =>
                        r.QueueUrl == queueUrl
                        && r.MaxNumberOfMessages == 10
                    ),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

        _amazonSqsClientMock
            .Verify(
                x => x.DeleteMessageAsync(
                    queueUrl,
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Exactly(3)
            );
    }
}
