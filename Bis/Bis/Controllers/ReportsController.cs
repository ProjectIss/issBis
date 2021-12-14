using Bis.Models;
using Bis.SQLHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Bis.Custom;
using System.Data.Entity.Core.Objects;

namespace Bis.Controllers
{
    [CustomAuthorize(Roles = "Admin,Manager")]
    public class ReportsController : Controller
    {
        private BISModel db = new BISModel();
        List<SelectListItem> empList = new List<SelectListItem>();


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
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AttendanceConsolidate"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, fromDate);
            objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, toDate);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "AttendanceConsolidate");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
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
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("EmployeeDetail"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, CATEGORY);
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
            var lstEmployeeDetails = db.Employees.ToList();
            ViewBag.employeeMoney = lstEmployeeDetails;
            return View(lstEmployeeDetails);
        }

        public ActionResult MoneyTransferReport(int employeeID, string DATEFROM)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("MoneyTransferReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, employeeID);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, DATEFROM);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "MoneyTransferReport");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
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

        public ActionResult esipfreport(int category, string from, string to)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("esipfreport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            objHelper.AddInParameter(objCommand, "DATEFROM", SqlDbType.NVarChar, from);
            objHelper.AddInParameter(objCommand, "DATETO", SqlDbType.NVarChar, to);
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
            return View(lstcategory);
        }
        [HttpPost]
        public JsonResult AdvannceReport(int? category)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AdvanceReport"));
            objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
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
        public ActionResult DailyAttendance()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            empList.Add(new SelectListItem { Text = "-Select Employee-", Value = "0" });
            ViewBag.employee = empList;
            return View(lstcategory);
        }
        [HttpPost]
        public JsonResult DailyAttendanceReport(int? category, string fromDate, string toDate, int empID)
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
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "DailyAttendanceReport");
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
    }
}
