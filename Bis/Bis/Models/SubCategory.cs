namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubCategory")]
    public partial class SubCategory
    {
        public SubCategory()
        {
            Departments = new HashSet<Department>();
            Employees = new HashSet<Employee>();
        }

        [Key]
        public int id { get; set; }

        public int categoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(10)]
        public string description { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
