using Tabeeb.Domain.Common;
using System;

namespace Tabeeb.Domain.Entities
{
    public class PatientVital : BaseAuditableEntity<long>
    {
        // ربط المؤشرات الحيوية بالزيارة/الحجز الحالي
        public long ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;

        //// ربط المؤشرات بالمريض نفسه لتسهيل جلب التاريخ الطبي له مباشرة
        //public long CustomerId { get; set; }
        //public Customer Customer { get; set; } = null!;

        // 1. درجة الحرارة (درجة مئوية - مثل: 37.5)
        public decimal? Temperature { get; set; }

        // 2. ضغط الدم (مثال: 120/80)
        public int? SystolicBP { get; set; }  // الضغط الانقباضي (العالي)
        public int? DiastolicBP { get; set; } // الضغط الانبساطي (الواطي)

        // 3. نبضات القلب (نبضة/دقيقة)
        public int? HeartRate { get; set; }


        // 5. نسبة الأكسجين في الدم (SpO2 - نسبة مئوية)
        public int? OxygenSaturation { get; set; }

        // 6. نسبة السكر في الدم (مجم/ديسيلتر)
        public decimal? BloodSugar { get; set; }

        // 7. القياسات البدنية
        public decimal? Weight { get; set; } // الوزن بالكيلوجرام
        public decimal? Height { get; set; } // الطول بالسنتيمتر

        // مؤشر كتلة الجسم (BMI) - يمكن حسابه تلقائياً أو حفظه
        public decimal? BMI { get; set; }

        // ملاحظات عامة للممرضة
        public string? Notes { get; set; }
    }
}