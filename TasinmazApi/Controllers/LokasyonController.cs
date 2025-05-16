using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasinmazApi.DataAccess;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Giriş yapan herkes erişebilir
    public class LokasyonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LokasyonController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        [HttpGet("iller")]
        public async Task<IActionResult> GetIller()
        {
            var iller = await _context.Iller.ToListAsync();
            return Ok(iller);
        }

       
        [HttpGet("ilceler/{ilId}")]
        public async Task<IActionResult> GetIlceler(int ilId)
        {
            var ilceler = await _context.Ilceler
                .Where(i => i.IlId == ilId)
                .ToListAsync();
            return Ok(ilceler);
        }

        
        [HttpGet("mahalleler/{ilceId}")]
        public async Task<IActionResult> GetMahalleler(int ilceId)
        {
            var mahalleler = await _context.Mahalleler
                .Where(m => m.IlceId == ilceId)
                .ToListAsync();
            return Ok(mahalleler);
        }
    }
}

