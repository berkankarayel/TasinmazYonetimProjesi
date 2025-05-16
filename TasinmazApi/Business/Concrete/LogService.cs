using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TasinmazApi.Business.Abstract;
using TasinmazApi.DataAccess;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Business.Concrete
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task LogEkleAsync(Log log)
        {
            log.TarihSaat = DateTime.UtcNow;
            log.KullaniciIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            _context.Loglar.Add(log);
            await _context.SaveChangesAsync();
        }



        public async Task<List<Log>> FilterAsync(LogFilterDto dto)
        {
            var query = _context.Loglar.AsQueryable();

            if (dto.KullaniciId.HasValue)
                query = query.Where(x => x.KullaniciId == dto.KullaniciId);

            if (!string.IsNullOrEmpty(dto.IslemTipi))
                query = query.Where(x => x.IslemTipi.ToLower() == dto.IslemTipi.ToLower());

            if (!string.IsNullOrEmpty(dto.Durum))
                query = query.Where(x => x.Durum.ToLower() == dto.Durum.ToLower());

            if (dto.BaslangicTarihi.HasValue)
            {
                var baslangicUtc = DateTime.SpecifyKind(dto.BaslangicTarihi.Value, DateTimeKind.Utc);
                query = query.Where(x => x.TarihSaat >= baslangicUtc);
            }

            if (dto.BitisTarihiGunSonu.HasValue)
            {
                var bitisUtc = DateTime.SpecifyKind(dto.BitisTarihiGunSonu.Value, DateTimeKind.Utc);
                query = query.Where(x => x.TarihSaat <= bitisUtc);
            }

            return await query.OrderByDescending(x => x.TarihSaat).ToListAsync();
        }


    }
}
