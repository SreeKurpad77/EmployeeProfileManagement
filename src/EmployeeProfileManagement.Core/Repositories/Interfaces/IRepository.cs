namespace EmployeeProfileManagement.Core.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<bool> DeleteAsync(int id);

    }
}
