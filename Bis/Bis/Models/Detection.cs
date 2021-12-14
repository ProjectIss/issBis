namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Detection")]
    public partial class Detection
    {
        [Key]
        public int id { get; set; }

        public int employeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? advance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? loan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? bonus { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tds { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? certificationFees { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? travelAllowance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? otherAllowance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? cashVoucher { get; set; }

        [Column(TypeName = "text")]
        public string remak { get; set; }
    }
}
