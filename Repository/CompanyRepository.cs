using Contracts;
using Entities;

namespace Repository
{
    /// <summary>
    /// 
    /// concrete implementation of Company repository
    /// </summary>
    public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context)
            : base(context) { }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();
    }

    
}
