namespace Bis.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        public Employee()
        {
            Advances = new HashSet<Advance>();
            Attendances = new HashSet<Attendance>();
            Detections = new HashSet<Detection>();
            Loans = new HashSet<Loan>();
            Salaries = new HashSet<Salary>();
            TPICalls = new HashSet<TPICall>();
        }

        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string employeeId { get; set; }

        public int? categoryId { get; set; }
        public virtual Category Category { get; set; }

        public int? subCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        public int? departmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string fatherNmae { get; set; }

        public int? age { get; set; }

        [StringLength(50)]
        public string gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? dob { get; set; }

        [StringLength(50)]
        public string maritalStatus { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string phoneNumber { get; set; }

        [Column(TypeName = "numeric")]
        public decimal adharNumber { get; set; }

        [StringLength(50)]
        public string bloodGroup { get; set; }

        [Column(TypeName = "text")]
        public string address { get; set; }

        [Column(TypeName = "text")]
        public string communicationAddress { get; set; }

        [StringLength(50)]
        public string designation { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? doj { get; set; }

        [StringLength(50)]
        public string bankName { get; set; }

        [StringLength(50)]
        public string branchName { get; set; }

        [StringLength(50)]
        public string holderName { get; set; }

        [StringLength(50)]
        public string accountNo { get; set; }

        [StringLength(50)]
        public string ifscCode { get; set; }

        [StringLength(50)]
        public string panNo { get; set; }

        [StringLength(50)]
        public string institutionName { get; set; }

        [StringLength(50)]
        public string degree { get; set; }

        [StringLength(50)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string yearofCompletion { get; set; }

        [StringLength(50)]
        public string university { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? percentage { get; set; }

        [StringLength(50)]
        public string ndeQualificationType { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? expiryDate { get; set; }

        [StringLength(50)]
        public string industryName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? salary { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? periodFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? periodTo { get; set; }

        [Column(TypeName = "text")]
        public string reason { get; set; }

        [StringLength(50)]
        public string salaryType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? bisSalary { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? uniformIssueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? shoeIssueDate { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        [StringLength(50)]
        public string grade { get; set; }

        //public Boolean? pf { get; set; }
        //public Boolean? esi { get; set; }

        [StringLength(50)]
        public string bioCode { get; set; }

        [StringLength(255)]
        public string photo { get; set; }
        public string insuranceCategory { get; set; }
        public string esi { get; set; }
        public string pf { get; set; }
        public Boolean createUser { get; set; }

        public virtual ICollection<Advance> Advances { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Detection> Detections { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<TPICall> TPICalls { get; set; }
    }
}
