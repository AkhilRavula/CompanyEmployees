using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetAllEmployeesAsync(Guid id,EmployeeParameters employeeParameters ,bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid CompanyId, Guid id, bool trackChanges);

        void CreateEmployee(Guid CompanyId, Employee employee);

        void DeleteEmployee(Employee employee);
    }
}
