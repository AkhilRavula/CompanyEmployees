using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{

    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/CompaniesV2")]
    public class CompaniesV2Controller : ControllerBase
    {

        private readonly IServiceManager _serviceManager;

        public CompaniesV2Controller(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompaniesAsync()
        {
           var result = await _serviceManager.CompanyService.getAllCompaniesAsync(false);

            return Ok(result);
        }
    }
}
