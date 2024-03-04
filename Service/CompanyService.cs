using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;
using AutoMapper;
using Entities.Exceptions;
using Entities;
using System.Collections;
using System.ComponentModel.Design;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
                var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);

                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

                return companiesDto;   
        }

        public async Task<CompanyDto?> GetCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
          
            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            return _mapper.Map<CompanyDto>(companyEntity);

        }

        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            
            var companyEntities  = await _repository.Company.GetByIdsAsync(ids, trackChanges);

            if (companyEntities.Count() != ids.Count())
                throw new CollectionByIdsBadRequestException();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return companiesToReturn;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(
            IEnumerable<CompanyForCreationDto> companyCollection)
        {
            
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach ( var companyEntity in companyEntities)
                _repository.Company.CreateCompany(companyEntity);

            await _repository.SaveAsync();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(',', companiesToReturn.Select(com => com.Id));

            return (companies: companiesToReturn, ids);
        }

        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
        }

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate,
            bool trackChanges)
        {
            var companyEntity = await GetCompanyAndCheckIfItExists(companyId, trackChanges);        

            _mapper.Map(companyForUpdate, companyEntity);
            await _repository.SaveAsync();
        }

        /// <summary>
        /// Get company or throw an exception if it is not found
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trackChanges"></param>
        /// <returns><code>Company</code></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges) ??
                throw new CompanyNotFoundException(id);

            return company;
        }
    }
}
