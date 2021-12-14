namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Salary")]
    public partial class Salary
    {
        [Key]
        public int id { get; set; }

        public int employeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? date { get; set; }

        public int? noOfDaysPresent { get; set; }

        public decimal? basicSalary { get; set; }

        public decimal? travelAllowance { get; set; }

        public decimal? loan { get; set; }

        public decimal? bonus { get; set; }

        public decimal? advance { get; set; }

        public decimal? tDS { get; set; }

        public decimal? cashVoucher { get; set; }

        public decimal? certificationFees { get; set; }

        public decimal? totalDeduction { get; set; }

        public decimal? grossSalary { get; set; }

        public decimal? actualSalary { get; set; }

        public decimal? netSalary { get; set; }

        public decimal? projectSalary { get; set; }

    }
}
