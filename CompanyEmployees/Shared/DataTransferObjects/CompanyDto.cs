

namespace Shared.DataTransferObjects
{
    /// <summary>
    /// Data transfer object (DTO) for Company 
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="FullAddress"></param>
    public record CompanyDto(
        Guid Id,
        string Name,
        string FullAddress
    );
}
