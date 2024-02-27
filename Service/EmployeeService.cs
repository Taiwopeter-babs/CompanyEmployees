using Contracts;
using Service.Contracts;
using AutoMapper;
using Shared.DataTransferObjects;
using Entities.Exceptions;
using Entities;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges) ??
                throw new CompanyNotFoundException(companyId);

            var employees = _repository.Employee.GetEmployees(companyId, trackChanges);
            
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges) ??
                throw new CompanyNotFoundException(companyId);

            var employee = _repository.Employee.GetEmployee(companyId, id, trackChanges) ??
                throw new EmployeeNotFoundException(id);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto
            employeeForCreationDto, bool trackChanges)
        {
            var company  = _repository.Company.GetCompany(companyId, trackChanges) ??
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();

            return _mapper.Map<EmployeeDto>(employeeEntity);
        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employee = _repository.Employee.GetEmployee(companyId, id, trackChanges) ??
                throw new EmployeeNotFoundException(id);

            _repository.Employee.DeleteEmployee(employee);
            _repository.Save();
        }

        public void UpdateEmployeeForCompany(Guid companyId, Guid id,
            EmployeeForUpdateDto employeeForUpdate,
            bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, compTrackChanges) ??
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _repository.Employee.GetEmployee(companyId, id, empTrackChanges) ??
                throw new EmployeeNotFoundException(id);

            // employeeEntity state is set to Modified from the empTrackChanges
            // The mapper maps any change to the employeeEntity from the dto
            _mapper.Map(employeeForUpdate, employeeEntity);
            _repository.Save();
        }
    }
}
