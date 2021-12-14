namespace Bis.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BISModel : DbContext
    {
        public BISModel()
            : base("name=BISModel")
        {
        }

        public virtual DbSet<Advance> Advances { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Detection> Detections { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<TPICall> TPICalls { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Charge> Charges { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TPIAllocation> TPIAllocations { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<CompanyCategory> CompanyCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasRequired(c => c.Location).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Vendor>().HasRequired(c => c.Company).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Vendor>().HasRequired(c => c.Location).WithMany().WillCascadeOnDelete(false);
        }
    }
}
