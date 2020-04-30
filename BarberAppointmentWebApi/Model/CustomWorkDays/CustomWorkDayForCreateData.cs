﻿using BarberAppointmentWebApi.Model.CustomAppointmentHours;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAppointmentWebApi.Model.CustomWorkDays
{
    public class CustomWorkDayForCreateData
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public int OffDay { get; set; }
        [Required(ErrorMessage = "You should provide a collection of appointment hour values.")]
        public ICollection<CustomAppointmentHour> CustomAppointmentHour { get; set; } = new List<CustomAppointmentHour>();
    }
}
