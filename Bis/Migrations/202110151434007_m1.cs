namespace Bis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advance",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        date = c.DateTime(),
                        amount = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        reason = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employee", t => t.employeeId, cascadeDelete: true)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.String(nullable: false, maxLength: 50),
                        categoryId = c.Int(),
                        subCategoryId = c.Int(),
                        departmentId = c.Int(),
                        companyId = c.Int(),
                        name = c.String(nullable: false, maxLength: 50),
                        fatherNmae = c.String(maxLength: 50),
                        age = c.Int(),
                        gender = c.String(maxLength: 50),
                        dob = c.DateTime(),
                        maritalStatus = c.String(maxLength: 50),
                        email = c.String(maxLength: 50),
                        phoneNumber = c.String(maxLength: 50),
                        adharNumber = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                        bloodGroup = c.String(maxLength: 50),
                        address = c.String(unicode: false, storeType: "text"),
                        communicationAddress = c.String(unicode: false, storeType: "text"),
                        designation = c.String(maxLength: 50),
                        doj = c.DateTime(),
                        bankName = c.String(maxLength: 50),
                        branchName = c.String(maxLength: 50),
                        holderName = c.String(maxLength: 50),
                        accountNo = c.String(maxLength: 50),
                        ifscCode = c.String(maxLength: 50),
                        panNo = c.String(maxLength: 50),
                        institutionName = c.String(maxLength: 50),
                        degree = c.String(maxLength: 50),
                        yearofCompletion = c.String(maxLength: 50),
                        university = c.String(maxLength: 50),
                        percentage = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        ndeQualificationType = c.String(maxLength: 50),
                        expiryDate = c.DateTime(),
                        industryName = c.String(maxLength: 50),
                        salary = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        periodFrom = c.DateTime(),
                        periodTo = c.DateTime(),
                        reason = c.String(unicode: false, storeType: "text"),
                        salaryType = c.String(maxLength: 50),
                        bisSalary = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        uniformIssueDate = c.DateTime(),
                        shoeIssueDate = c.DateTime(),
                        status = c.String(maxLength: 50),
                        grade = c.String(maxLength: 50),
                        basic = c.String(),
                        DA = c.String(),
                        HRA = c.String(),
                        bioCode = c.String(maxLength: 50),
                        photo = c.String(maxLength: 255),
                        insuranceCategory = c.String(),
                        insurancePolicyNo = c.String(),
                        esi = c.String(),
                        pf = c.String(),
                        createUser = c.Boolean(nullable: false),
                        reforerecePersonForBis = c.String(maxLength: 50),
                        preveiousComapnyReforerece = c.String(maxLength: 50),
                        reforerecePersonContactnumber = c.String(maxLength: 50),
                        reforerecePersonDesination = c.String(maxLength: 50),
                        collegeHodName = c.String(maxLength: 50),
                        collegeHodContactNumber = c.String(),
                        NDTcertificateExpairyDate = c.String(maxLength: 50),
                        alternativeNumber = c.String(maxLength: 50),
                        InActiveDate = c.DateTime(),
                        shiftId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Category", t => t.categoryId)
                .ForeignKey("dbo.Department", t => t.departmentId)
                .ForeignKey("dbo.SubCategory", t => t.subCategoryId)
                .ForeignKey("dbo.Company", t => t.companyId)
                .ForeignKey("dbo.shifts", t => t.shiftId)
                .Index(t => t.categoryId)
                .Index(t => t.subCategoryId)
                .Index(t => t.departmentId)
                .Index(t => t.companyId)
                .Index(t => t.shiftId);
            
            CreateTable(
                "dbo.Attendance",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        date = c.DateTime(),
                        temperator = c.String(maxLength: 50),
                        mask = c.String(maxLength: 50),
                        remark = c.String(maxLength: 255),
                        status = c.String(maxLength: 50),
                        entryTime = c.DateTime(nullable: false),
                        PFStatus = c.String(),
                        companyId = c.Int(nullable: false),
                        departmentId = c.Int(nullable: false),
                        shiftId = c.Int(nullable: false),
                        categoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employee", t => t.employeeId, cascadeDelete: true)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 255),
                        description = c.String(unicode: false, storeType: "text"),
                        OverTime = c.String(),
                        StartingTime = c.String(),
                        EndingTime = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SubCategory",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        categoryId = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 255),
                        description = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Category", t => t.categoryId, cascadeDelete: true)
                .Index(t => t.categoryId);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        subCategoryId = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 255),
                        description = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.SubCategory", t => t.subCategoryId, cascadeDelete: true)
                .Index(t => t.subCategoryId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        locationId = c.Int(nullable: false),
                        companyName = c.String(nullable: false, maxLength: 50),
                        companyCategoryId = c.Int(nullable: false),
                        phoneNo = c.String(maxLength: 50),
                        companyDescription = c.String(maxLength: 250),
                        companyAddress = c.String(maxLength: 250),
                        email = c.String(maxLength: 50),
                        gSTIN = c.String(maxLength: 50),
                        pinCode = c.Int(),
                        stateCode = c.Int(),
                        cityCode = c.Int(),
                        vendorCode = c.String(maxLength: 50),
                        state = c.String(maxLength: 50),
                        city = c.String(maxLength: 50),
                        companyDetails = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Company Category", t => t.companyCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.locationId)
                .Index(t => t.locationId)
                .Index(t => t.companyCategoryId);
            
            CreateTable(
                "dbo.Company Category",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 255),
                        categoryDayCost = c.String(),
                        categoryOTCost = c.String(),
                        description = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.TPICalls",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        tPIAllocationId = c.Int(nullable: false),
                        date = c.DateTime(),
                        reportingQC = c.String(maxLength: 50),
                        plant = c.String(maxLength: 50),
                        productGroup = c.String(maxLength: 50),
                        inTime = c.DateTime(),
                        outTime = c.DateTime(),
                        offeringTime = c.Decimal(precision: 18, scale: 2),
                        idleTime = c.Decimal(precision: 18, scale: 2),
                        days = c.Int(),
                        totalQTYoffered = c.Int(),
                        noofOkCasting = c.Int(),
                        ftp = c.Int(),
                        stp = c.Int(),
                        rw = c.Int(),
                        hold = c.Int(),
                        rejected = c.Int(),
                        scopeInspection = c.String(unicode: false, storeType: "text"),
                        status = c.String(maxLength: 50),
                        createdBy = c.Int(),
                        createdAt = c.DateTime(),
                        modifiedBy = c.Int(),
                        modifiedAt = c.DateTime(),
                        stay = c.String(),
                        Company_id = c.Int(),
                        Employee_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.TPIAllocations", t => t.tPIAllocationId, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.Company_id)
                .ForeignKey("dbo.Employee", t => t.Employee_id)
                .Index(t => t.tPIAllocationId)
                .Index(t => t.Company_id)
                .Index(t => t.Employee_id);
            
            CreateTable(
                "dbo.TPIAllocations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        employeeId = c.Int(nullable: false),
                        companyId = c.Int(nullable: false),
                        locationId = c.Int(nullable: false),
                        vendorId = c.Int(nullable: false),
                        travelAllovance = c.Decimal(precision: 18, scale: 2),
                        date = c.DateTime(),
                        start = c.DateTime(),
                        finish = c.DateTime(),
                        status = c.String(maxLength: 50),
                        remark = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Company", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.employeeId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.locationId, cascadeDelete: true)
                .ForeignKey("dbo.Vendor", t => t.vendorId, cascadeDelete: true)
                .Index(t => t.employeeId)
                .Index(t => t.companyId)
                .Index(t => t.locationId)
                .Index(t => t.vendorId);
            
            CreateTable(
                "dbo.Vendor",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(nullable: false),
                        locationId = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 50),
                        code = c.String(nullable: false, maxLength: 50),
                        phone = c.String(maxLength: 50),
                        email = c.String(maxLength: 50),
                        address = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Company", t => t.companyId)
                .ForeignKey("dbo.Location", t => t.locationId)
                .Index(t => t.companyId)
                .Index(t => t.locationId);
            
            CreateTable(
                "dbo.Detection",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        advance = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        loan = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        bonus = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        tds = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        certificationFees = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        travelAllowance = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        otherAllowance = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        otherDetection = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        cashVoucher = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        remak = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employee", t => t.employeeId, cascadeDelete: true)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.Loan",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        date = c.DateTime(),
                        loanAmount = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        reason = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employee", t => t.employeeId, cascadeDelete: true)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.Salary",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        date = c.DateTime(),
                        noOfDaysPresent = c.Int(),
                        basicSalary = c.Decimal(precision: 18, scale: 2),
                        travelAllowance = c.Decimal(precision: 18, scale: 2),
                        loan = c.Decimal(precision: 18, scale: 2),
                        bonus = c.Decimal(precision: 18, scale: 2),
                        advance = c.Decimal(precision: 18, scale: 2),
                        tDS = c.Decimal(precision: 18, scale: 2),
                        cashVoucher = c.Decimal(precision: 18, scale: 2),
                        certificationFees = c.Decimal(precision: 18, scale: 2),
                        totalDeduction = c.Decimal(precision: 18, scale: 2),
                        grossSalary = c.Decimal(precision: 18, scale: 2),
                        actualSalary = c.Decimal(precision: 18, scale: 2),
                        netSalary = c.Decimal(precision: 18, scale: 2),
                        projectSalary = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employee", t => t.employeeId, cascadeDelete: true)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartingTime = c.DateTime(nullable: false),
                        EndingTime = c.DateTime(nullable: false),
                        OverTime = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Charges",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        locationId = c.Int(nullable: false),
                        companyId = c.Int(nullable: false),
                        categoryId = c.Int(nullable: false),
                        employeeStayCharge = c.Int(),
                        employeeVisitCharge = c.Int(),
                        companyStayCharge = c.Int(),
                        companyVisitCharge = c.Int(),
                        employeeClaimCharges = c.Int(),
                        companyClaimCharges = c.Int(),
                        remark = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Category", t => t.categoryId, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.locationId, cascadeDelete: true)
                .Index(t => t.locationId)
                .Index(t => t.companyId)
                .Index(t => t.categoryId);
            
            CreateTable(
                "dbo.monthlyAttendances",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        employeeName = c.String(),
                        catagory = c.String(),
                        year = c.String(),
                        month = c.String(),
                        date = c.DateTime(nullable: false),
                        present = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        abosent = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        PFAttendance = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employee", t => t.employeeId, cascadeDelete: true)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 50),
                        pf = c.Decimal(nullable: false, precision: 18, scale: 2),
                        esi = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50),
                        username = c.String(nullable: false, maxLength: 50),
                        password = c.String(nullable: false, maxLength: 50),
                        role = c.String(nullable: false, maxLength: 50),
                        status = c.String(maxLength: 50),
                        lastLogin = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.monthlyAttendances", "employeeId", "dbo.Employee");
            DropForeignKey("dbo.Charges", "locationId", "dbo.Location");
            DropForeignKey("dbo.Charges", "companyId", "dbo.Company");
            DropForeignKey("dbo.Charges", "categoryId", "dbo.Category");
            DropForeignKey("dbo.TPICalls", "Employee_id", "dbo.Employee");
            DropForeignKey("dbo.Employee", "shiftId", "dbo.shifts");
            DropForeignKey("dbo.Salary", "employeeId", "dbo.Employee");
            DropForeignKey("dbo.Loan", "employeeId", "dbo.Employee");
            DropForeignKey("dbo.Detection", "employeeId", "dbo.Employee");
            DropForeignKey("dbo.Employee", "companyId", "dbo.Company");
            DropForeignKey("dbo.TPICalls", "Company_id", "dbo.Company");
            DropForeignKey("dbo.TPICalls", "tPIAllocationId", "dbo.TPIAllocations");
            DropForeignKey("dbo.TPIAllocations", "vendorId", "dbo.Vendor");
            DropForeignKey("dbo.Vendor", "locationId", "dbo.Location");
            DropForeignKey("dbo.Vendor", "companyId", "dbo.Company");
            DropForeignKey("dbo.TPIAllocations", "locationId", "dbo.Location");
            DropForeignKey("dbo.TPIAllocations", "employeeId", "dbo.Employee");
            DropForeignKey("dbo.TPIAllocations", "companyId", "dbo.Company");
            DropForeignKey("dbo.Company", "locationId", "dbo.Location");
            DropForeignKey("dbo.Company", "companyCategoryId", "dbo.Company Category");
            DropForeignKey("dbo.Employee", "subCategoryId", "dbo.SubCategory");
            DropForeignKey("dbo.Department", "subCategoryId", "dbo.SubCategory");
            DropForeignKey("dbo.Employee", "departmentId", "dbo.Department");
            DropForeignKey("dbo.SubCategory", "categoryId", "dbo.Category");
            DropForeignKey("dbo.Employee", "categoryId", "dbo.Category");
            DropForeignKey("dbo.Attendance", "employeeId", "dbo.Employee");
            DropForeignKey("dbo.Advance", "employeeId", "dbo.Employee");
            DropIndex("dbo.monthlyAttendances", new[] { "employeeId" });
            DropIndex("dbo.Charges", new[] { "categoryId" });
            DropIndex("dbo.Charges", new[] { "companyId" });
            DropIndex("dbo.Charges", new[] { "locationId" });
            DropIndex("dbo.Salary", new[] { "employeeId" });
            DropIndex("dbo.Loan", new[] { "employeeId" });
            DropIndex("dbo.Detection", new[] { "employeeId" });
            DropIndex("dbo.Vendor", new[] { "locationId" });
            DropIndex("dbo.Vendor", new[] { "companyId" });
            DropIndex("dbo.TPIAllocations", new[] { "vendorId" });
            DropIndex("dbo.TPIAllocations", new[] { "locationId" });
            DropIndex("dbo.TPIAllocations", new[] { "companyId" });
            DropIndex("dbo.TPIAllocations", new[] { "employeeId" });
            DropIndex("dbo.TPICalls", new[] { "Employee_id" });
            DropIndex("dbo.TPICalls", new[] { "Company_id" });
            DropIndex("dbo.TPICalls", new[] { "tPIAllocationId" });
            DropIndex("dbo.Company", new[] { "companyCategoryId" });
            DropIndex("dbo.Company", new[] { "locationId" });
            DropIndex("dbo.Department", new[] { "subCategoryId" });
            DropIndex("dbo.SubCategory", new[] { "categoryId" });
            DropIndex("dbo.Attendance", new[] { "employeeId" });
            DropIndex("dbo.Employee", new[] { "shiftId" });
            DropIndex("dbo.Employee", new[] { "companyId" });
            DropIndex("dbo.Employee", new[] { "departmentId" });
            DropIndex("dbo.Employee", new[] { "subCategoryId" });
            DropIndex("dbo.Employee", new[] { "categoryId" });
            DropIndex("dbo.Advance", new[] { "employeeId" });
            DropTable("dbo.User");
            DropTable("dbo.Settings");
            DropTable("dbo.monthlyAttendances");
            DropTable("dbo.Charges");
            DropTable("dbo.shifts");
            DropTable("dbo.Salary");
            DropTable("dbo.Loan");
            DropTable("dbo.Detection");
            DropTable("dbo.Vendor");
            DropTable("dbo.TPIAllocations");
            DropTable("dbo.TPICalls");
            DropTable("dbo.Location");
            DropTable("dbo.Company Category");
            DropTable("dbo.Company");
            DropTable("dbo.Department");
            DropTable("dbo.SubCategory");
            DropTable("dbo.Category");
            DropTable("dbo.Attendance");
            DropTable("dbo.Employee");
            DropTable("dbo.Advance");
        }
    }
}
