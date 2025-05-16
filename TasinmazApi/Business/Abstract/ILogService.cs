using TasinmazApi.Dtos;
using TasinmazApi.Entities.Concrete;

namespace TasinmazApi.Business.Abstract
{
    public interface ILogService
    {
        Task LogEkleAsync(Log log);

        Task<List<Log>> FilterAsync(LogFilterDto dto);


    }
}
