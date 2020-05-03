using BarberAppointmentWebApi.Model.BookAppoinments;
using BarberAppointmentWebApi.Model.ClientProfiles;
using BarberAppointmentWebApi.Model.CustomWorkDays;
using BarberAppointmentWebApi.Model.WorkDays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAppointmentWebApi.Model.BarberProfiles
{
    public class BarberProfile
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; } = new List<WorkDay>();
        public ICollection<CustomWorkDay> CustomWorkDays { get; set; } = new List<CustomWorkDay>();
        public ICollection<BookAppointment> BookAppoinments { get; set; } = new List<BookAppointment>();
        public ICollection<ClientProfile> ClientProfiles { get; set; } = new List<ClientProfile>();
        public int UserId { get; set; }
    }
}
