using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contract;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> CreateEmployeeAsync(Guid CompanyId, IEnumerable<EmployeeCreationDto> employeeCreationDto)
        {

            var empentities = _mapper.Map<IEnumerable<Employee>>(employeeCreationDto);

            foreach (var item in empentities)
            {               
                _repository.Employee.CreateEmployee(CompanyId, item);
            }           
            await _repository.SaveAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(empentities);
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id,bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(id);
            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<EmployeeDto> employees,Metadata metadata)> GetAllEmployeesAsync(Guid id, EmployeeParameters employeeParameters,bool trackChanges)
        {
            if(!employeeParameters.ValidAgeRange)
            {
                throw new MaxAgeRangeBadRequestException();
            }
            var employeeswithmetadata = await _repository.Employee.GetAllEmployeesAsync(id, employeeParameters,trackChanges: false);
            if (employeeswithmetadata is null)
                throw new CompanyNotFoundException(id);
            var employeedto = _mapper.Map<IEnumerable<EmployeeDto>>(employeeswithmetadata);
            return (employees:employeedto,metadata:employeeswithmetadata.Metadata);
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, false);
            if (employee is null)
                throw new EmployeeNotFoundException(id);
            var employeedto = _mapper.Map<EmployeeDto>(employee);
            return employeedto;
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repository.Company.GetCompanyByIdAsync(companyId, compTrackChanges);

            if (company is null) 
                throw new CompanyNotFoundException(companyId); 

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges); 

            if (employeeEntity is null)
                throw new EmployeeNotFoundException(companyId); 

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity); 
            return (employeeToPatch, employeeEntity);
        }

        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }

        public async Task UpdateEmployeeForCompanyAsync(Guid CompanyId, Guid id, 
            EmployeeForUpdateDto employeeForUpdateDto, bool trackCmpChanges, bool trackEmpChanges)
        {
            var company = await _repository.Company.GetCompanyByIdAsync(CompanyId, trackCmpChanges);
            if (company is null)
                throw new CompanyNotFoundException(CompanyId);
            var employee = await _repository.Employee.GetEmployeeAsync(CompanyId, id, trackEmpChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map<Employee>(employeeForUpdateDto);

            _mapper.Map(employeeForUpdateDto, employee);

            await _repository.SaveAsync();
        }
    }
}
