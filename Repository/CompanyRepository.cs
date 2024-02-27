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

        public Company? GetCompany(Guid companyId, bool trackChanges) =>
            FindByCondition(com => com.Id.Equals(companyId), trackChanges)
            .SingleOrDefault();

        public void CreateCompany(Company company) => Create(company);

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
            FindByCondition(com => ids.Contains(com.Id), trackChanges)
            .ToList();

        public void DeleteCompany(Company company) => Delete(company);
    }

    
}
