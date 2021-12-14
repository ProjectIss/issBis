using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Bis.Models
{
    public class Charge
    {
        [Key]
        public int id { get; set; }

        public int locationId { get; set; }
        public virtual Location Location { get; set; }

        public int companyId { get; set; }
        public virtual Company Company { get; set; }

        public int categoryId { get; set; }
        public virtual Category Category{ get; set; }

    public int? employeeStayCharge { get; set; }

        public int? employeeVisitCharge { get; set; }

        public int? companyStayCharge { get; set; }

        public int? companyVisitCharge { get; set; }
        public int? employeeClaimCharges { get; set; }

        public int? companyClaimCharges { get; set; }

        [Column(TypeName = "text")]
        public string remark { get; set; }
    }
}