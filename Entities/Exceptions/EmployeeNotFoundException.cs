﻿

namespace Entities.Exceptions
{
    public sealed class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employeeId) :
            base($"The employee with the id: {employeeId}, doesn't exist " +
                $"in the database.")
        { }
    }
}
