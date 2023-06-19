using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{

    [ApiController]
    [Route("api")]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        public RootController(LinkGenerator linkGenerator) => _linkGenerator = linkGenerator;


        [HttpGet(Name ="GetRoot")]
        public IActionResult GetRoot([FromHeader(Name ="Accept")] string mediatype)
        {

            if(mediatype.Contains("application/vnd.codemaze.apiroot"))
            {
                var linklist = new List<Link>()
                {
                    new Link
                    {
                        Href=_linkGenerator.GetUriByName(HttpContext,nameof(GetRoot),new{ }),
                        Rel="self",
                        Method="Get"
                    },
                    new Link
                    {
                        Href=_linkGenerator.GetUriByName(HttpContext,"GetCompanies",new{ }),
                        Rel="Companies",
                        Method="Get"
                    },
                    new Link
                    {
                        Href=_linkGenerator.GetUriByName(HttpContext,"CreateCompany",new{ }),
                        Rel="Create Company",
                        Method="Post"
                    }

                };

                return Ok(linklist);
            }

            return NoContent();
        }

    }


}
