using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Book.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Column(TypeName = "nvarchar")]
          [StringLength(400)]
        public string? FullName { get; set; }

        [Column(TypeName = "nvarchar")]
          [StringLength(400)]
        public string? Address { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? BirthDay{get;set;}
    }
}