﻿

namespace Entities.Exceptions
{
    public sealed class CompanyCollectionBadRequest : BadRequestException
    {
        public CompanyCollectionBadRequest() :
            base("Company collection sent is null") { }
    }
}
