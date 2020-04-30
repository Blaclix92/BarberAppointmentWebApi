using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarberAppointmentWebApi.Model.CustomWorkDays;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BarberAppointmentWebApi.Controller
{
    [ApiController]
    [Route("api/customworkdays")]
    public class CustomWorkDayController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCustomWorkDays()
        {
            return Ok(CustomWorkDaysDataStore.Current.Days);
        }

        [HttpGet("{id}", Name = "GetCustomWorkDayById")]
        public IActionResult GetCustomWorkDayById(int id)
        {
            CustomWorkDay workDay = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }
            return Ok(workDay);
        }

        [HttpPost]
        public IActionResult CreateCustomWorkDay([FromBody] CustomWorkDayForCreateData data)
        {
            var maxCustomWorkDayId = WorkDaysDataStore.Current.Days.Max(wd => wd.Id);
            var newCustomWorkDay = new CustomWorkDay()
            {
                Id = ++maxCustomWorkDayId,
                OffDay = data.OffDay,
                Date = data.Date,
                CustomAppointmentHour = data.CustomAppointmentHour
            };
            CustomWorkDaysDataStore.Current.Days.Add(newCustomWorkDay);
            return CreatedAtRoute("GetCustomWorkDayById", new { newCustomWorkDay.Id }, newCustomWorkDay);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWorkDay(int id, [FromBody] CustomWorkDayForUpdateData data)
        {
            CustomWorkDay customworkDay = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (customworkDay == null)
            {
                return NotFound();
            }
            customworkDay.OffDay = data.OffDay;
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchWorkDay(int id, [FromBody] JsonPatchDocument<CustomWorkDayForUpdateData> patchDoc)
        {
            CustomWorkDay customWorkDay = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (customWorkDay == null)
            {
                return NotFound();
            }
            CustomWorkDayForUpdateData patchCustomWorkDay = new CustomWorkDayForUpdateData()
            {
                OffDay = customWorkDay.OffDay,
            };

            patchDoc.ApplyTo(patchCustomWorkDay, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            customWorkDay.OffDay = patchCustomWorkDay.OffDay;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWorkDay(int id)
        {
            CustomWorkDay customWorkDay = CustomWorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (customWorkDay == null)
            {
                return NotFound();
            }
            CustomWorkDaysDataStore.Current.Days.Remove(customWorkDay);
            return NoContent();
        }
    }
}