namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Employees = new HashSet<Employee>();
            SubCategories = new HashSet<SubCategory>();
        }

        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
