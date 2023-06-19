using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> getAllCompaniesAsync(bool trackChanges);
        Task<CompanyDto> GetCompanyByIdAsync(Guid id,bool trackChanges);

        Task<CompanyDto> CreateCompanyAsync(CompanyCreationDto company);

        Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyCreationDto> CompanyCollection);

        Task DeleteEmployeeAsync(Guid id, bool trackChanges);

        Task UpdateCompanyAsync(Guid CompanyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);
    }
}
