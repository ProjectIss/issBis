namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Company")]
    public partial class Company
    {
        public Company()
        {
            TPICalls = new HashSet<TPICall>();
        }

        [Key]
        public int id { get; set; }

        public int locationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        [StringLength(50)]
        public string companyName { get; set; }

        public int companyCategoryId { get; set; }
        public virtual CompanyCategory CompanyCategory { get; set; }

        [StringLength(50)]
        public string phoneNo { get; set; }

        [StringLength(250)]
        public string companyDescription { get; set; }

        [StringLength(250)]
        public string companyAddress { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string gSTIN { get; set; }

        public int? pinCode { get; set; }

        public int? stateCode { get; set; }

        public int? cityCode { get; set; }

        [StringLength(50)]
        public string vendorCode { get; set; }

        [StringLength(50)]
        public string state { get; set; }

        [StringLength(50)]
        public string city { get; set; }

        [Column(TypeName = "text")]
        public string companyDetails { get; set; }

        public virtual ICollection<TPICall> TPICalls { get; set; }
    }
}
