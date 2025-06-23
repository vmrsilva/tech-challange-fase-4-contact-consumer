using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallange.Contact.Domain.Contact.Messaging;
using TechChallange.Contact.Domain.Contact.Service;

namespace TechChallange.Contact.Consumer
{
    public class InsertContactConsumer : IConsumer<ContactCreateMessageDto>
    {

        private readonly IContactService _contactService;
        private readonly ILogger<InsertContactConsumer> _logger;

        public InsertContactConsumer(IContactService contactService, ILogger<InsertContactConsumer> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ContactCreateMessageDto> context)
        {
            var contactMessage = context.Message;

            _logger.LogInformation($"Message receveid {contactMessage.ToString()}");

            try
            {
                await _contactService.CreateAsync(new Domain.Contact.Entity.ContactEntity(
                        contactMessage.Name,
                        contactMessage.Phone,
                        contactMessage.Email,
                        contactMessage.RegionId
                    )).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on insert contact {contactMessage.ToString()} - Error: {ex.Message}");

                throw;
            }


        }
    }
}
