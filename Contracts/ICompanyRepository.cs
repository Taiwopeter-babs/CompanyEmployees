using Entities;

namespace Contracts
{
    /// <summary>
    /// Provides the CompanyRepository abstraction
    /// </summary>
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
        Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges);
        void CreateCompany(Company company);
        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCompany(Company company);
    }
}
