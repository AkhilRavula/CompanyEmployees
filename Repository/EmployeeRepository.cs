using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using Repository.Extensions;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>,IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateEmployee(Guid CompanyId, Employee employee)
        {
            employee.CompanyId = CompanyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }

        public async Task<PagedList<Employee>> GetAllEmployeesAsync(Guid id,EmployeeParameters employeeParameters, bool trackChanges)
        {
            var totalemp= await FindByCondition(c => c.CompanyId.Equals(id), trackChanges)
                .FilterEmployees(employeeParameters.MinAge,employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy).ToListAsync();

            return PagedList<Employee>.Pagedlist(totalemp, employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid CompanyId, Guid id, bool trackChanges)
        {
            return await FindByCondition(c => c.CompanyId.Equals(CompanyId) && c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
    }
}
