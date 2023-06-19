using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contract;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public sealed class CompanyService : ICompanyService
    {

        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompanyService(IRepositoryManager repository, ILoggerManager
        logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyCreationDto company)
        {
            var cmpentity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(cmpentity);
            await _repository.SaveAsync();

            return _mapper.Map<CompanyDto>(cmpentity);

        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyCreationDto> CompanyCollection)
        {
            var cmpentities = _mapper.Map<IEnumerable<Company>>(CompanyCollection);

            foreach (var item in cmpentities)
            {
                _repository.Company.CreateCompany(item);
            }
            await _repository.SaveAsync();

            var returnentities = _mapper.Map<IEnumerable<CompanyDto>>(cmpentities);
            var ids = string.Join(",", cmpentities.Select(c => c.Id));

            return (companies:returnentities, ids:ids);

            
        }

        public async Task DeleteEmployeeAsync(Guid id, bool trackChanges)
        {
            var cmp = await _repository.Company.GetCompanyByIdAsync(id, trackChanges);
            if (cmp is null)
                throw new CompanyNotFoundException(id);
            _repository.Company.DeleteCompany(cmp);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<CompanyDto>> getAllCompaniesAsync(bool trackChanges)
        {
            try
            {
                var cmp= await _repository.Company.getAllCompaniesAsync(trackChanges);

                _logger.LogInfo("Hi i am akhil");
                //return cmp.Select(c => new CompanyDto(c.Id, c.Name, c.Address +""+ c.Country)).ToList();
                var cmpdto = _mapper.Map<IEnumerable<CompanyDto>>(cmp);
                return cmpdto;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something Went wrong in {nameof(getAllCompaniesAsync)} Service Method {ex}");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            var cmp = await _repository.Company.GetByIdsAsync(ids, trackChanges: false);
            var returncmp=_mapper.Map<IEnumerable<CompanyDto>>(cmp);
            return returncmp;
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid id, bool trackChanges)
        {
                var cmp = await _repository.Company.GetCompanyByIdAsync(id, trackChanges);
            if (cmp is null)
                throw new CompanyNotFoundException(id);

            return _mapper.Map<CompanyDto>(cmp);
        }

        public async Task UpdateCompanyAsync(Guid CompanyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var cmp = await _repository.Company.GetCompanyByIdAsync(CompanyId, trackChanges);
            if (cmp is null)
                throw new CompanyNotFoundException(CompanyId);
            _mapper.Map(companyForUpdate, cmp);
           await _repository.SaveAsync();
        }
    }
}
