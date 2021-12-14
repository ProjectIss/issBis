using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bis.Custom;
using Bis.Models;
using Bis.SQLHelper;
using Newtonsoft.Json;

namespace Bis.Controllers
{
    [CustomAuthorize(Roles = "Admin,Manager")]
    public class AttendancesController : Controller
    {
        private BISModel db = new BISModel();

        // GET: Attendances
        public ActionResult Index()
        {
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View();
        }

        public JsonResult CategoryEmployees(int category, string date)
        {
            List<object> lstAttendance = new List<object>();
            var catEmployees = db.Employees.Where(x => x.categoryId == category);
            DateTime attDate = Convert.ToDateTime(date);
            var todayEmployees = db.Attendances.Where(x => EntityFunctions.TruncateTime(x.date) == attDate);
            foreach (var employee in catEmployees)
            {
                var todayEmployee = todayEmployees.FirstOrDefault(x => x.employeeId == employee.id);
                if (todayEmployee == null)
                {
                    todayEmployee = new Attendance
                    {
                        date = attDate,
                        employeeId = employee.id,
                        status = "Present",
                    };
                    db.Attendances.Add(todayEmployee);
                }
            }
            db.SaveChanges();
            return Json(DataTableToJSON(BindTable(date,category)), JsonRequestBehavior.AllowGet);
        }

        private DataTable BindTable(string date,int CategoryID)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AttendanceDetails"));
            objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, date);
            objHelper.AddInParameter(objCommand, "CATEGORYID", SqlDbType.Int, CategoryID);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "AttendanceDetails");
            return dtResult;
        }

        public string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public ActionResult SaveAttendance(List<Attendance> attendance,string Date)
        {
            List<object> lstAttendance = new List<object>();
            DateTime attDate = Convert.ToDateTime(Date);
            var todayEmployees = db.Attendances.Where(x => EntityFunctions.TruncateTime(x.date) == attDate);
            foreach (var att in attendance)
            {
                var todayEmployee = todayEmployees.FirstOrDefault(x => x.id == att.id);
                if (todayEmployee != null)
                {
                    todayEmployee.status = att.status;
                }
            }
            db.SaveChanges();
            return Json(lstAttendance);
        }

        // GET: Attendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,employeeId,date,status")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Attendances.Add(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", attendance.employeeId);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", attendance.employeeId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,employeeId,date,status")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", attendance.employeeId);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            db.Attendances.Remove(attendance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
