using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TasinmazApi.Business.Abstract;
using TasinmazApi.DataAccess;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;
using TasinmazApi.Helpers;

namespace TasinmazApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TasinmazController : ControllerBase
    {
        private readonly ITasinmazService _tasinmazService;
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;

        public TasinmazController(ITasinmazService tasinmazService, ApplicationDbContext context, ILogService logService)
        {
            _tasinmazService = tasinmazService;
            _context = context;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rol = User.FindFirstValue(ClaimTypes.Role);
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _tasinmazService.GetAllAsync(rol, kullaniciId);
            return Ok(result);
        }

        [HttpPost("export/pdf")]
        [Authorize(Roles = "Admin")]
        public IActionResult ExportTasinmazPdf([FromBody] List<Tasinmaz> secilenler)
        {
            if (secilenler == null || !secilenler.Any())
                return BadRequest("Hiç taşınmaz seçilmedi.");

            var pdf = TasinmazPdfGenerator.Generate(secilenler);
            return File(pdf, "application/pdf", "tasinmazlar.pdf");
        }

        [HttpPost("export/excel")]
        [Authorize(Roles = "Admin")]
        public IActionResult ExportTasinmazExcel([FromBody] List<Tasinmaz> secilenler)
        {
            if (secilenler == null || !secilenler.Any())
                return BadRequest("Hiç taşınmaz seçilmedi.");

            var excel = TasinmazExcelGenerator.Generate(secilenler);
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "tasinmazlar.xlsx");
        }

        [HttpPost("my/export/pdf")]
        [Authorize(Roles = "Kullanici")]
        public async Task<IActionResult> ExportSelectedPdf([FromBody] List<int> secilenIdler)
        {
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var list = await _context.Tasinmazlar
                .Where(x => secilenIdler.Contains(x.TasinmazId) && x.KullaniciId == kullaniciId)
                .Include(x => x.Mahalle).ThenInclude(m => m.Ilce).ThenInclude(i => i.Il)
                .ToListAsync();

            var pdf = TasinmazPdfGenerator.Generate(list);
            return File(pdf, "application/pdf", "secilen-tasinmazlar.pdf");
        }

        [HttpPost("my/export/excel")]
        [Authorize(Roles = "Kullanici")]
        public async Task<IActionResult> ExportSelectedExcel([FromBody] List<int> secilenIdler)
        {
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var list = await _context.Tasinmazlar
                .Where(x => secilenIdler.Contains(x.TasinmazId) && x.KullaniciId == kullaniciId)
                .ToListAsync();

            var excel = TasinmazExcelGenerator.Generate(list);
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "secilen-tasinmazlar.xlsx");
        }

        [HttpPost]
        [Authorize(Roles = "Kullanici")]
        public async Task<IActionResult> Add([FromBody] TasinmazCreateDto dto)
        {
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var tasinmaz = new Tasinmaz
            {
                KullaniciId = kullaniciId,
                IlId = dto.IlId,
                IlceId = dto.IlceId,
                MahalleId = dto.MahalleId,
                Ada = dto.Ada,
                Parsel = dto.Parsel,
                Nitelik = dto.Nitelik,
                Koordinat = dto.Koordinat
            };

            var result = await _tasinmazService.AddAsync(tasinmaz, kullaniciId);

            if (result == null)
                return BadRequest("Taşınmaz eklenemedi.");

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Kullanici")]
        public async Task<IActionResult> Update([FromBody] TasinmazUpdateDto dto)
        {
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _tasinmazService.UpdateAsync(dto, kullaniciId);

            if (result == null)
                return NotFound("Taşınmaz bulunamadı veya yetkiniz yok.");

            return Ok(result);
        }

       
        [HttpPost("my/toplu-sil")]
        [Authorize(Roles = "Kullanici")]
        public async Task<IActionResult> TopluSil([FromBody] List<int> secilenIdler)
        {
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var tasinmazlar = await _context.Tasinmazlar
                .Where(t => secilenIdler.Contains(t.TasinmazId) && t.KullaniciId == kullaniciId)
                .ToListAsync();

            if (!tasinmazlar.Any())
                return NotFound("Silinecek taşınmaz bulunamadı.");

            _context.Tasinmazlar.RemoveRange(tasinmazlar);
            await _context.SaveChangesAsync();

            foreach (var t in tasinmazlar)
            {
                await _logService.LogEkleAsync(new Log
                {
                    KullaniciId = kullaniciId,
                    IslemTipi = "Silme",
                    Durum = "Başarılı",
                    Aciklama = $"Toplu silme: Taşınmaz silindi. ID: {t.TasinmazId}, Ada: {t.Ada}, Parsel: {t.Parsel}, Nitelik: {t.Nitelik}"
                });
            }

            return Ok($"{tasinmazlar.Count} taşınmaz başarıyla silindi.");
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterTasinmazlar([FromBody] TasinmazFilterDto dto)
        {
            var rol = User.FindFirstValue(ClaimTypes.Role);
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _tasinmazService.FilterAsync(rol, kullaniciId, dto);
            return Ok(result);
        }

        [HttpPost("filter-kullanici")]
        [Authorize(Roles = "Kullanici")]
        public async Task<IActionResult> FilterMyTasinmazlar([FromBody] TasinmazFilterDto dto)
        {
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _tasinmazService.FilterAsync("Kullanici", kullaniciId, dto);
            return Ok(result);
        }
    }
}
