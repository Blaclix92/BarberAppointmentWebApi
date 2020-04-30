using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAppointmentWebApi.Model.BookAppoinment
{
    public class BookAppointmentForCreateData
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public string Hour { get; set; }
    }
}
