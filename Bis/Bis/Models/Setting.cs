using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Bis.Models
{
    public class Setting
    {
        [Key]
        public int id { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        public decimal pf { get; set; }
        public decimal esi { get; set; }
    }
}