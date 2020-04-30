using BarberAppointmentWebApi.Model;
using BarberAppointmentWebApi.Model.WorkDay;
using BarberAppointmentWebApi.Model.AppointmentHour;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace BarberAppointmentWebApi.Controller
{
    [ApiController]
    [Route("api/workdays")]
    public class WorkDayController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWorkDays()
        {
            return Ok(WorkDaysDataStore.Current.Days);
        }

        [HttpGet("{id}", Name = "GetWorkDayById")]
        public IActionResult GetWorkDayById(int id)
        {
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }
            return Ok(workDay);
        }

        [HttpPost]
        public IActionResult CreateWorkDay([FromBody] WorkDayForCreateData data)
        {
            var maxWorkDayId = WorkDaysDataStore.Current.Days.Max(wd => wd.Id);
            var newWorkDay = new WorkDay()
            {
                Id = ++maxWorkDayId,
                Day = data.Day,
                AppointmentHours = data.AppointmentHours
            };
            WorkDaysDataStore.Current.Days.Add(newWorkDay);
            return CreatedAtRoute("GetWorkDayById", new { newWorkDay.Id }, newWorkDay);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWorkDay(int id, [FromBody] WorkDayForUpdateData data)
        {
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }

            workDay.Day = data.Day;
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchWorkDay(int id, [FromBody] JsonPatchDocument<WorkDayForUpdateData> patchDoc)
        {
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }
            WorkDayForUpdateData patchWorkDay = new WorkDayForUpdateData()
            {
                Day = workDay.Day
            };

            patchDoc.ApplyTo(patchWorkDay,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            workDay.Day = patchWorkDay.Day;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWorkDay(int id)
        {
            WorkDay workDay = WorkDaysDataStore.Current.Days.FirstOrDefault(wd => wd.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }
            WorkDaysDataStore.Current.Days.Remove(workDay);
            return NoContent();
        }
    }
}
