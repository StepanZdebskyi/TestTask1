using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask1.Context;
using TestTask1.DTOs;
using TestTask1.Models;

namespace TestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : Controller
    {
        private readonly IncidentsManagContext _context;
        public IncidentsController(IncidentsManagContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromBody]IncidentDTO incident)
        {
            var account = await _context.Accounts
                .Include(a => a.Contacts)
                .Include(b => b.Incidents)
                .Where(a => a.Name == incident.AccountName).FirstOrDefaultAsync();

            if (account == null)
                return NotFound();

            var contact = await _context.Contacts.Where(a => a.Email == incident.Email).FirstOrDefaultAsync();

            if (contact == null)
            {
                Contact newContact = new(){ 
                    FirstName = incident.FirstName,
                    LastName = incident.LastName, 
                    Email = incident.Email
                };

                _context.Contacts.Add(newContact);

                account.Contacts.Add(newContact);

            }
            else {
                contact.FirstName = incident.FirstName;
                contact.LastName = incident.LastName;

                if (account.Contacts.Where(a => a.Email == contact.Email).FirstOrDefault() == null)
                    account.Contacts.Add(contact);
            }

            Incident newIncident = new()
            {
                Description = incident.Description
            };

            _context.Incidents.Add(newIncident);

            account.Incidents.Add(newIncident);

            await _context.SaveChangesAsync();

            return Ok(new { IncidentName = incident.AccountName });
        }

        [HttpGet]
        public async Task<IEnumerable<Incident>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteIncident([FromBody] DeleteIncidentDTO incident)
        {
            var incidentToDelete = await _context.Incidents.Where(a => a.Name == incident.IncidentName).FirstOrDefaultAsync();
            string incidentName = incident.IncidentName;

            if (incidentToDelete == null)
                return NotFound();

            _context.Incidents.Remove(incidentToDelete);

            await _context.SaveChangesAsync();

            return Ok(new { DeletedIncidentName = incidentName });
        }
    }
}
