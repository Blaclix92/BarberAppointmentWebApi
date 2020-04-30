using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarberAppointmentWebApi.Model.CustomAppointmentHours;
using BarberAppointmentWebApi.Model.CustomWorkDays;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BarberAppointmentWebApi.Controller
{
    [Route("api/customworkdays/{customworkdayId}/customappointmenthours")]
    [ApiController]
    public class CustomAppointmentHourController : ControllerBase
    {
        [HttpGet()]
        public IActionResult GetAppointmentHours(int customworkdayId)
        {
            CustomWorkDay customworkDay = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == customworkdayId);
            if (customworkDay == null)
            {
                return NotFound();
            }
            return Ok(customworkDay.CustomAppointmentHour);
        }

        [HttpGet("{id}", Name = "GetCustomAppointmentHourById")]
        public IActionResult GetCustomAppointmentHourById(int customworkdayId, int id)
        {
            CustomAppointmentHour customAppointmentHour = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == customworkdayId).CustomAppointmentHour.FirstOrDefault(ah => ah.Id == id);
            if (customAppointmentHour == null)
            {
                return NotFound();
            }
            return Ok(customAppointmentHour);
        }

        [HttpPost()]
        public IActionResult CreateCustomAppointmentHour(int customworkdayId, [FromBody] CustomAppointmentHourForCreateData data)
        {
            CustomWorkDay customWorkday = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == customworkdayId);
            if (customWorkday == null)
            {
                return NotFound();
            }
            int maxCustomAppointmentHourId = CustomWorkDaysDataStore.Current.Days.SelectMany(wd => wd.CustomAppointmentHour).Max(ah => ah.Id);

            CustomAppointmentHour newCustomAppointmentHour = new CustomAppointmentHour()
            {
                Id = ++maxCustomAppointmentHourId,
                Visible = data.Visible,
                Hour = data.Hour
            };
            customWorkday.CustomAppointmentHour.Add(newCustomAppointmentHour);
            return CreatedAtRoute("GetCustomAppointmentHourById", new { customworkdayId, id = newCustomAppointmentHour.Id }, newCustomAppointmentHour);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAppointmentHour(int customworkdayId, int id, [FromBody] CustomAppointmentHourForUpdateData data)
        {
            CustomAppointmentHour customAppointmentHour = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == customworkdayId).CustomAppointmentHour.FirstOrDefault(ah => ah.Id == id);
            if (customAppointmentHour == null)
            {
                return NotFound();
            }
            customAppointmentHour.Hour = data.Hour;
            customAppointmentHour.Visible = data.Visible;
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAppointmentHour(int customworkdayId, int id, [FromBody] JsonPatchDocument<CustomAppointmentHourForUpdateData> patchDoc)
        {
            CustomAppointmentHour appointmentHour = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == customworkdayId).CustomAppointmentHour.FirstOrDefault(ah => ah.Id == id);
            if (appointmentHour == null)
            {
                return NotFound();
            }
            CustomAppointmentHourForUpdateData patchData = new CustomAppointmentHourForUpdateData()
            {
                Hour = appointmentHour.Hour,
                Visible = appointmentHour.Visible
            };

            patchDoc.ApplyTo(patchData, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            appointmentHour.Hour = patchData.Hour;
            appointmentHour.Visible = patchData.Visible;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAppointmentHourById(int customworkdayId, int id)
        {
            CustomAppointmentHour customAppointmentHour = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == customworkdayId).CustomAppointmentHour.FirstOrDefault(ah => ah.Id == id);
            if (customAppointmentHour == null)
            {
                return NotFound();
            }
            CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == customworkdayId).CustomAppointmentHour.Remove(customAppointmentHour);
            return NoContent();
        }
    }
}