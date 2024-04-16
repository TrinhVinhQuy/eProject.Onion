using AutoMapper;
using eProject.Application.Abstracts;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;

namespace eProject.Application.Services
{
    public class BaseServices<T> : IBaseServices<T> where T : class 
    {
        private readonly IRepositoryBase<T> _repository;

        public BaseServices(IRepositoryBase<T> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T> InsertAsync(T entity)
        {
            return await _repository.InsertAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(object id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
