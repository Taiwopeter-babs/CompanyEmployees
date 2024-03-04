using Entities;
using Shared.RequestFeatures;

namespace Contracts
{
    /// <summary>
    /// Provides the EmployeeRepository Abstraction
    /// </summary>
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
            EmployeeParameters employeeParams, bool trackChanges);
        Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
