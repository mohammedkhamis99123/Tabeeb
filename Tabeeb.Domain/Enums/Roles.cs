using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Enums
{
    public  abstract class Roles
    {
        public const string SuperAdmin = nameof(SuperAdmin);

        public const string Admin = nameof(Admin);

        public const string Employee = nameof(Employee);

        public const string Accountant = nameof(Accountant);

        public const string Doctor = nameof(Doctor);

        public const string DoctorAssistant = nameof(DoctorAssistant);

        public const string Customer = nameof(Customer);




    }
}
