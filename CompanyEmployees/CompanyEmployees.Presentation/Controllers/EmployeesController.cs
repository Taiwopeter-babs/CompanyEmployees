using ActionFilters;
using Entities.ErrorModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;


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
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
            [FromQuery] EmployeeParameters employeeParams)
        {
            var pagedResult = await _service.EmployeeService.
                GetEmployeesAsync(companyId, employeeParams, trackChanges: false);

            // set X-Pagination header
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(pagedResult.metadata));

            return Ok(pagedResult.employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeAsync(Guid companyId, Guid id) 
        {
            var employee = await _service.EmployeeService
                 .GetEmployeeAsync(companyId, id, trackChanges: false);

            return Ok(employee);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody]
        EmployeeForCreationDto employee)
        {
            var employeeToReturn = await _service.EmployeeService
                .CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany",
                new { companyId, Id = employeeToReturn.Id },
                employeeToReturn
                );
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            await _service.EmployeeService
                .DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] EmployeeForUpdateDto employee)
        {

            await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee,
                compTrackChanges: false, empTrackChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(
            Guid companyId, Guid id,
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc
            )
        {
            ErrorDetails error = new()
            {
                StatusCode = 400,
                Message = "PatchDoc sent from client is null"
            };

            if (patchDoc is null)
                return BadRequest(error);

            foreach (var prop in patchDoc.GetType().GetProperties())
                Console.WriteLine(prop);

            var (employeeToPatch, employeeEntity) = await _service.EmployeeService
                .GetEmployeeForPatchAsync(
                companyId, id, compTrackChanges: false, empTrackChanges: true);

            patchDoc.ApplyTo(employeeToPatch);

            await _service.EmployeeService.SaveChangesForPatchAsync(employeeToPatch,
                employeeEntity);

            return NoContent();
        }
    }
}
