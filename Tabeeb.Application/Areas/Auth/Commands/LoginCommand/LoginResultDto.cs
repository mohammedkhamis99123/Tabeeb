using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Clinic.Queries;
using Tabeeb.Application.Areas.Clinic.Queries.ClinicsDropDown;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.Auth.Commands.LoginCommand.LoginCommand
{
    public  class LoginResultDto
    {
        public string? Token {  get; set; }

        public DateTime? TokenEndAt { get; set; }


        public string? RefreshtokenToken { get; set; }

        public bool RedirectToChooseClinic {  get; set; }

        public bool RedirectToChooseRole { get; set; }

        public DateTime? RefreshTokenEndAt { get; set; }

        public int[] Permissions { get; set; }

        public string[] Roles { get; set; }

        public int ?ClinicID {get; set; }

        public int? DoctorID {get; set; }

        public int? EmployeeID {get; set; }

        public List<ClinicDto>? Clinics { get; set; } = new List<ClinicDto>();

    }
}
