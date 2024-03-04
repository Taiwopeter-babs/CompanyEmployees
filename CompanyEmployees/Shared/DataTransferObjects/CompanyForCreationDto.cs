

using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto
    {
        [Required(ErrorMessage = "Company Name is a required field")]
        [MaxLength(60, ErrorMessage = "Maximum length for Name is 60 characters")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Company Address is a required field")]
        [MaxLength(60, ErrorMessage = "Maximum length for Address is 60 characters")]
        public string? Address { get; init; }

        [Required(ErrorMessage = "Country is a required field")]
        public string? Country { get; init; }

        public IEnumerable<EmployeeForCreationDto>? Employees;
    };
}
