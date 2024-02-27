

namespace Shared.DataTransferObjects
{
    /// <summary>
    /// Data transfer object (DTO) for Company 
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="FullAddress"></param>

    public record CompanyDto 
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    };
}
