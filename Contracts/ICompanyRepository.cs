using Entities;

namespace Contracts
{
    /// <summary>
    /// Provides the CompanyRepository abstraction
    /// </summary>
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company? GetCompany(Guid companyId, bool trackChanges);
        void CreateCompany(Company company);
        IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCompany(Company company);
    }
}
