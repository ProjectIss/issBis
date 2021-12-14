namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Loan")]
    public partial class Loan
    {
        [Key]
        public int id { get; set; }

        public int employeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? loanAmount { get; set; }

        [Column(TypeName = "text")]
        public string reason { get; set; }
    }
}
