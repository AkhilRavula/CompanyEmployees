using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/Companies")]
    //[ResponseCache(CacheProfileName = "120SecondsDuration")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _Service;


        public CompaniesController(IServiceManager Service)
        {
            _Service = Service;
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _Service.CompanyService.DeleteEmployeeAsync(id, trackChanges: false);
            return NoContent();
        }

        [HttpGet (Name ="GetCompanies")]
        //[ResponseCache(Duration =60)]
        [Authorize]
        public async Task<IActionResult> GetAllCompanies()
        {              
                var list =await _Service.CompanyService.getAllCompaniesAsync(trackChanges:false);
                return Ok(list);         
        }


        [HttpGet("{id:Guid}",Name ="CompanyById")]

        public async Task<IActionResult> GetCompanyById(Guid id)
        {              
                var cmp = await _Service.CompanyService.GetCompanyByIdAsync(id, trackChanges: false);
                return Ok(cmp);
        }

        [HttpPost(Name ="CreateCompany")]

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyCreationDto companyCreationDto)
        {
            //if (companyCreationDto is null) 
            //    return BadRequest("CompanyForCreationDto object is null");

            var cmp = await _Service.CompanyService.CreateCompanyAsync(companyCreationDto);

            return CreatedAtRoute("CompanyById", new { id = cmp.Id }, cmp);
        }


        [HttpGet("Collection/({ids})",Name ="GetCompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var Cmp = await _Service.CompanyService.GetByIdsAsync(ids, false);
            return Ok(Cmp);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody]IEnumerable<CompanyCreationDto> companies)
        {
            var cmp = await _Service.CompanyService.CreateCompanyCollectionAsync(companies);

            return CreatedAtRoute("GetCompanyCollection", new { cmp.ids }, cmp.companies);
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id,[FromBody]CompanyForUpdateDto companyForUpdate)
        {
            //if (companyForUpdate is null)
            //    return BadRequest("CompanyForUpdateDto object is null");

            await _Service.CompanyService.UpdateCompanyAsync(id, companyForUpdate, true);

            return NoContent();
        }

    }
}
