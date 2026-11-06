using Tabeeb.Domain.Common;
using Tabeeb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Reservation:BaseAuditableEntity<long>
    {

        public long  ReservationNo {  get; set; }
        public DateTime Date {  get; set; }

        //public int ClinicId {  get; set; }


        public long CustomerId {  get; set; }

        public Customer Customer { get; set; }
        public ReservationStatus Status {  get; set; }

        public ReservationType Type { get; set; }

        public decimal TotalAmount {  get; set; }

        public int DoctorID {  get; set; }

        public Doctor Doctor { get; set; } = new();


        public int ClinicId { get; set; }

        public string PaymentIntentId { get; set; }
        public Clinic Clinics { get; set; } 

        public Prescription Prescription { get; set; }


        public Evaluation Evaluation { get; set; }
        public virtual PatientVital PatientVital { get; set; } 


        public int? OfferId { get; set; }


        public Offer Offer { get; set; }




    }
}
