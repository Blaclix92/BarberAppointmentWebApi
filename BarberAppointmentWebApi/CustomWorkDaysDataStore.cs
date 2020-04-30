﻿using BarberAppointmentWebApi.Model.CustomAppointmentHours;
using BarberAppointmentWebApi.Model.CustomWorkDays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAppointmentWebApi
{
    public class CustomWorkDaysDataStore
    {
        public static CustomWorkDaysDataStore Current { get; } = new CustomWorkDaysDataStore();
        public List<CustomWorkDay> Days { get; set; }
        public CustomWorkDaysDataStore()
        {
            //init dummy data
            Days = new List<CustomWorkDay>()
            {
                new CustomWorkDay
                {   Id=1,
                    Date ="12-02-2019",
                    OffDay = 0,
                    CustomAppointmentHour = new List<CustomAppointmentHour>() {
                        new CustomAppointmentHour(){ Id= 1, Visible=1, Hour="11:00 AM"},
                        new CustomAppointmentHour(){ Id= 2, Visible=1, Hour="12:00 PM"},
                        new CustomAppointmentHour(){ Id= 3, Visible=1, Hour="01:00 PM"}
                } },
                new CustomWorkDay{
                    Id =2,
                    Date ="25-02-2019",
                    OffDay = 0,
                    CustomAppointmentHour = new List<CustomAppointmentHour>() {
                        new CustomAppointmentHour(){ Id= 1, Visible=1, Hour="11:00 AM"},
                        new CustomAppointmentHour(){ Id= 2, Visible=1, Hour="12:00 PM"},
                        new CustomAppointmentHour(){ Id= 3, Visible=1, Hour="01:00 PM"}
                } },
                new CustomWorkDay{
                    Id =3,
                    Date ="25-03-2019",
                    OffDay = 0,
                 CustomAppointmentHour = new List<CustomAppointmentHour>() {
                        new CustomAppointmentHour(){ Id= 1, Visible=1, Hour="11:00 AM"},
                        new CustomAppointmentHour(){ Id= 2, Visible=1, Hour="12:00 PM"},
                        new CustomAppointmentHour(){ Id= 3, Visible=1, Hour="01:00 PM"}
                } },
            };
        }
    }
}
