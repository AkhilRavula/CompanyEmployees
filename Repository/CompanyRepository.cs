using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>,ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateCompany(Company company)
        {
            Create(company);
        }

        public void DeleteCompany(Company company)
        {
            Delete(company);
        }

        public async Task<IEnumerable<Company>> getAllCompaniesAsync(bool trackchanges)
        {
            return await Find(trackchanges).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(c => ids.Contains(c.Id), trackChanges).ToListAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(Guid CompanyId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(CompanyId), trackChanges).SingleOrDefaultAsync();
        }
    }
}
