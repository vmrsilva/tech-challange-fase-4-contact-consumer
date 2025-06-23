using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallange.Contact.Domain.Region.Exception
{
    public class RegionNotFoundException : System.Exception
    {
        public RegionNotFoundException() : base(message: "Região não encontrada na base dados.")
        {

        }
    }
}
