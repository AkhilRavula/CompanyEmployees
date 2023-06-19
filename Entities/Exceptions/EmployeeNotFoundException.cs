using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid EmployeeId) : base ($"Employee with Id {EmployeeId} doesnt exist in database")
        {

        }
    }
}
