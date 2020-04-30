﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BarberAppointmentWebApi.Model.AppointmentHour;

namespace BarberAppointmentWebApi.Model.WorkDay
{
    public class WorkDayForUpdateData
    {
        [Required(ErrorMessage = "You should provide a day value.")]
        public string Day { get; set; }
    }
}
