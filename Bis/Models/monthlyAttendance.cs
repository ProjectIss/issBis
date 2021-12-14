using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bis.Models
{
    public class monthlyAttendance
    {
        [Key]
        public int id { get; set; }
        public int employeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public string employeeName { get; set; }
        public string catagory { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public DateTime date { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? present { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? abosent { get; set; }
        public string PFAttendance { get; set; }
    }
}