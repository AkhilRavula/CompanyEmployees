using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid CompanyId) : base($"The Company with CompanyId {CompanyId}"+
           "doesnt exist in database")
        {

        }
    }
}
