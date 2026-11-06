using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Areas.ClinicDoctor.Command
{
    public  class ClinicDoctorDto
    {
        public int? DoctorId { get; set; }

        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public byte SpecializationId { get; set; }


        public string IdentityNumber { get; set; } = null!;
        public IFormFile Image { get; set; } 

        public IFormFile Stamp { get; set; }
        public string? ImagePath {  get; set; }
        public string? DoctorStamp { get; set; }

        public int SessionDuration { get; set; }

        public decimal SessionPrice { get; set; }

        public List<ClinicDoctorWorktimeDto> worktimeDtos { get; set; }

        public bool IsActive {  get; set; }

        public string Email {  get; set; }
        public DateTime JoinDate { get; set; }

        public int ClinicId {  get; set; }
    }
}
