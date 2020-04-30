﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAppointmentWebApi.Model.AppointmentHour
{
    public class AppointmentHourForUpdateData
    {
        [Required(ErrorMessage = "You should provide a hour value.")]
        public string Hour { get; set; }
    }
}
