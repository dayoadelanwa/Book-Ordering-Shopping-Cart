﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }
        public int? PostalCode { get; set; }
        public int? PhoneNumber { get; set; }
    }
}
