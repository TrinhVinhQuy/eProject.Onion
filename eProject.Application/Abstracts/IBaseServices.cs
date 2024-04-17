namespace eProject.Application.Abstracts
{
    public interface IBaseServices<T> where T : class 
    {
        Task<T> InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
