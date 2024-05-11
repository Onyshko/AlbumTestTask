using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Interfaces;
using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Models;
using AutoMapper;

namespace AlbumTestTask.Services.Implementation
{
    public class BaseCrudService<TEntity, TModel> : IBaseCrudService<TModel>
        where TModel : BaseModel
        where TEntity : BaseEntity
    {
        protected readonly IBaseRepository<TEntity> _repo;
        protected readonly IMapper _mapper;

        public BaseCrudService(IBaseRepository<TEntity> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public virtual async Task<int> AddAsync(TModel model)
        {
            return await _repo.AddAsync(_mapper.Map<TEntity>(model));
        }

        public virtual async Task<TModel> GetAsync(int id)
        {
            var entity = await _repo.GetAsync(id);
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task<IList<TModel>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<List<TModel>>(entities);
        }

        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            var entity = await _repo.UpdateAsync(_mapper.Map<TEntity>(model));
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
