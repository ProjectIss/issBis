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
            ViewBag.employeeTotal = db.Employees.Count();
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
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("DashBoardAttendance"));
            objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, Date);
            objHelper.AddInParameter(objCommand, "FIRSTDATE", SqlDbType.NVarChar, firstDate);
            objHelper.AddInParameter(objCommand, "LASTDATE", SqlDbType.NVarChar, lastDate);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "DashBoardAttendance");
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
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
}