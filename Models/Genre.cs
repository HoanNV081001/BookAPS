using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Book.Enums;

namespace Book.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Genre can not be null")]
        [StringLength(255)]
        public string Description { get; set; }
        public GenreApproval Status { get; set; }
    }
}