using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallange.Contact.Domain.Contact.Entity;

namespace TechChallange.Contact.Domain.Contact.Service
{
    public interface IContactService
    {
        Task CreateAsync(ContactEntity contactEntity);
    }
}
