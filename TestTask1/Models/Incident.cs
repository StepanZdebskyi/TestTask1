using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask1.Models
{
    public class Incident
    {
        [Key]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Description { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
