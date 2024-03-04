using Contracts;
using Service.Contracts;
using AutoMapper;
using Shared.DataTransferObjects;
using Entities.Exceptions;
using Entities;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<EmployeeDto> _dataShaper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger,
            IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        public async Task<(IEnumerable<ExpandoObject> employees, PageMetadata metadata)>
            GetEmployeesAsync(
            Guid companyId, EmployeeParameters employeeParams, bool trackChanges)
        {
            if (!employeeParams.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();
            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employeesWithPageMetadata = await _repository.Employee.GetEmployeesAsync(
                companyId, employeeParams, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithPageMetadata);

            var shapedData = _dataShaper.ShapeData(employeesDto,
                employeeParams.Fields);

            return (employees: shapedData, metadata: employeesWithPageMetadata.PageMetadata);
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto
            employeeForCreationDto, bool trackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            return _mapper.Map<EmployeeDto>(employeeEntity);
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employee = await GetEmployeeForCompanyAndCheckIfItExists(
                companyId, id, trackChanges);

            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();
        }

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
            EmployeeForUpdateDto employeeForUpdate,
            bool compTrackChanges, bool empTrackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(
                companyId, id, empTrackChanges);

            // employeeEntity state is set to Modified from the empTrackChanges
            // The mapper maps any change to the employeeEntity from the dto
            _mapper.Map(employeeForUpdate, employeeEntity);
            await _repository.SaveAsync();
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> 
            GetEmployeeForPatchAsync(
            Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges
            )
        {
            await GetCompanyAndCheckIfItExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(
                companyId, id, empTrackChanges);

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            return (employeeToPatch, employeeEntity);
        }

        public async Task SaveChangesForPatchAsync(
            EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }

        /// <summary>
        /// Get employee or throw an exception
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="EmployeeNotFoundException"></exception>
        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(
            Guid companyId, Guid id, bool empTrackChanges
            )
        {
            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges) ??
                throw new EmployeeNotFoundException(id);

            return employee;
        }

        /// <summary>
        /// Get a company or throw an exception
        /// </summary>
        /// <param name="id"></param>
        /// <param name="compTrackChanges"></param>
        /// <returns></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool compTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, compTrackChanges) ??
                throw new CompanyNotFoundException(id);

            return company;
        }
    }
}
