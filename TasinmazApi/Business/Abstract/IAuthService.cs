using System.Threading.Tasks;
using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Business.Abstract
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(Kullanici kullanici);
        Task<string> LoginAsync(KullaniciLoginDto dto);
        string HashPassword(string password);
        string GenerateJwtToken(Kullanici kullanici);
    }
}


