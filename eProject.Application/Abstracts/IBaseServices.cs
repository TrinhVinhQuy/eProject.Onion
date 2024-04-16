﻿namespace eProject.Application.Abstracts
{
    public interface IBaseServices<T> where T : class 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<T> InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
    }
}
