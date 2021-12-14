namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class Location
    {
     
        [Key]
        public int id { get; set; }

        [Column("location")]
        [StringLength(50)]
        public string name { get; set; }
    }
}
