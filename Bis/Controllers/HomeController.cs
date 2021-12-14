using Bis.Custom;
using Bis.Models;
using Bis.SQLHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bis.Controllers
{
    [CustomAuthorize(Roles = "Admin,Manager,Employee")]
    public class HomeController : Controller
    {

        public BISModel db = new BISModel();
        public ActionResult Index()
        {

            ViewBag.employeeTotal = db.Employees.Where(x => x.status == "Active").Count();
            ViewBag.companyTotal = db.Companies.Count();
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("TodayAbsent"));
            var totalAbsent = objHelper.ExecuteScalar(objCommand);
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("TotalTPI"));
            var totalTPI = objHelper.ExecuteScalar(objCommand);
            ViewBag.totalAbsent = totalAbsent;
            ViewBag.totalTPI = totalTPI;
            return View();
        }
        public JsonResult HomeIndex(string Date, string firstDate, string lastDate)
        {
            List<IndexAttendance> lstIndexAttendance = new List<IndexAttendance>();
            var lstCat = db.Categories.ToList();
            foreach (var item in lstCat)
            {
                IndexAttendance indexAttendance = new IndexAttendance();
                var lstEmployee = db.Employees.Where(x => x.categoryId == item.id).ToList();
                indexAttendance.Category = item.name;
                foreach (var emp in lstEmployee)
                {
                    var currentDate = DateTime.Now.Date;
                    var monthAtt = db.Attendances.Where(x => x.employeeId == emp.id && x.date.Value.Month == currentDate.Month && x.date.Value.Year == currentDate.Date.Year).ToList();
                    var att = db.Attendances.Where(x => x.employeeId == emp.id && x.date == currentDate).ToList();
                    indexAttendance.Present += att.Where(x => x.status == "Present" && x.date == currentDate).Count();
                    indexAttendance.Absent += att.Where(x => x.status == "Absent" && x.date == currentDate).Count();
                    indexAttendance.MonthAbsent += monthAtt.Where(x => x.status == "Present").Count();
                    indexAttendance.MonthPresent += monthAtt.Where(x => x.status == "Absent").Count();
                    indexAttendance.catID = item.id;
                }
                lstIndexAttendance.Add(indexAttendance);
            }
            //SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            //SqlCommand objCommand = new SqlCommand();
            //QueryBuilder objBuilder = new QueryBuilder();
            //objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("DashBoardAttendance"));
            //objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, Date);
            //objHelper.AddInParameter(objCommand, "FIRSTDATE", SqlDbType.NVarChar, firstDate);
            //objHelper.AddInParameter(objCommand, "LASTDATE", SqlDbType.NVarChar, lastDate);
            //DataTable dtResult = objHelper.LoadDataTable(objCommand, "DashBoardAttendance");
            return Json(lstIndexAttendance, JsonRequestBehavior.AllowGet);
        }
        public string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private decimal? parseFloat()
        {
            throw new NotImplementedException();
        }

    }
    public class IndexAttendance
    {
        public int catID { get; set; }
        public string Category { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int MonthPresent { get; set; }
        public int MonthAbsent { get; set; }
    }
}