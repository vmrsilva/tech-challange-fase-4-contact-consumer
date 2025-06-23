using TechChallange.Contact.Domain.Contact.Entity;
using TechChallange.Contact.Domain.Contact.Repository;

namespace TechChallange.Contact.Domain.Contact.Service
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        //private readonly ICacheRepository _cacheRepository;
        //private readonly IIntegrationService _integrationService;
        //private readonly IRegionIntegration _regionIntegration;


        public ContactService(IContactRepository contactRepository)
                              //ICacheRepository cacheRepository,
                              //IIntegrationService integrationService,
                              //IRegionIntegration regionIntegration)
        {
            _contactRepository = contactRepository;
            //_cacheRepository = cacheRepository;
            //_integrationService = integrationService;
            //_regionIntegration = regionIntegration;
        }
        public async Task CreateAsync(ContactEntity contactEntity)
        {
            //var region = await GetRegionById(contactEntity.RegionId).ConfigureAwait(false);

            //if (region == null)
            //    throw new RegionNotFoundException();

            await _contactRepository.Create(contactEntity).ConfigureAwait(false);
        }

    }
}
