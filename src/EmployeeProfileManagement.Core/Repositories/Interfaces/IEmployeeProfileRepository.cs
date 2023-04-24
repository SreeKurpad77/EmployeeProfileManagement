using EmployeeProfileManagement.Core.Model;

namespace EmployeeProfileManagement.Core.Repositories.Interfaces
{
    public interface IEmployeeProfileRepository
    {
        Task<ResultObject<IEnumerable<EmployeeProfile>>> GetAll();
        Task<ResultObject<EmployeeProfile>> GetById(int id);
        Task<ResultObject<EmployeeProfile>> Add(EmployeeProfile entity);
        Task<ResultObject<EmployeeProfile>> Update(EmployeeProfile entity);
        Task<ResultObject<bool>> Delete(int id);

    }
}
