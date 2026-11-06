using Tabeeb.Domain.Common;
using Tabeeb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class ClinicImage:BaseAuditableEntity<long>
    {
        public string ImagePath { get; set; } = null!;

        public int ClinicId {  get; set; }


        public ImageType ImageType { get; set; }

        public Clinic Clinics { get; set; } = null!;
    }
}
