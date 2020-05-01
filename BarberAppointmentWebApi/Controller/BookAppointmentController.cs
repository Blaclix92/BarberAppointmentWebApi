using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BarberAppointmentWebApi.Model.BookAppoinments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BarberAppointmentWebApi.Controller
{
    [ApiController]
    [Authorize]
    public class BookAppointmentController : ControllerBase
    {
        [HttpGet]
        [Route("api/bookappointments")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBookAppointments()
        {
            return Ok(BookAppointmentDataStore.Current.Appointments);
        }

        [HttpGet("{id}", Name = "GetBookAppointmentById")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBookAppointmentById(int id)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            BookAppointment appointment = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [Route("api/barber/bookappointments")]
        [HttpGet("{date}", Name = "GetBookAppointmentsByDateAndBarberId")]
        [Authorize(Roles ="Barber")]
        public IActionResult GetBookAppointmentsByDateAndBarberId(string date)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int barberId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
                List<BookAppointment> appointments = BookAppointmentDataStore.Current.Appointments.Where(ba => ba.BarberId == barberId && ba.Date == date).ToList();
                if (appointments.Count() == 0 || appointments == null)
                {
                    return NotFound();
                }
                return Ok(appointments);
        }

        [Route("api/barber/bookappointments/{clientId}")]
        [HttpPost]
        [Authorize(Roles = "Barber")]
        public IActionResult CreateBookAppointmentForBarber(int clientId, [FromBody] BookAppointmentForCreateData data)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int barberId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            var role = claimsPrincipal.FindFirst("role").Value;
            var maxBookAppointmentId = BookAppointmentDataStore.Current.Appointments.Max(ba => ba.Id);
            var newBookAppointment = new BookAppointment()
            {
                Id = ++maxBookAppointmentId,
                Cancel = 0,
                Hour = data.Hour,
                Date = data.Date,
                ClientId = clientId, // check uitvoeren van barber clients list
                BarberId = barberId
            };
            BookAppointmentDataStore.Current.Appointments.Add(newBookAppointment);
            return CreatedAtRoute("GetBookAppointmentsById", new { newBookAppointment.Id }, newBookAppointment);
        }

        [Route("api/clients/bookappointments")]
        [HttpGet("{date}", Name = "GetBookAppointmentByDateClientId")]
        [Authorize(Roles ="Client")]
        public IActionResult GetBookAppointmentByDateClientId(string date)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int clientId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            BookAppointment appointment = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.ClientId == clientId && ba.Date == date);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [Route("api/client/bookappointments/")]
        [HttpPost]
        [Authorize(Roles = "Client")]
        public IActionResult CreateBookAppointment([FromBody] BookAppointmentForCreateData data)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int clientId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            var role = claimsPrincipal.FindFirst("role").Value;
            var maxBookAppointmentId = BookAppointmentDataStore.Current.Appointments.Max(ba => ba.Id);
            var newBookAppointment = new BookAppointment()
            {
                Id = ++maxBookAppointmentId,
                Cancel = 0,
                Hour = data.Hour,
                Date = data.Date,
                ClientId = clientId,
                BarberId = 2 // TODO: this should be fetched from database
            };
            BookAppointmentDataStore.Current.Appointments.Add(newBookAppointment);
            return CreatedAtRoute("GetBookAppointmentsById", new { newBookAppointment.Id }, newBookAppointment);
        }

        [Route("api/barber/bookappointments/")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Barber")]
        public IActionResult UpdateBookAppointmentForBarber(int id, [FromBody] BookAppointmentForUpdateData data)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            int barberId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
                BookAppointment bookAppointment = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.Id == id && ba.BarberId == barberId);
                if (bookAppointment == null)
                {
                    return NotFound();
                }

                bookAppointment.Cancel = data.Cancel;
                bookAppointment.Hour = data.Hour;
                return NoContent();
        }

        [Route("api/bookappointments")]
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

        [Route("api/bookappointments")]
        [HttpDelete("{id}")]
        public IActionResult DeleteBookAppointment(int id)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var role = claimsPrincipal.FindFirst("role").Value;
            if (!role.Equals("client"))
            {
                BookAppointment bookAppointment = BookAppointmentDataStore.Current.Appointments.FirstOrDefault(ba => ba.Id == id);
                if (bookAppointment == null)
                {
                    return NotFound();
                }
                BookAppointmentDataStore.Current.Appointments.Remove(bookAppointment);
                return NoContent();
            }
            return Unauthorized();
        }
    }
}