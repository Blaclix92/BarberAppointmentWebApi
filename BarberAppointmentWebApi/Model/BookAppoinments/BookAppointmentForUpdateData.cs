using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAppointmentWebApi.Model.BookAppoinments
{
    public class BookAppointmentForUpdateData
    {
        public int Cancel { get; set; }
        public string Hour { get; set; }
    }
}
