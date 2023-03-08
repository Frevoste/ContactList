using ContactListApi.Models;
using ContactListApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactListApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AppUserController : Controller
    {
        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
            
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _appUserService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
