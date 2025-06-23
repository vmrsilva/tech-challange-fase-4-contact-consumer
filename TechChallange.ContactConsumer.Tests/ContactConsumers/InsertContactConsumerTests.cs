using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using TechChallange.Contact.Consumer;
using TechChallange.Contact.Domain.Contact.Entity;
using TechChallange.Contact.Domain.Contact.Messaging;
using TechChallange.Contact.Domain.Contact.Service;

namespace TechChallange.ContactConsumer.Tests.ContactConsumers
{
    public class InsertContactConsumerTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Mock<ILogger<InsertContactConsumer>> _loggerMock;
        private readonly InsertContactConsumer _consumer;

        public InsertContactConsumerTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _loggerMock = new Mock<ILogger<InsertContactConsumer>>();
            _consumer = new InsertContactConsumer(_contactServiceMock.Object, _loggerMock.Object);
        }

        [Fact(DisplayName = "Should consume ContactCreateMessageDto and call CreateAsync successfully")]
        public async Task ConsumeValidMessageShouldCallCreateAsync()
        {
            var message = new ContactCreateMessageDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123456789",
                RegionId = Guid.NewGuid()
            };

            var contextMock = new Mock<ConsumeContext<ContactCreateMessageDto>>();
            contextMock.Setup(x => x.Message).Returns(message);

            await _consumer.Consume(contextMock.Object);

            _contactServiceMock.Verify(service => service.CreateAsync(It.Is<ContactEntity>(c =>
                c.Name == message.Name &&
                c.Email == message.Email &&
                c.Phone == message.Phone &&
                c.RegionId == message.RegionId
            )), Times.Once);
        }

        [Fact(DisplayName = "Should log error and rethrow exception when CreateAsync fails")]
        public async Task ConsumeCreateAsyncThrowsExceptionShouldLogErrorAndThrow()
        {
            var message = new ContactCreateMessageDto
            {
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Phone = "987654321",
                RegionId = Guid.NewGuid()
            };

            var contextMock = new Mock<ConsumeContext<ContactCreateMessageDto>>();
            contextMock.Setup(x => x.Message).Returns(message);

            var exception = new Exception("Test exception");
            _contactServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<ContactEntity>()))
                .ThrowsAsync(exception);

            var ex = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(contextMock.Object));
            Assert.Equal("Test exception", ex.Message);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error on insert contact")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }
    }
}
