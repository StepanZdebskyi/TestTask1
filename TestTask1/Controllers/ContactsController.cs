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
    public class ContactsController : Controller
    {
        private readonly IncidentsManagContext _context;

        public ContactsController(IncidentsManagContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactDTO contactInfo)
        {
            var account = await _context.Accounts.Include(a => a.Contacts).Where(b => b.Name == contactInfo.AccountName).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.Where(a => a.Email == contactInfo.Email).FirstOrDefaultAsync();

            if (contact == null)
            {
                contact = new Contact
                {
                    AccountId = account.Id,
                    FirstName = contactInfo.FirstName,
                    LastName = contactInfo.LastName,
                    Email = contactInfo.Email
                };

                _context.Contacts.Add(contact);

                account.Contacts.Add(contact);
            }
            else 
            {
                contact.FirstName = contactInfo.FirstName;
                contact.LastName = contactInfo.LastName;

                if (account.Contacts.Where(a => a.Email == contact.Email).FirstOrDefault() == null)
                    account.Contacts.Add(contact);
            }

            await _context.SaveChangesAsync();

            return Ok(new { ContactEmail = contact.Email });

        }

        [HttpGet]
        public async Task<IEnumerable<ContactDTO>> GetContact()
        {
            var result = await _context.Contacts
            .Include(c => c.Account)
            .Select(c => new ContactDTO
            {
                AccountName = c.Account.Name,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            })
            .ToListAsync();

            return result;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContact([FromBody] ContactDTO contact)
        {
            string contactEmail = contact.Email;
            var contactToDelete = await _context.Contacts.Where(a => a.Email == contactEmail).FirstOrDefaultAsync();

            if (contactToDelete == null)
                return NotFound();

            _context.Contacts.Remove(contactToDelete);

            await _context.SaveChangesAsync();

            return Ok(new { DeletedContactEmail = contactEmail });
        }
    }
}
