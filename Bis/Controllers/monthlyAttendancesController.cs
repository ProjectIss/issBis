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
using Bis.Models;
using Bis.SQLHelper;
using Newtonsoft.Json;

namespace Bis.Controllers
{
    public class MonthlyAttendancesController : Controller
    {
        private BISModel db = new BISModel();

        // GET: MonthlyAttendances
        public ActionResult Index()
        {
            //var MonthlyAttendances = db.MonthlyAttendances.Include(m => m.Employee);
            //return View(MonthlyAttendances.ToList());
            var lstcategory = db.Categories.ToList();
            ViewBag.category = lstcategory;
            return View();
        }

        // GET: MonthlyAttendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            monthlyAttendance monthlyAttendance = db.MonthlyAttendances.Find(id);
            if (monthlyAttendance == null)
            {
                return HttpNotFound();
            }
            return View(monthlyAttendance);
        }

        // GET: MonthlyAttendances/Create
        public ActionResult Create()
        {
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId");
            return View();
        }

        // POST: MonthlyAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,employeeId,employeeName,catagory,year,month,date,PFAttendance")] monthlyAttendance monthlyAttendance)
        {
            if (ModelState.IsValid)
            {
                db.MonthlyAttendances.Add(monthlyAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", monthlyAttendance.employeeId);
            return View(monthlyAttendance);
        }

        // GET: MonthlyAttendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            monthlyAttendance monthlyAttendance = db.MonthlyAttendances.Find(id);
            if (monthlyAttendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", monthlyAttendance.employeeId);
            return View(monthlyAttendance);
        }

        // POST: MonthlyAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,employeeId,employeeName,catagory,year,month,date,PFAttendance")] monthlyAttendance monthlyAttendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlyAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", monthlyAttendance.employeeId);
            return View(monthlyAttendance);
        }

        // GET: MonthlyAttendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            monthlyAttendance monthlyAttendance = db.MonthlyAttendances.Find(id);
            if (monthlyAttendance == null)
            {
                return HttpNotFound();
            }
            return View(monthlyAttendance);
        }

        // POST: MonthlyAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            monthlyAttendance monthlyAttendance = db.MonthlyAttendances.Find(id);
            db.MonthlyAttendances.Remove(monthlyAttendance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult CategoryEmployees(int category, string date)
        {

            List<object> lstAttendance = new List<object>();
            var catEmployees = db.Employees.Where(x => x.categoryId == category);
            DateTime attDate = Convert.ToDateTime(date);
            var monthLength = db.MonthlyAttendances.Count();
            var todayEmployees = db.MonthlyAttendances.ToList();
            if (monthLength > 0)
            {
                todayEmployees = db.MonthlyAttendances.Where(x => EntityFunctions.TruncateTime(x.date).Value.Month == attDate.Month && EntityFunctions.TruncateTime(x.date).Value.Year == attDate.Year).ToList();
            }
            foreach (var employee in catEmployees)
            {
                try
                {
                    var todayEmployee = todayEmployees.FirstOrDefault();
                    if (todayEmployees.Count() > 0)
                        todayEmployee = todayEmployees.FirstOrDefault(x => x.employeeId == employee.id);
                    if (todayEmployee == null)
                    {
                        todayEmployee = new monthlyAttendance
                        {
                            date = attDate,
                            employeeId = employee.id,
                            present = 0,
                            abosent = 0,
                            year = attDate.Year.ToString(),
                            catagory = category.ToString(),
                            month = attDate.Month.ToString(),
                            employeeName = employee.name,
                        };
                        db.MonthlyAttendances.Add(todayEmployee);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            db.SaveChanges();

            return Json(DataTableToJSON(BindTable(date, category)), JsonRequestBehavior.AllowGet);

        }

        private DataTable BindTable(string date, int CategoryID)
        {
            SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            SqlCommand objCommand = new SqlCommand();
            QueryBuilder objBuilder = new QueryBuilder();
            objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("monthAttendanceDetails"));
            objHelper.AddInParameter(objCommand, "DATE", SqlDbType.NVarChar, date);
            objHelper.AddInParameter(objCommand, "CATEGORYID", SqlDbType.Int, CategoryID);
            DataTable dtResult = objHelper.LoadDataTable(objCommand, "monthAttendanceDetails");
            return dtResult;
        }

        public string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public ActionResult SaveAttendance(List<monthlyAttendance> attendance, string Date)
        {
            List<object> lstAttendance = new List<object>();
            DateTime attDate = Convert.ToDateTime(Date);
            var todayEmployees = db.MonthlyAttendances.Where(x => EntityFunctions.TruncateTime(x.date).Value.Month == attDate.Month && EntityFunctions.TruncateTime(x.date).Value.Year == attDate.Year);
            foreach (var att in attendance)
            {
                var todayEmployee = todayEmployees.FirstOrDefault(x => x.id == att.id);
                if (todayEmployee != null)
                {
                    todayEmployee.present = att.present;
                    todayEmployee.abosent = att.abosent;
                    todayEmployee.PFAttendance = att.PFAttendance;
                }
            }
            db.SaveChanges();
            return Json(lstAttendance);
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
