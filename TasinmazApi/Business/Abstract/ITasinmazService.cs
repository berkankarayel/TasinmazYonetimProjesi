using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Business.Abstract
{
    public interface ITasinmazService
    {
        Task<List<Tasinmaz>> GetAllAsync(string rol, int kullaniciId);
        Task<Tasinmaz> AddAsync(Tasinmaz tasinmaz, int kullaniciId);
        Task<Tasinmaz> UpdateAsync(Tasinmaz tasinmaz, int kullaniciId);
        Task<bool> DeleteAsync(int id, int kullaniciId);

        Task<Tasinmaz> UpdateAsync(TasinmazUpdateDto dto, int kullaniciId); // yeni eklenecek satır
        Task<List<Tasinmaz>> FilterAsync(string rol, int kullaniciId, TasinmazFilterDto dto);

    }
}
