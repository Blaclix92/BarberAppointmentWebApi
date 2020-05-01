using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarberAppointmentWebApi.Model.WorkDays;
using BarberAppointmentWebApi.Model.AppointmentHour;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BarberAppointmentWebApi.Controller
{
    [ApiController]
    [Route("api/workdays/{workdayId}/appointmenthours")]
    [Authorize]
    public class AppointmentHourController : ControllerBase
    {
        [HttpGet()]
        public IActionResult GetAppointmentHours(int workdayId)
        {
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == workdayId);
            if (workDay == null)
            {
                return NotFound();
            }
            return Ok(workDay.AppointmentHours);
        }

        [HttpPost()]
        public IActionResult CreateAppointmentHour(int workdayId, [FromBody] AppointmentHourForCreate data)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("client"))
            {
                WorkDay day = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == workdayId);
                if (day == null)
                {
                    return NotFound();
                }
                var maxAppointmentHourId = WorkDaysDataStore.Current.Days.SelectMany(wd => wd.AppointmentHours).Max(ah => ah.Id);

                var newAppointmentHour = new AppointmentHour()
                {
                    Id = ++maxAppointmentHourId,
                    Hour = data.Hour
                };
                day.AppointmentHours.Add(newAppointmentHour);
                return CreatedAtRoute("GetAppointmentHourById", new { workdayId, id = newAppointmentHour.Id }, newAppointmentHour);
            }
            return Unauthorized();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAppointmentHour(int workdayId, int id, [FromBody] AppointmentHourForUpdateData data)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("client"))
            {
                AppointmentHour appointmentHour = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == workdayId).AppointmentHours.FirstOrDefault(ah => ah.Id == id);
                if (appointmentHour == null)
                {
                    return NotFound();
                }
                appointmentHour.Hour = data.Hour;
                return NoContent();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAppointmentHour(int workdayId, int id, [FromBody] JsonPatchDocument<AppointmentHourForUpdateData> patchDoc)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("client"))
            {
                AppointmentHour appointmentHour = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == workdayId).AppointmentHours.FirstOrDefault(ah => ah.Id == id);
                if (appointmentHour == null)
                {
                    return NotFound();
                }
                var patchData = new AppointmentHourForUpdateData()
                {
                    Hour = appointmentHour.Hour
                };

                patchDoc.ApplyTo(patchData, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                appointmentHour.Hour = patchData.Hour;
                return NoContent();
            }
            return Unauthorized();
        }

        [HttpGet("{id}", Name = "GetAppointmentHourById")]
        public IActionResult GetAppointmentHourById(int workdayId, int id)
        {
            AppointmentHour appointmentHour = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == workdayId).AppointmentHours.FirstOrDefault(ah => ah.Id == id);
            if (appointmentHour == null)
            {
                return NotFound();
            }
            return Ok(appointmentHour);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAppointmentHourById(int workdayId, int id)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("client"))
            {
                AppointmentHour appointmentHour = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == workdayId).AppointmentHours.FirstOrDefault(ah => ah.Id == id);
                if (appointmentHour == null)
                {
                    return NotFound();
                }
                WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == workdayId).AppointmentHours.Remove(appointmentHour);
                return NoContent();
            }
            return Unauthorized();
        }
    }
}