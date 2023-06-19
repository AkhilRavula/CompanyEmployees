using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> getAllCompaniesAsync(bool trackchanges);
        Task<Company> GetCompanyByIdAsync(Guid CompanyId,bool trackChanges);

        void CreateCompany(Company company);

        void DeleteCompany(Company company);

        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}
