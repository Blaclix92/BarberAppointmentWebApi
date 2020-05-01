using BarberAppointmentWebApi.Model.BarberProfiles;
using BarberAppointmentWebApi.Model.BookAppoinments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAppointmentWebApi.Model.ClientProfiles
{
    public class ClientProfile
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<BookAppointment> BookAppoinments { get; set; } = new List<BookAppointment>();
        public BarberProfile BarberProfile { get; set; } = new BarberProfile();
        public int UserId { get; set; }
    }
}
