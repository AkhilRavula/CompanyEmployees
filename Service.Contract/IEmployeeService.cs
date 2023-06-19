using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IEmployeeService
    {

        Task<(IEnumerable<EmployeeDto> employees,Metadata metadata)> GetAllEmployeesAsync(Guid id,EmployeeParameters employeeParameters, bool trackChanges);

        Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

        Task<IEnumerable<EmployeeDto>> CreateEmployeeAsync(Guid CompanyId, IEnumerable<EmployeeCreationDto> employeeCreationDto);

        Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id,bool trackChanges);

        Task UpdateEmployeeForCompanyAsync(Guid CompanyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool trackCmpChanges, bool trackEmpChanges);

        Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges); 
        Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}
