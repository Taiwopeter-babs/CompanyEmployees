

namespace Entities.Exceptions
{
    public  sealed class IdParametersBadRequestException : BadRequestException
    {
        public IdParametersBadRequestException() : 
            base("ids Parameter is null") { }
    }
}
