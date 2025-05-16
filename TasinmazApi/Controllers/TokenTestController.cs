using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TasinmazApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TokenTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult TestToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var rol = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                Message = "Token geçerli ✅",
                KullaniciId = userId,
                Email = email,
                Rol = rol
            });
        }
    }
}
