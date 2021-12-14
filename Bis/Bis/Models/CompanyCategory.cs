namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Company Category")]
    public partial class CompanyCategory
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string name { get; set; }
        public string categoryDayCost { get; set; }
        public string categoryOTCost { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }
    }
}
