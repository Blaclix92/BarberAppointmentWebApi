﻿using BarberAppointmentWebApi.Model;
using BarberAppointmentWebApi.Model.WorkDays;
using BarberAppointmentWebApi.Model.AppointmentHour;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BarberAppointmentWebApi.Controller
{
    
    [ApiController]
    [Authorize]
    public class WorkDayController : ControllerBase
    {
        [Route("api/workdays")]
        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult GetWorkDays()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            return Ok(WorkDaysDataStore.Current.Days);
        }

        [Route("api/client/workdays")]
        [HttpGet]
        public IActionResult GetWorkDaysForClient()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int userId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            string role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("admin"))
            {
                int barberId = role == "barber" ? userId : 2; //TODO: replace (2) with barber id from database
                List<WorkDay> workdays = WorkDaysDataStore.Current.Days.Where(wd => wd.BarberId == barberId).ToList();
                if (workdays.Count == 0 || workdays == null)
                {
                    return NotFound();
                }
                return Ok(workdays);

            }
            return Unauthorized();
           
        }

        [Route("api/barber/workdays")]
        [HttpGet("{id}", Name = "GetWorkDayByIdForBarber")]
        [Authorize(Roles ="barber")]
        public IActionResult GetWorkDayByIdForBarber(int id)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int barberId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id && wd.BarberId == barberId);
            if (workDay == null)
            {
                return NotFound();
            }
            return Ok(workDay);
        }

        [Route("api/client/workdays")]
        [HttpGet("{id}", Name = "GetWorkDayByIdForClient")]
        [Authorize(Roles ="client")]
        public IActionResult GetWorkDayByIdForClient(int id)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int clientId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            int barberId = 2; //TODO: replace (2) with barber id from database
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id && wd.BarberId == barberId);
            if (workDay == null)
            {
                return NotFound();
            }
            return Ok(workDay);
        }

        [Route("api/admin/workdays")]
        [HttpGet("{id}", Name = "GetWorkDayByIdForAdmin")]
        public IActionResult GetWorkDayByIdForAdmin(int id)
        {
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }
            return Ok(workDay);
        }
        [Route("api/workdays")]
        [HttpPost]
        public IActionResult CreateWorkDay([FromBody] WorkDayForCreateData data)
        {
            ClaimsPrincipal claimsPrincipal = User as ClaimsPrincipal;
            string role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("client"))
            {
                var maxWorkDayId = WorkDaysDataStore.Current.Days.Max(wd => wd.Id);
                var newWorkDay = new WorkDay()
                {
                    Id = ++maxWorkDayId,
                    Day = data.Day,
                    BarberId = data.BarberId,
                    AppointmentHours = data.AppointmentHours
                };
                WorkDaysDataStore.Current.Days.Add(newWorkDay);
                return CreatedAtRoute(role.Equals("admin") ? "GetWorkDayByIdForAdmin" : "GetWorkDayByIdForBarber", new { newWorkDay.Id }, newWorkDay);
            }
            return Unauthorized();
        }

        [Route("api/workdays")]
        [HttpPut()]
        public IActionResult UpdateWorkDay([FromBody] WorkDayForUpdateData data)
        {
            ClaimsPrincipal claimsPrincipal = User as ClaimsPrincipal;
            int barberId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            string role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("client"))
            {
                WorkDay workDay = role.Equals("admin")? WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == data.Id):
                    WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == data.Id && wd.BarberId == barberId);
                if (workDay == null)
                {
                    return NotFound();
                }

                workDay.Day = data.Day;
                return NoContent();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchWorkDay(int id, [FromBody] JsonPatchDocument<WorkDayForUpdateData> patchDoc)
        {
            ClaimsPrincipal claimsPrincipal = User as ClaimsPrincipal;
            string role = claimsPrincipal.FindFirst("role").Value;
            int barberId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            if (!role.Equals("client"))
            {
                WorkDay workDay = role.Equals("admin") ? WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id):
                    WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id && wd.BarberId == barberId);
                if (workDay == null)
                {
                    return NotFound();
                }
                WorkDayForUpdateData patchWorkDay = new WorkDayForUpdateData()
                {
                    Day = workDay.Day
                };

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                patchDoc.ApplyTo(patchWorkDay, ModelState);

              
                workDay.Day = patchWorkDay.Day;
                return NoContent();
            }
            return Unauthorized();
        }

        [Route("api/workdays")]
        [HttpDelete("{id}")]
        public IActionResult DeleteWorkDay(int id)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var role = claimsPrincipal.FindFirst("role").Value;
            int barberId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            if (!role.Equals("client"))
            {
                WorkDay workDay = role.Equals("admin") ? WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id):
                    WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id && wd.BarberId == barberId);
                if (workDay == null)
                {
                    return NotFound();
                }
                WorkDaysDataStore.Current.Days.Remove(workDay);
                return NoContent();
            }
            return Unauthorized();
        }
    }
}
