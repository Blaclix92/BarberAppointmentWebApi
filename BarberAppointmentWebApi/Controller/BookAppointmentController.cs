using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarberAppointmentWebApi.Model.BookAppoinment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BarberAppointmentWebApi.Controller
{
    [Route("api/bookappointments")]
    [ApiController]
    public class BookAppointmentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBookAppointments()
        {
            return Ok(BookAppointmentDataStore.Current.Appointments);
        }

        [HttpGet("{id}", Name = "GetBookAppointmentsById")]
        public IActionResult GetBookAppointmentsById(int id)
        {
            BookAppointment appointment = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPost]
        public IActionResult CreateBookAppointment([FromBody] BookAppointmentForCreateData data)
        {
            var maxBookAppointmentId = BookAppointmentDataStore.Current.Appointments.Max(ba => ba.Id);
            var newBookAppointment = new BookAppointment()
            {
              Id= ++maxBookAppointmentId,
              Cancel = 0,
              Hour = data.Hour,
              Date = data.Date
            };
            BookAppointmentDataStore.Current.Appointments.Add(newBookAppointment);
            return CreatedAtRoute("GetBookAppointmentsById", new { newBookAppointment.Id }, newBookAppointment);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookAppointment(int id, [FromBody] BookAppointmentForUpdateData data)
        {
            BookAppointment bookAppointment = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.Id == id);
            if (bookAppointment == null)
            {
                return NotFound();
            }

            bookAppointment.Cancel = data.Cancel;
            bookAppointment.Hour = data.Hour; 
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchWorkDay(int id, [FromBody] JsonPatchDocument<BookAppointmentForUpdateData> patchDoc)
        {
            BookAppointment bookAppointment = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.Id == id);
            if (bookAppointment == null)
            {
                return NotFound();
            }
            BookAppointmentForUpdateData patchWorkDay = new BookAppointmentForUpdateData()
            {
               Hour = bookAppointment.Hour,
               Cancel = bookAppointment.Cancel
            };

            patchDoc.ApplyTo(patchWorkDay, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bookAppointment.Cancel = patchWorkDay.Cancel;
            bookAppointment.Hour = patchWorkDay.Hour;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookAppointment(int id)
        {
            BookAppointment bookAppointment  = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.Id == id);
            if (bookAppointment == null)
            {
                return NotFound();
            }
            BookAppointmentDataStore.Current.Appointments.Remove(bookAppointment);
            return NoContent();
        }
    }
}