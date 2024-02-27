

namespace Entities.Exceptions
{
    public sealed class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid companyId) : 
            base($"The company with the id: {companyId}, doesn't exist " +
                $"in the database.") { }
    }
}
