using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Contract;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shared.RequestFeatures;
using CompanyEmployees.Presentation.ActionFilters;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public EmployeesController(IServiceManager service)
        {
            _service = service;
        }
        
        [HttpGet(Name ="GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees(Guid companyId,[FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedemployees=await _service.EmployeeService.GetAllEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", 
                System.Text.Json.JsonSerializer.Serialize(pagedemployees.metadata));
            return Ok(pagedemployees.employees);
        }

        [HttpGet("{id:Guid}",Name ="GetEmployee")]
        public async Task<IActionResult> GetEmployee(Guid CompanyId,Guid id)
        {
            var emp = await _service.EmployeeService.GetEmployeeAsync(CompanyId, id, false);
            return Ok(emp);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid CompanyId ,[FromBody] IEnumerable<EmployeeCreationDto> employeeCreationDto)
        {
            if (string.IsNullOrEmpty(CompanyId.ToString()) || employeeCreationDto is null)
                return BadRequest("CompanyId cannot be null");
            if(!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);

                //convert to a string
                var modelStateJson = JsonConvert.SerializeObject(serializableModelState);
                return UnprocessableEntity(modelStateJson);
            }

            var emp=await _service.EmployeeService.CreateEmployeeAsync(CompanyId, employeeCreationDto);
            return CreatedAtRoute("GetAllEmployees", new { CompanyId }, emp);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid CompanyId,Guid id)
        {
           await  _service.EmployeeService.DeleteEmployeeForCompanyAsync(CompanyId, id, false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid CompanyId,Guid id,[FromBody] EmployeeForUpdateDto updateDto)
        {
            

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            await _service.EmployeeService.UpdateEmployeeForCompanyAsync(CompanyId, id, updateDto, false, true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")] 
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        { 
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null."); 

            var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id,
                compTrackChanges: false, empTrackChanges: true); 

            patchDoc.ApplyTo(result.employeeToPatch,ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);

                //convert to a string
                var modelStateJson = JsonConvert.SerializeObject(serializableModelState);
                return UnprocessableEntity(modelStateJson);
            }

            await _service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
            return NoContent(); 
        }
    }
}
