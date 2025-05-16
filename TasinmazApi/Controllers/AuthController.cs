using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TasinmazApi.Business.Abstract;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; // authsefvisin arayüzünü kullanacak servis nesnesi. 

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Kullanici kullanici)
        {
            var result = await _authService.RegisterAsync(kullanici);
            if (result.Contains("zaten"))
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] KullaniciLoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result.StartsWith("Kullanıcı") || result.Contains("boş"))
                return BadRequest(result);

            return Ok(new { token = result });
        }
    }
}
