namespace Tabeeb.Application.Areas.DoctorWorkTime.Commands.UpdateDoctorWorkTime
{
    public  class DoctorWorkTimeInfo
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly? StartAt { get; set; }


        public TimeOnly? EndAt { get; set; }

        public bool IsActive { get; set; }
    }
}