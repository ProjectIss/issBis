using Bis.Custom;
using Bis.Models;
using Bis.SQLHelper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Bis.Controllers
{
    [CustomAuthorize(Roles = "Admin,Manager")]
    public class ReportsController : Controller
    {
        private BISModel db = new BISModel();
        List<SelectListItem> empList = new List<SelectListItem>();
        List<SelectListItem> companyList = new List<SelectListItem>();
        List<SelectListItem> shift = new List<SelectListItem>();

        private object empID;







        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reports/Create
        public ActionResult Salary()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }
        [HttpPost]
        public JsonResult SalaryProcess(string category, string DateFrom, string DateTo)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("SalaryProcess"));
            objHelper.AddInParameter(objCommand, "CATEGORY", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, DateFrom);
            objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, DateTo);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "SalaryProcess");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSalary(List<Salary> salarydata, string Date)
        {
            List<object> lstSaray = new List<object>();
            DateTime salaryDate = Convert.ToDateTime(Date);
            var totalSalary = db.Salaries.Where(x => x.date.Value.Month == salaryDate.Month);
            foreach (var sal in salarydata)
            {
                var existSalary = totalSalary.FirstOrDefault(x => x.id == sal.id);
                if (existSalary != null)
                {
                    existSalary.date = DateTime.Now;
                    existSalary.employeeId = sal.id;
                    existSalary.noOfDaysPresent = sal.noOfDaysPresent;
                    existSalary.basicSalary = sal.basicSalary;
                    existSalary.travelAllowance = sal.travelAllowance;
                    existSalary.loan = sal.loan;
                    existSalary.bonus = sal.bonus;
                    existSalary.advance = sal.advance;
                    existSalary.tDS = sal.tDS;
                    existSalary.cashVoucher = sal.cashVoucher;
                    existSalary.certificationFees = sal.certificationFees;
                    existSalary.totalDeduction = sal.totalDeduction;
                    existSalary.grossSalary = sal.grossSalary;
                    existSalary.actualSalary = sal.actualSalary;
                    existSalary.netSalary = sal.netSalary;
                    existSalary.projectSalary = sal.projectSalary;
                }
                else
                {
                    existSalary = new Salary
                    {
                        date = DateTime.Now,
                        employeeId = sal.id,
                        noOfDaysPresent = sal.noOfDaysPresent,
                        basicSalary = sal.basicSalary,
                        travelAllowance = sal.travelAllowance,
                        loan = sal.loan,
                        bonus = sal.bonus,
                        advance = sal.advance,
                        tDS = sal.tDS,
                        cashVoucher = sal.cashVoucher,
                        certificationFees = sal.certificationFees,
                        totalDeduction = sal.totalDeduction,
                        grossSalary = sal.grossSalary,
                        actualSalary = sal.actualSalary,
                        netSalary = sal.netSalary,
                        projectSalary = sal.projectSalary
                    };
                    db.Salaries.Add(existSalary);
                }
            }
            db.SaveChanges();
            return Json(lstSaray);
        }

        public ActionResult Attendance(Category category)
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }

        [HttpPost]
        public JsonResult AttendanceResult(string caetory)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AttendanceByCategory"));
            objHelper.AddInParameter(objCommand, "Category", SqlDbType.VarChar, "TPI");
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "AttendanceByCategory");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AttendanceConsolidate(string caetory)
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }

        public JsonResult Consolidate(int? category, string fromDate, string toDate)
        {

            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();

            objCommand = objHelper.GetStoreProcedureCommand(objBuilder.BuildQuery("AttendanceConsolidate"));
            objHelper.AddInParameter(objCommand, "CategoryId", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "fromDate", SqlDbType.NVarChar, fromDate);
            objHelper.AddInParameter(objCommand, "toDate", SqlDbType.NVarChar, toDate);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "AttendanceConsolidate");
            var data = DataTableToJSON(dtResult);
            //jsonResult.MaxJsonLength = int.MaxValue;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployeeDetails()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }

        [HttpPost]
        public JsonResult EmpDetails(int? CATEGORY)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            if (CATEGORY != null)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeDetail"));
                objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, CATEGORY);
            }
            else objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeDetailFORALL"));
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "EmployeeDetail");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);

        }

        public ActionResult Detection()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }
        public JsonResult DeductionReport(string FROM, string TO)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("DeductionReport"));
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, FROM);
            objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, TO);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "DeductionReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);

        }

        public ActionResult TravelExpenses()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }

        [HttpPost]
        public JsonResult TravelExpenseReport(int? category, string from, string to)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("TravelExpenseReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, from);
            objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, to);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "TravelExpenseReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmpDetails()
        {
            var lstEmployeeDetails = db.Employees.ToList();
            ViewBag.employee = lstEmployeeDetails;
            return View(lstEmployeeDetails);
        }
        public JsonResult Employees(int category)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("Employees"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "Employees");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);

        }

        public ActionResult MoneyTransfer()
        {
            var lstEmployeeDetails = db.Categories.ToList();
            ViewBag.employeeMoney = lstEmployeeDetails;
            return View(lstEmployeeDetails);
        }

        public ActionResult MoneyTransferReport(int employeeID, string DATEFROM)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            string month = DATEFROM.Split('-')[1];
            string year = DATEFROM.Split('-')[0];

            if (employeeID == 0) objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("MoneyTransferReportWithoutID"));
            else objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("MoneyTransferReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, employeeID);
            objHelper.AddInParameter(objCommand, "MONTH", SqlDbType.Int, month);
            objHelper.AddInParameter(objCommand, "YEAR", SqlDbType.Int, year);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "MoneyTransferReport");
            DataTable rep = RemoveDuplicateRows(dtResult, "EmployeeId");
            return Json(DataTableToJSON(rep), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExperienceCertificate()
        {
            var lstcategory = db.Employees.ToList();
            ViewBag.employeeid = lstcategory;
            return View(lstcategory);
        }

        [HttpPost]
        public JsonResult ExperienceCertificateReport(int category)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("ExperienceCertificateReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "ExperienceCertificateReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }


        public ActionResult EsiPf()
        {
            var lstEmployeeDetails = db.Categories.ToList();
            ViewBag.category = lstEmployeeDetails;
            return View(lstEmployeeDetails);
        }
        public ActionResult esireport(int category, string date)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("esiReport"));
            objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, date);
            objHelper.AddInParameter(objCommand, "CATEGORYID", SqlDbType.Int, category);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "esipfreport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public ActionResult PF()
        {
            var lstEmployeeDetails = db.Categories.ToList();
            ViewBag.category = lstEmployeeDetails;
            return View(lstEmployeeDetails);
        }

        public ActionResult esipfreport(int category, string date)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("esipfreport"));
            objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, date);
            objHelper.AddInParameter(objCommand, "CATEGORYID", SqlDbType.Int, category);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "esipfreport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public ActionResult paySlip()
        {
            var lstEmployeeDetails = db.Employees.ToList();
            ViewBag.employee = lstEmployeeDetails;
            return View(lstEmployeeDetails);
        }
        public ActionResult PaySlipReport(int category)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("PaySlipReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "PaySlipReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Loan()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }
        [HttpPost]
        public JsonResult LoanReport(int? category)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("LoanReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "LoanReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Advance()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            empList.Add(new SelectListItem { Text = "-Select Employee-", Value = "0" });
            ViewBag.employee = empList;
            return View(lstcategory);

        }
        [HttpPost]
        public JsonResult AdvannceReport(int? category, string fromDate, string toDate, int empID)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AdvanceReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
            objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
            objHelper.AddInParameter(objCommand, "EMPLOYEEID", SqlDbType.NVarChar, empID);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "AdvanceReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeDetailsByCatID(int CATEGORY)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeDetailsByCategoryID"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, CATEGORY);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "EmployeeDetailsByCategoryID");
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                empList.Add(new SelectListItem { Text = dtResult.Rows[i]["name"].ToString(), Value = dtResult.Rows[i]["id"].ToString() });
            }
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EmployeeDetailsByDepartID(int depart)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeDetailsByDepartmentID"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, depart);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "EmployeeDetailsByDepartmentID");
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                empList.Add(new SelectListItem { Text = dtResult.Rows[i]["name"].ToString(), Value = dtResult.Rows[i]["id"].ToString() });
            }
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DailyAttendance()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            var departmentlist = RemoveDuplicatesIterative(db.Departments.ToList());
            ViewBag.department = departmentlist;
            empList.Add(new SelectListItem { Text = "-Select Employee-", Value = "0" });

            ViewBag.employee = empList;
            return View(lstcategory);
        }
        [HttpPost]
        public JsonResult DailyAttendanceReport(int? category, string fromDate, string toDate, int empID, int? dept)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = empID.Equals(0) ? objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("DailyAttendanceReport")) :
            objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("DailyAttendanceReportByEmp"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
            objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
            objHelper.AddInParameter(objCommand, "EMPLOYEEID", SqlDbType.NVarChar, empID);
            if (dept != null && dept > 0)
            {
                objHelper.AddInParameter(objCommand, "DEPT", SqlDbType.NVarChar, dept);
            }
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "DailyAttendanceReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Annexure()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;

            //companyList = db.Companies.ToList();
            companyList.Add(new SelectListItem { Text = "-Select company-", Value = "0" });


            List<Company> lstCompany = new List<Company>();

            foreach (var ch in db.Charges.ToList())
            {
                var company = db.Companies.Where(x => x.id == ch.companyId).FirstOrDefault();
                if (company != null)
                {
                    companyList.Add(new SelectListItem { Text = company.companyName, Value = company.id.ToString() });

                }

            }
            var temp = companyList.GroupBy(x => x.Value);
            List<SelectListItem> companyNewList = new List<SelectListItem>();
            foreach (var i in temp)
            {
                int dup = 0;
                foreach (var m in i)
                {
                    if (dup == 0)
                    {
                        companyNewList.Add(new SelectListItem { Text = m.Text, Value = m.Value.ToString() });
                        dup++;
                    }
                }
            }

            ViewBag.company = companyNewList;

            return View();
        }
        public JsonResult AnnexureCat(int Id)
        {
            ViewBag.category = null;
            List<SelectListItem> cate = new List<SelectListItem>();
            cate.Add(new SelectListItem { Text = "-Select Category-", Value = "0" });

            foreach (var ch in db.Charges.ToList())
            {
                var category = db.Categories.Where(x => x.id == ch.categoryId).FirstOrDefault();
                if (category != null)
                {
                    cate.Add(new SelectListItem { Text = category.name, Value = category.id.ToString() });

                }

            }
            ViewBag.category = cate;

            return Json(companyList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AnnexureReport(int? category, string fromDate, int? cmpID)
        {

            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            if (category > 0 && cmpID > 0)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AnnexureReport"));
                objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
                objHelper.AddInParameter(objCommand, "COMPANYID", SqlDbType.NVarChar, cmpID);
            }
            else if (cmpID > 0)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AnnexureReportByCompany"));
                objHelper.AddInParameter(objCommand, "COMPANYID", SqlDbType.NVarChar, cmpID);
                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
                //objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
            }
            else if (category > 0)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AnnexureReportByCategory"));
                objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
            }
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "AnnexureReport");

            //List<Annexure> lstAnnexures = new List<Annexure>();
            //foreach (DataRow att in dtResult.Rows)
            //{
            //    Annexure annexure = new Annexure();
            //    annexure.Days = Convert.ToInt32(att["Days"].ToString());
            //    int employeeId = Convert.ToInt32(att["employeeId"].ToString());
            //    int catId = 0;
            //    int companyId = 0;
            //    int departmentId = 0;
            //    var dayily = db.Attendances.Where(x => x.employeeId == employeeId).FirstOrDefault();
            //    if (dayily != null)
            //    {
            //         catId = Convert.ToInt32(dayily.categoryId);//att["categoryId"].ToString());
            //         companyId = Convert.ToInt32(dayily.companyId);
            //         departmentId = Convert.ToInt32(dayily.departmentId);
            //    };

            //    var shift = db.Attendances.Where(x => x.employeeId == employeeId && x.shiftId != 0).FirstOrDefault();
            //    int shiftId = 0;
            //    if (shift != null) shiftId = shift.shiftId;
            //    var emp = db.Employees.Where(x => x.id == employeeId).FirstOrDefault();
            //    var cat = db.Categories.Where(x => x.id == catId).FirstOrDefault();
            //    var comp = db.Companies.Where(x => x.id == companyId).FirstOrDefault();
            //    // var dpt = db.Departments.Where(x => x.id == Convert.ToInt32(att["departmentId"].ToString())).FirstOrDefault();
            //    var ch = db.Charges.Where(x => x.companyId == companyId && x.categoryId==category).FirstOrDefault();
            //    var sh = db.Shifts.Where(x => x.Id == shiftId).FirstOrDefault();
            //    if (emp != null) annexure.empName = emp.name;
            //    if (emp != null) annexure.empId = emp.id.ToString();
            //    if (cat != null) annexure.Category = cat.name;
            //    if (comp != null) annexure.ComapnyName = comp.companyName;
            //    if (ch != null) annexure.Charges = Convert.ToInt32(ch.companyClaimCharges);
            //    if (ch != null) annexure.CompanyClaimCharges = annexure.Days * annexure.Charges;
            //    if (sh != null) annexure.Shift = sh.Name;
            //    lstAnnexures.Add(annexure);

            //}
            //DataTable resp = RemoveDuplicateRows(dtResult, "employeeId");
            var data = DataTableToJSON(dtResult);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TPIAnnexure()
        {
            var lstcompany = DuplicatesIterative(db.Companies.ToList());
            ViewBag.company = lstcompany;

            return View();
        }
        [HttpPost]
        public JsonResult TPIAnnexureReport(string fromDate, int? cmpID)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("TPIAnnexure"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, cmpID);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "TPIAnnexure");
            List<TPIReport> tPIReports = new List<TPIReport>();
            List<TPIReport> newL = new List<TPIReport>();
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                TPIReport tpi = new TPIReport();
                tpi.employeeid = Convert.ToInt32(dtResult.Rows[i]["employeeid"]);
                tpi.name = dtResult.Rows[i]["name"].ToString();
                tpi.location = dtResult.Rows[i]["location"].ToString();
                tpi.stay = dtResult.Rows[i]["stay"].ToString();
                tpi.Visit = Convert.ToInt32(dtResult.Rows[i]["Visit"]);
                tPIReports.Add(tpi);
            }

            List<empList> temp = new List<empList>();
            foreach (var item in tPIReports)
            {

                empList selects = new empList();
                selects.Name = item.name;
                //selects.Id = tPIReports.Select(x => x.Visit);
                 temp.Add(selects);
            }
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TPICallAnnexure()
        {
            var lstcompany = RemoveDuplicatesIterative(db.Companies.ToList());
            ViewBag.company = lstcompany;
            return View();
        }
        [HttpPost]
        public JsonResult TPICallAnnexureReport(string fromDate, int? cmpID)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("TPICallAnnexure"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, cmpID);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
            // objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "TPICallAnnexure");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public ActionResult IdCard()
        {
            var lstcategory = db.Employees.ToList();
            ViewBag.employee = lstcategory;
            ViewBag.photoPath = @"C:\GitHub\bis\Bis\Bis\Content\Images\TEST.jpg";
            return View(lstcategory);
        }


        public JsonResult EmployeeIDCard(int? id)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeIDCard"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, id);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "EmployeeIDCard");
            if (dtResult.Rows.Count > 0)
            {
                ViewBag.photoPath = Server.MapPath(dtResult.Rows[0]["Photos"].ToString());
            }
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EmployeeDetailsByID(int CATEGORY)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeDetailsByID"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, CATEGORY);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "EmployeeDetailsByID");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EmployeeDetailsByctg(int CATEGORY)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeDetailsByID"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, CATEGORY);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "EmployeeDetailsByID");
            ViewBag.employeeId = dtResult;
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public JsonResult VendorByCompanyId(int NAME)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("VendorByCompanyID"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, NAME);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "VendorByCompanyID");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public JsonResult VendorByLocationId(int LOCATION)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("VendorByLocationID"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, LOCATION);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "VendorByLocationID");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        // GET: Reports/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: Reports/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult MonthlyAttendance()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            empList.Add(new SelectListItem { Text = "-Select Employee-", Value = "0" });
            ViewBag.employee = empList;
            return View(lstcategory);
        }
        [HttpPost]
        public JsonResult MonthlyAttendanceReport(int? category, string selectedMonth, int empID)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = empID.Equals(0) ? objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("monthlyAtentdanceReport")) :
            objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("monthlyAttendanceReportBYempID"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, selectedMonth);
            objHelper.AddInParameter(objCommand, "EMPID", SqlDbType.NVarChar, empID);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "MonthlyAttendanceReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public ActionResult monthlySalary()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View(lstcategory);
        }
        [HttpPost]
        public JsonResult MonthlySalaryProcess(string category, string DateFrom)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();

            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("SalaryProcess"));
            objHelper.AddInParameter(objCommand, "CATEGORY", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, DateFrom);

            DataTable dtResult = objHelper.LoadDataTable(objCommand, "monthlysalaryProcess");
            foreach (DataRow item in dtResult.Rows)
            {
                if (item["EmpStatus"].ToString() == "InActive")
                {
                    item.Delete();
                }
            }
            dtResult.AcceptChanges();
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthlySaveSalary(List<Salary> salarydata, string Date)
        {
            List<object> lstSaray = new List<object>();
            DateTime salaryDate = Convert.ToDateTime(Date);
            var totalSalary = db.Salaries.Where(x => x.date.Value.Month == salaryDate.Month && x.date.Value.Year == salaryDate.Year).ToList();
            foreach (var sal in salarydata)
            {
                var existSalary = totalSalary.Where(x => x.employeeId == sal.id).FirstOrDefault();
                if (existSalary != null)
                {
                    existSalary.date = salaryDate;
                    existSalary.employeeId = sal.id;
                    existSalary.noOfDaysPresent = sal.noOfDaysPresent;
                    existSalary.basicSalary = sal.basicSalary;
                    existSalary.travelAllowance = sal.travelAllowance;
                    existSalary.loan = sal.loan;
                    existSalary.bonus = sal.bonus;
                    existSalary.advance = sal.advance;
                    existSalary.tDS = sal.tDS;
                    existSalary.cashVoucher = sal.cashVoucher;
                    existSalary.certificationFees = sal.certificationFees;
                    existSalary.totalDeduction = sal.totalDeduction;
                    existSalary.grossSalary = sal.grossSalary;
                    existSalary.actualSalary = sal.actualSalary;
                    existSalary.netSalary = sal.netSalary;
                    existSalary.projectSalary = sal.projectSalary;
                }
                else
                {
                    existSalary = new Salary
                    {
                        date = salaryDate,
                        employeeId = sal.id,
                        noOfDaysPresent = sal.noOfDaysPresent,
                        basicSalary = sal.basicSalary,
                        travelAllowance = sal.travelAllowance,
                        loan = sal.loan,
                        bonus = sal.bonus,
                        advance = sal.advance,
                        tDS = sal.tDS,
                        cashVoucher = sal.cashVoucher,
                        certificationFees = sal.certificationFees,
                        totalDeduction = sal.totalDeduction,
                        grossSalary = sal.grossSalary,
                        actualSalary = sal.actualSalary,
                        netSalary = sal.netSalary,
                        projectSalary = sal.projectSalary
                    };
                    db.Salaries.Add(existSalary);
                }
            }
            db.SaveChanges();
            return Json(lstSaray);
        }
        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);
            return dTable;
        }
        public ActionResult ShiftReport()
        {
            List<SelectListItem> shift = new List<SelectListItem>();
            //shift.Add(new SelectListItem { Text = "-Select Shift -", Value = "0" });
            foreach (var item in db.Shifts.ToList())
            {
                shift.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                //  empList.Add(new SelectListItem { Text = dtResult.Rows[i]["name"].ToString(), Value = dtResult.Rows[i]["id"].ToString() });
            }
            ViewBag.shift = shift;



            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            //companyList = db.Companies.ToList();
            companyList.Add(new SelectListItem { Text = "-Select company-", Value = "0" });
            foreach (var item in db.Companies.ToList())
            {
                companyList.Add(new SelectListItem { Text = item.companyName, Value = item.id.ToString() });
                //  empList.Add(new SelectListItem { Text = dtResult.Rows[i]["name"].ToString(), Value = dtResult.Rows[i]["id"].ToString() });
            }
            ViewBag.company = companyList;
            return View();
        }
        [HttpPost]
        public JsonResult ShiftReport(int? shift, DateTime fromDate, DateTime toDate, int? category)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            
            if (shift != null && fromDate != null && toDate != null && category > 0)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("ShiftReportAll"));
                objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);

                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.DateTime, fromDate);
                objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.DateTime, toDate);
                //objHelper.AddInParameter(objCommand, "COMPANY", SqlDbType.Int, company);
                objHelper.AddInParameter(objCommand, "SHIFT", SqlDbType.Int, shift);
            }
            else if (shift != null && fromDate != null && toDate != null)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("ShiftReportShit"));
                //objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
                objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
                //objHelper.AddInParameter(objCommand, "COMPANY", SqlDbType.Int, company);
                objHelper.AddInParameter(objCommand, "SHIFT", SqlDbType.Int, shift);
            }
            else if (category != null && fromDate != null && toDate != null)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("ShiftReportcategory"));
                objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
                objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);

                //objHelper.AddInParameter(objCommand, "COMPANY", SqlDbType.Int, company);
                //objHelper.AddInParameter(objCommand, "SHIFT", SqlDbType.Int, shift);
            }

            DataTable dtResult = objHelper.LoadDataTable(objCommand, "ShiftReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);


        }
        
        public ActionResult TPIreport()
        {
            var lstcompany = Duplicatecompany(db.Companies.ToList());
            ViewBag.company = lstcompany;
            var lstemploye = db.TPIAllocations.Where(x => x.status != "Completed");
            List<empList> lstEmp = new List<empList>();
            foreach (var item in lstemploye)
            {
                empList obj = new empList();
                obj.Id = item.employeeId;
                obj.Name = item.Employee.name;
                lstEmp.Add(obj);
            }
            ViewBag.employee = lstEmp;
            return View();
        }
        public JsonResult TPIreports(string fromDate, string toDate, int? company, int? employee)
        {

            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            if (company > 0)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("TPIReport"));
                objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, company);
                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
                objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
            }
            else if (employee > 0)
            {
                objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("TPIReports"));
                objHelper.AddInParameter(objCommand, "EmployeeID", SqlDbType.Int, employee);
                objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
                objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
            }
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "TPIReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);


        }
        public static List<Department> RemoveDuplicatesIterative(List<Department> items)
        {
            List<Department> result = new List<Department>();
            for (int i = 0; i < items.Count; i++)
            {
                // Assume not duplicate.
                bool duplicate = false;
                for (int z = 0; z < i; z++)
                {
                    if (items[z].name == items[i].name)
                    {
                        // This is a duplicate.
                        duplicate = true;
                        break;
                    }
                }
                // If not duplicate, add to result.
                if (!duplicate)
                {
                    result.Add(items[i]);
                }
            }
            return result;
        }
        public static List<Company> RemoveDuplicatesIterative(List<Company> items)
        {
            List<Company> result = new List<Company>();
            for (int i = 0; i < items.Count; i++)
            {
                // Assume not duplicate.
                bool duplicate = false;
                for (int z = 0; z < i; z++)
                {
                    if (items[z].companyName == items[i].companyName)
                    {
                        // This is a duplicate.
                        duplicate = true;
                        break;
                    }
                }
                // If not duplicate, add to result.
                if (!duplicate)
                {
                    result.Add(items[i]);
                }
            }
            return result;
        }
        public static List<Company> DuplicatesIterative(List<Company> items)
        {
            List<Company> result = new List<Company>();
            for (int i = 0; i < items.Count; i++)
            {
                // Assume not duplicate.
                bool duplicate = false;
                for (int z = 0; z < i; z++)
                {
                    if (items[z].companyName == items[i].companyName)
                    {
                        // This is a duplicate.
                        duplicate = true;
                        break;
                    }
                }
                // If not duplicate, add to result.
                if (!duplicate)
                {
                    result.Add(items[i]);
                }
            }
            return result;
        }
        public static List<Company> Duplicatecompany(List<Company> items)
        {
            List<Company> result = new List<Company>();
            for (int i = 0; i < items.Count; i++)
            {
                // Assume not duplicate.
                bool duplicate = false;
                for (int z = 0; z < i; z++)
                {
                    if (items[z].companyName == items[i].companyName)
                    {
                        // This is a duplicate.
                        duplicate = true;
                        break;
                    }
                }
                // If not duplicate, add to result.
                if (!duplicate)
                {
                    result.Add(items[i]);
                }
            }
            return result;
        }
        public static List<Employee> Duplicateemploye(List<Employee> items)
        {
            List<Employee> result = new List<Employee>();
            for (int i = 0; i < items.Count; i++)
            {
                // Assume not duplicate.
                bool duplicate = false;
                for (int z = 0; z < i; z++)
                {
                    if (items[z].name == items[i].name)
                    {
                        // This is a duplicate.
                        duplicate = true;
                        break;
                    }
                }
                // If not duplicate, add to result.
                if (!duplicate)
                {
                    result.Add(items[i]);
                }
            }
            return result;
        }
    }
    public class Annexure
    {
        public string empId { get; set; }
        public string Category { get; set; } = "";
        public string empName { get; set; } = "";
        public string Shift { get; set; } = "";
        public string ComapnyName { get; set; } = "";
        public int Days { get; set; } = 0;
        public int Charges { get; set; } = 0;
        public int CompanyClaimCharges { get; set; } = 0;
    }
    public class empList
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public class TPIReport
    {
        public int employeeid { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string companyname { get; set; }
        public string stay { get; set; }
        public int Visit { get; set; }
    }

}
