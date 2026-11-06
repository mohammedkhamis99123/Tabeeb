using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class ClinicCustomer
    {

        public long CustomerId {  get; set; }

        public int ClinicId {  get; set; }

        public long MedicalFile {  get; set; }


        //public int DoctorId { get; set; }   // الدكتور المسؤول عن العميل


        public Customer Customer { get; set; } = null!;

        //public Doctor Doctor { get; set; } = null!;
        public Clinic Clinics { get; set; } = null!;
    }
}
