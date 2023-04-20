using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class CoverType
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
