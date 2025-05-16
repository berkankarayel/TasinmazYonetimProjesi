using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TasinmazApi.Business.Abstract;
using TasinmazApi.DataAccess;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;
using TasinmazApi.Enums;

namespace TasinmazApi.Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;

        public UserService(ApplicationDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public async Task<ResultDto> AddAsync(UserCreateDto dto)
        {
            if (await _context.Kullanicilar.AnyAsync(x => x.Email == dto.Email))
            {
                return new ResultDto { Success = false, Message = "Bu email zaten kullanılıyor!" };
            }

            
            var salt = Guid.NewGuid().ToString("N").Substring(0, 16);
            var hashedPassword = HashPassword(dto.Sifre + salt);

            var kullanici = new Kullanici
            {
                KullaniciAdi = dto.KullaniciAdi,
                KullaniciSoyadi = dto.KullaniciSoyadi,
                Email = dto.Email,
                Sifre = hashedPassword,
                Salt = salt,
                Rol = Enum.Parse<UserRole>(dto.Rol),
                Adres = dto.Adres
            };

            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();

            await _logService.LogEkleAsync(new Log
            {
                KullaniciId = kullanici.KullaniciId,
                IslemTipi = "Yeni Kayıt",
                Durum = "Başarılı",
                Aciklama = $"Yeni kullanıcı eklendi: {kullanici.KullaniciAdi} {kullanici.KullaniciSoyadi} ({kullanici.Email}), Rol: {kullanici.Rol}",
                TarihSaat = DateTime.UtcNow,
                KullaniciIp = "127.0.0.1"
            });

            return new ResultDto { Success = true, Message = "Kullanıcı başarıyla eklendi ✅" };
        }

        public async Task<ResultDto> UpdateAsync(UserUpdateDto dto)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(dto.KullaniciId);
            if (kullanici == null)
            {
                return new ResultDto { Success = false, Message = "Kullanıcı bulunamadı!" };
            }

            kullanici.KullaniciAdi = dto.KullaniciAdi;
            kullanici.KullaniciSoyadi = dto.KullaniciSoyadi;
            kullanici.Email = dto.Email;
            kullanici.Adres = dto.Adres;
            kullanici.Rol = Enum.Parse<UserRole>(dto.Rol);

            await _context.SaveChangesAsync();

            await _logService.LogEkleAsync(new Log
            {
                KullaniciId = kullanici.KullaniciId,
                IslemTipi = "Güncelleme",
                Durum = "Başarılı",
                Aciklama = $"Kullanıcı bilgileri güncellendi: {kullanici.KullaniciAdi} {kullanici.KullaniciSoyadi} ({kullanici.Email}), Yeni Rol: {kullanici.Rol}",
                TarihSaat = DateTime.UtcNow,
                KullaniciIp = "127.0.0.1"
            });

            return new ResultDto { Success = true, Message = "Kullanıcı başarıyla güncellendi ✅" };
        }


        public async Task<ResultDto> DeleteAsync(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null)
            {
                return new ResultDto { Success = false, Message = "Kullanıcı bulunamadı!" };
            }

            // Loglama artık LogService üzerinden yapılacak
            await _logService.LogEkleAsync(new Log
            {
                KullaniciId = id,
                IslemTipi = "Silme",
                Durum = "Başarılı",
                Aciklama = $"Kullanıcı silindi: {kullanici.KullaniciAdi} {kullanici.KullaniciSoyadi} ({kullanici.Email})"
            });

            _context.Kullanicilar.Remove(kullanici);
            await _context.SaveChangesAsync();

            return new ResultDto { Success = true, Message = "Kullanıcı başarıyla silindi ✅" };
        }




        public async Task<List<Kullanici>> GetAllAsync()
        {
            return await _context.Kullanicilar.ToListAsync();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        public async Task<List<Kullanici>> FilterAsync(UserFilterDto dto)
        {
            var query = _context.Kullanicilar.AsQueryable();

            if (!string.IsNullOrEmpty(dto.KullaniciAdi))
                query = query.Where(x => x.KullaniciAdi.ToLower().Contains(dto.KullaniciAdi.ToLower()));

            if (!string.IsNullOrEmpty(dto.Email))
                query = query.Where(x => x.Email.ToLower().Contains(dto.Email.ToLower()));

            if (!string.IsNullOrEmpty(dto.Rol))
            {
                query = query.AsEnumerable()
                    .Where(x=>x.Rol.ToString().ToLower()==dto.Rol.ToString().ToLower())
                    .AsQueryable();
            }


            return await Task.FromResult(query.ToList());
        }

    }
}
