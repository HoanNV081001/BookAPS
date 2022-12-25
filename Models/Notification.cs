using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Book.Enums;

namespace Book.Models
{
    public class Notification
    {
        [Key]
        public int NotiId { get; set; }
        public string Message { get; set; }
        public NotiCheck NotiStatus { get; set; }
    }
}