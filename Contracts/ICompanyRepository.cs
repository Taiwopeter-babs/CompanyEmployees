using Entities;

namespace Contracts
{
    /// <summary>
    /// Provides the CompanyRepository abstraction
    /// </summary>
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
    }
}
