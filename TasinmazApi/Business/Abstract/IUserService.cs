using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Business.Abstract
{
    public interface IUserService
    {
        Task<ResultDto> AddAsync(UserCreateDto dto);
        Task<ResultDto> UpdateAsync(UserUpdateDto dto);
        Task<ResultDto> DeleteAsync(int id);
        Task<List<Kullanici>> GetAllAsync();

        Task<List<Kullanici>> FilterAsync(UserFilterDto dto);
    }
}
