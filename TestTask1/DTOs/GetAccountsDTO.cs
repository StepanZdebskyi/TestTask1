using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask1.DTOs
{
    public class GetAccountsDTO
    {
        public string Name { get; set; }

        public IEnumerable<ContactDTO> Contacts { get; set; }
    }
}
