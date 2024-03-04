namespace Entities.Exceptions
{
    public class MaxAgeRangeBadRequestException : BadRequestException
    {
        public MaxAgeRangeBadRequestException() : 
            base("Max age cannot be less than minimum age") { }
    }
}
