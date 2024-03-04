using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges) =>
            await FindByCondition(com => com.Id.Equals(companyId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateCompany(Company company) => Create(company);

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(com => ids.Contains(com.Id), trackChanges)
            .ToListAsync();

        public void DeleteCompany(Company company) => Delete(company);
    }

    
}
