using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context) { }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
            EmployeeParameters employeeParams, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .FilterEmployees(employeeParams.MinAge, employeeParams.MaxAge)
                .Search(employeeParams.SearchTerm) // search extension
                .Sort(employeeParams.OrderBy) // sorting extension
                .ToListAsync();

            int itemsCount = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .CountAsync();

            return PagedList<Employee>.ToPagedList(employees, itemsCount,
                employeeParams.PageNumber, employeeParams.PageSize);
        }
            

        public async Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
            await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id),
                trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);
    }

    
}
