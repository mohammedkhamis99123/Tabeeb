using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.Employee.Queries
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;



        public string Addrees { get; set; } = null!;


        public bool IsActive { get; set; }

        public int? DoctorId { get; set; }


        public string ImagePath {  get; set; }
        public string IdentityNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

    }
}
