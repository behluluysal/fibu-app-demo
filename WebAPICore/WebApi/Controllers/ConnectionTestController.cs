using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidate]
    public class ConnectionTestController
    {
        [HttpGet]
        public ApiResponse Get()
        {
            return new ApiResponse("Success",200);
        }
    }
}
