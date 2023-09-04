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
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto user)
        {
            var result = await serviceManager.authenticationService.RegisterUser(user);

            if(!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.TryAddModelError(err.Code, err.Description);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(201);

        }
   
        
        [HttpPost("login")]
        public async Task<IActionResult> ValidateUser([FromBody] UserForAuthenticationDto user)
        {
            if(! await serviceManager.authenticationService.ValidateUser(user))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Token = await serviceManager.authenticationService.CreateToken()
            });
        }
    
    }
}
