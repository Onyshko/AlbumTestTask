using AlbumTestTask.Services.Models;

namespace AlbumTestTask.Services.Interfaces
{
    public interface IBaseCrudService<TModel>
        where TModel : BaseModel
    {
        Task<IList<TModel>> GetAllAsync();

        Task<TModel> GetAsync(int id);

        Task<int> AddAsync(TModel model);

        Task<TModel> UpdateAsync(TModel model);

        Task<bool> DeleteAsync(int id);
    }
}
