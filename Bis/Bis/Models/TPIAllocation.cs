namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TPIAllocation
    {
        [Key]
        public int id { get; set; }

        public string title { get; set; } // company name - date default

        public int employeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int companyId { get; set; }
        public virtual Company Company { get; set; }

        public int locationId { get; set; }
        public virtual Location Location { get; set; }

        public int vendorId { get; set; }
        public virtual Vendor Vendor{ get; set; }

        public decimal? travelAllovance { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? date { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? start { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? finish { get; set; }
        [StringLength(50)]
        public string status { get; set; }

        [Column(TypeName = "text")]
        public string remark { get; set; }
    }
}
