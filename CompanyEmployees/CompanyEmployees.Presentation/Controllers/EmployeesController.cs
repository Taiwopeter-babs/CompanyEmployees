using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;


namespace CompanyEmployees.Presentation.Controllers
{
    [ApiController]
    [Route("api/v0/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmployeesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var companyEmployees = _service.EmployeeService.
                GetEmployees(companyId, trackChanges: false);

            return Ok(companyEmployees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployee(Guid companyId, Guid id) 
        {
            var employee = _service.EmployeeService
                 .GetEmployee(companyId, id, trackChanges: false);

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployee(Guid companyId, [FromBody]
        EmployeeForCreationDto employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee object is null");
            }

            var employeeToReturn = _service.EmployeeService
                .CreateEmployeeForCompany(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany",
                new { companyId, Id = employeeToReturn.Id },
                employeeToReturn
                );
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForUpdateDto cannot be null");

            _service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee,
                compTrackChanges: false, empTrackChanges: true);

            return NoContent();
        }
    }
}
