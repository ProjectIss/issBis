namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TPICall
    {
        [Key]
        public int id { get; set; }

        public int tPIAllocationId { get; set; }
        public virtual TPIAllocation TPIAllocation { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? date { get; set; }

        [StringLength(50)]
        public string reportingQC { get; set; }

        [StringLength(50)]
        public string plant { get; set; }

        [StringLength(50)]
        public string productGroup { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? inTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? outTime { get; set; }

        public decimal? offeringTime { get; set; }
        public decimal? idleTime { get; set; }

        public int? days { get; set; }

        public int? totalQTYoffered { get; set; }

        public int? noofOkCasting { get; set; }

        public int? ftp { get; set; }

        public int? stp { get; set; }

        public int? rw { get; set; }

        public int? hold { get; set; }

        public int? rejected { get; set; }

        [Column(TypeName = "text")]
        public string scopeInspection { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public int? createdBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? createdAt { get; set; }
        public int? modifiedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? modifiedAt { get; set; }
    }
}
