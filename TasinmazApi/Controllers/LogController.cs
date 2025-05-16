using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TasinmazApi.Business.Abstract;
using TasinmazApi.DataAccess;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;
using TasinmazApi.Helpers;

namespace TasinmazApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // sadece admin erişebilir
    public class LogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;
        public LogController(ApplicationDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _context.Loglar
                .OrderByDescending(l => l.TarihSaat)
                .ToListAsync();

            return Ok(logs);
        }
        [HttpPost("export/pdf")]
        [Authorize(Roles = "Admin")]
        public IActionResult ExportSelectedPdf([FromBody] List<Log> selectedLogs)
        {
            if (selectedLogs == null || !selectedLogs.Any())
                return BadRequest("Herhangi bir log seçilmedi.");

            var pdfBytes = LogPdfGenerator.Generate(selectedLogs);
            return File(pdfBytes, "application/pdf", "secili-loglar.pdf");
        }

        [HttpPost("export/excel")]
        [Authorize(Roles = "Admin")]
        public IActionResult ExportSelectedExcel([FromBody] List<Log> selectedLogs)
        {
            if (selectedLogs == null || !selectedLogs.Any())
                return BadRequest("Herhangi bir log seçilmedi.");

            var excelBytes = LogExcelGenerator.Generate(selectedLogs);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "secili-loglar.xlsx");
        }


        [HttpPost("filter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FilterLogs([FromBody] LogFilterDto dto)
        {
            try
            {
                var logs = await _logService.FilterAsync(dto);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata oluştu: {ex.Message}");
            }
        }

    }
}

