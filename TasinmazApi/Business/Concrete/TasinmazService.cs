using Microsoft.EntityFrameworkCore;
using TasinmazApi.Business.Abstract;
using TasinmazApi.DataAccess;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Business.Concrete
{
    public class TasinmazService : ITasinmazService
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogService _logService;

        public TasinmazService(ApplicationDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public async Task<List<Tasinmaz>> GetAllAsync(string rol, int kullaniciId)
        {
            if (rol == "Admin")
            {
                return await _context.Tasinmazlar
                    .Include(x => x.Mahalle).ThenInclude(m => m.Ilce).ThenInclude(i => i.Il)
                    .Include(x => x.Kullanici)
                    .OrderBy(t => t.TasinmazId) 
                    .ToListAsync();
            }
            // Kulanıcı 
            return await _context.Tasinmazlar
                .Where(t => t.KullaniciId == kullaniciId)
                .Include(x => x.Mahalle).ThenInclude(m => m.Ilce).ThenInclude(i => i.Il)
                .Include(x => x.Kullanici)
                .OrderBy(t => t.TasinmazId) 
                .ToListAsync();
        }


        public async Task<Tasinmaz> AddAsync(Tasinmaz tasinmaz, int kullaniciId)
        {
            try
            {
                tasinmaz.KullaniciId = kullaniciId;
                _context.Tasinmazlar.Add(tasinmaz);
                await _context.SaveChangesAsync();

                await _logService.LogEkleAsync(new Log
                {
                    KullaniciId = kullaniciId,
                    IslemTipi = "Ekleme",
                    Durum = "Başarılı",
                    Aciklama = $"Yeni taşınmaz eklendi. Ada: {tasinmaz.Ada}, Parsel: {tasinmaz.Parsel}, Nitelik: {tasinmaz.Nitelik}"

                });

                return tasinmaz;
            }
            catch (Exception ex)
            {
                await _logService.LogEkleAsync(new Log
                {
                    KullaniciId = kullaniciId,
                    IslemTipi = "Ekleme",
                    Durum = "Başarısız",
                    Aciklama = "Hata: " + ex.Message
                });
                return null;
            }
        }

        public async Task<Tasinmaz> UpdateAsync(TasinmazUpdateDto dto, int kullaniciId)
        {
            var existing = await _context.Tasinmazlar.FirstOrDefaultAsync(t => t.TasinmazId == dto.TasinmazId && t.KullaniciId == kullaniciId);
            if (existing == null)
            {
                await _logService.LogEkleAsync(new Log
                {
                    KullaniciId = kullaniciId,
                    IslemTipi = "Güncelleme",
                    Durum = "Başarısız",
                    Aciklama = "Taşınmaz bulunamadı veya yetki yok."
                });
                return null;
            }

            existing.Ada = dto.Ada;
            existing.Parsel = dto.Parsel;
            existing.Nitelik = dto.Nitelik;
            existing.MahalleId = dto.MahalleId;
            existing.IlId = dto.IlId;
            existing.IlceId = dto.IlceId;
            existing.Koordinat = dto.Koordinat;

            await _context.SaveChangesAsync();

            await _logService.LogEkleAsync(new Log
            {
                KullaniciId = kullaniciId,
                IslemTipi = "Güncelleme",
                Durum = "Başarılı",
                Aciklama = $"Taşınmaz güncellendi. Ada: {existing.Ada} → {dto.Ada}, Parsel: {existing.Parsel} → {dto.Parsel}, Nitelik: {existing.Nitelik} → {dto.Nitelik}"
            });

            return existing;
        }



        public async Task<bool> DeleteAsync(int id, int kullaniciId)
        {
            try
            {
                var entity = await _context.Tasinmazlar.FirstOrDefaultAsync(t => t.TasinmazId == id && t.KullaniciId == kullaniciId);
                if (entity == null)
                {
                    await _logService.LogEkleAsync(new Log
                    {
                        KullaniciId = kullaniciId,
                        IslemTipi = "Silme",
                        Durum = "Başarısız",
                        Aciklama = "Taşınmaz bulunamadı veya yetki yok."
                    });
                    return false;
                }

                _context.Tasinmazlar.Remove(entity);
                await _context.SaveChangesAsync();

                await _logService.LogEkleAsync(new Log
                {
                    KullaniciId = kullaniciId,
                    IslemTipi = "Silme",
                    Durum = "Başarılı",
                    Aciklama = $"Taşınmaz silindi. ID: {entity.TasinmazId}, Ada: {entity.Ada}, Parsel: {entity.Parsel}, Nitelik: {entity.Nitelik}"
                });

                return true;
            }
            catch (Exception ex)
            {
                await _logService.LogEkleAsync(new Log
                {
                    KullaniciId = kullaniciId,
                    IslemTipi = "Silme",
                    Durum = "Başarısız",
                    Aciklama = "Hata: " + ex.Message
                });
                return false;
            }
        }


        public async Task<List<Tasinmaz>> FilterAsync(string rol, int kullaniciId, TasinmazFilterDto dto)
        {
            var query = _context.Tasinmazlar
                .Include(x => x.Mahalle).ThenInclude(m => m.Ilce).ThenInclude(i => i.Il)
                .Include(x => x.Kullanici)
                .AsQueryable();

            if (rol != "Admin")
                query = query.Where(t => t.KullaniciId == kullaniciId);

            if (!string.IsNullOrEmpty(dto.Ada))
                query = query.Where(t => t.Ada.ToLower().Contains(dto.Ada.ToLower()));

            if (!string.IsNullOrEmpty(dto.Parsel))
                query = query.Where(t => t.Parsel.ToLower().Contains(dto.Parsel.ToLower()));

            if (!string.IsNullOrEmpty(dto.Nitelik))
                query = query.Where(t => t.Nitelik.ToLower().Contains(dto.Nitelik.ToLower()));

            if (!string.IsNullOrEmpty(dto.IlAdi))
                query = query.Where(t => t.Mahalle.Ilce.Il.IlAdi.ToLower().Contains(dto.IlAdi.ToLower()));

            if (!string.IsNullOrEmpty(dto.IlceAdi))
                query = query.Where(t => t.Mahalle.Ilce.IlceAdi.ToLower().Contains(dto.IlceAdi.ToLower()));

            if (!string.IsNullOrEmpty(dto.MahalleAdi))
                query = query.Where(t => t.Mahalle.MahalleAdi.ToLower().Contains(dto.MahalleAdi.ToLower()));

            return await query
     .Select(t => new Tasinmaz
     {
         TasinmazId = t.TasinmazId,
         KullaniciId = t.KullaniciId,
         Ada = t.Ada,
         Parsel = t.Parsel,
         Nitelik = t.Nitelik,
         Koordinat = t.Koordinat,
         IlId = t.IlId,
         IlceId = t.IlceId,
         MahalleId = t.MahalleId,
         Kullanici = t.Kullanici,
         Mahalle = new Mahalle
         {
             MahalleAdi = t.Mahalle.MahalleAdi,
             Ilce = new Ilce
             {
                 IlceAdi = t.Mahalle.Ilce.IlceAdi,
                 Il = new Il
                 {
                     IlAdi = t.Mahalle.Ilce.Il.IlAdi
                 }
             }
         }
     })
     .ToListAsync();

        }

        public Task<Tasinmaz> UpdateAsync(Tasinmaz tasinmaz, int kullaniciId)
        {
            throw new NotImplementedException();
        }
    }
}
