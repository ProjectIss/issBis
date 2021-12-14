namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attendance")]
    public partial class Attendance
    {
        [Key]
        public int id { get; set; }

        public int employeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? date { get; set; }

        [StringLength(50)]
        public string temperator { get; set; }
        [StringLength(50)]
        public string mask { get; set; }
        [StringLength(255)]
        public string remark { get; set; }

        [StringLength(50)]
        public string status { get; set; }
    }
}
