using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarberAppointmentWebApi.Model.AppointmentHour;

namespace BarberAppointmentWebApi.Model.WorkDays
{
    public class WorkDay
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public int NumberOfAppointmentHours { get { return AppointmentHours.Count; } }
        public ICollection<AppointmentHour.AppointmentHour> AppointmentHours { get; set; } = new List<AppointmentHour.AppointmentHour>();
    }
}
