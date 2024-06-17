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
    public class AccountsController : Controller
    {
        private readonly IncidentsManagContext _context;

        public AccountsController(IncidentsManagContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDTO accountInfo)
        {
            var account = await _context.Accounts.Include(a => a.Contacts).Where(b => b.Name == accountInfo.Name).FirstOrDefaultAsync();

            var contact = await _context.Contacts.Where(a => a.Email == accountInfo.ContactEmail).FirstOrDefaultAsync();

            if (account == null)
            {
                account = new Account
                {
                    Name = accountInfo.Name,
                };
                _context.Accounts.Add(account);
            }

            if (contact == null)
            {
                Contact newContact = new()
                {
                    FirstName = accountInfo.ContactFirstName,
                    LastName = accountInfo.ContactLastName,
                    Email = accountInfo.ContactEmail, 
                    AccountId = account.Id,
                };

                if (account.Contacts == null)
                {
                    account.Contacts = new List<Contact>();
                }
                _context.Contacts.Add(newContact);

                account.Contacts.Add(newContact);
            }
            else
            {
                contact.FirstName = accountInfo.ContactFirstName;
                contact.LastName = accountInfo.ContactLastName;

                if (account.Contacts == null)
                {
                    account.Contacts = new List<Contact>();
                    account.Contacts.Add(contact);
                }

                if (account.Contacts != null && account.Contacts.Where(a => a.Email == contact.Email).FirstOrDefault() == null)
                    account.Contacts.Add(contact);
            }

            await _context.SaveChangesAsync();

            return Ok(new { AccountName = account.Name });
        }

        [HttpGet]
        public async Task<IEnumerable<GetAccountsDTO>> GetAccount()
        {
            var result = await _context.Accounts
            .Include(a => a.Contacts)
            .Select(acc => new GetAccountsDTO
            {
                Name = acc.Name,
                Contacts = acc.Contacts.Select(cont => new ContactDTO
                {
                    AccountName = acc.Name,
                    FirstName = cont.FirstName,
                    LastName = cont.LastName,
                    Email = cont.Email
                }).ToList()
            }).ToListAsync();

            return result;
        }
    }
}
