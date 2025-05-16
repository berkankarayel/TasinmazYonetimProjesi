using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasinmazApi.Business.Abstract;
using TasinmazApi.Business.Concrete;
using TasinmazApi.DataAccess;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;
using TasinmazApi.Helpers;

namespace TasinmazApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;

        public UserController(IUserService userService, ApplicationDbContext context, ILogService logService)
        {
            _userService = userService;
            _context = context;
            _logService = logService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            var result = await _userService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Kullanıcı başarıyla eklendi..." });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Kullanıcıyı başarıyla güncelledi...." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Kullanıcıyı başarıyla sildi ..." });
        }

        [HttpPost("toplu-sil")]
        public async Task<IActionResult> TopluSil([FromBody] List<int> secilenIdler)
        {
            var kullanicilar = await _context.Kullanicilar
                .Where(k => secilenIdler.Contains(k.KullaniciId))
                .ToListAsync();

            if (!kullanicilar.Any())
                return NotFound("Silinecek kullanıcı bulunamadı.");

            // 🔥 Her silinen kullanıcı için log ekle
            foreach (var k in kullanicilar)
            {
                await _logService.LogEkleAsync(new Log
                {
                    KullaniciId = k.KullaniciId,
                    IslemTipi = "Silme",
                    Durum = "Başarılı",
                    Aciklama = $" silinen kullanıcı: {k.KullaniciAdi} {k.KullaniciSoyadi} ({k.Email})"
                });
            }

            _context.Kullanicilar.RemoveRange(kullanicilar);
            await _context.SaveChangesAsync();

            return Ok("Kullanıcılar başarıyla silindi.");
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost("export/pdf")]
        public IActionResult ExportUserPdf([FromBody] List<Kullanici> secilenKullanicilar)
        {
            var pdf = UserPdfGenerator.Generate(secilenKullanicilar);
            return File(pdf, "application/pdf", "kullanicilar.pdf");
        }

        [HttpPost("export/excel")]
        public IActionResult ExportUserExcel([FromBody] List<Kullanici> secilenKullanicilar)
        {
            var excel = UserExcelGenerator.Generate(secilenKullanicilar);
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "kullanicilar.xlsx");
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterUsers([FromBody] UserFilterDto dto)
        {
            var users = await _userService.FilterAsync(dto);
            return Ok(users);
        }
    }
}
