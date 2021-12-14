using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bis.Custom;
using Bis.Models;
using Newtonsoft.Json;

namespace Bis.Controllers
{
    [CustomAuthorize(Roles = "Admin,Manager")]
    public class AdvancesController : Controller
    {
        private BISModel db = new BISModel();

        // GET: Advances
        public ActionResult Index()
        {
            var advances = db.Advances.Include(a => a.Employee);
            return View(advances.ToList());
        }

        // GET: Advances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advance advance = db.Advances.Find(id);
            if (advance == null)
            {
                return HttpNotFound();
            }
            return View(advance);
        }
        [HttpPost]
        public JsonResult EmployeeDetailsByID(int? category)
        {
            //SQLHelper.SQLHelper objHelper = new SQLHelper.SQLHelper();
            //SqlCommand objCommand = new SqlCommand();
            //QueryBuilder objBuilder = new QueryBuilder();
            //objCommand = objHelper.GetSqlQueryCommand(objBuilder.BuildQuery("AdvanceReport"));
            //objHelper.AddInParameter(objCommand, "ID", SqlDbType.Int, category);
            //DataTable dtResult = objHelper.LoadDataTable(objCommand, "AdvanceReport");
            DataTable dtResult = new DataTable();
            return Json(DataTableToJSON(dtResult), JsonRequestBehavior.AllowGet);
        }
        public string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        // GET: Advances/Create
        public ActionResult Create()
        {
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId");
            return View();
        }

        // POST: Advances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,employeeId,date,amount,reason")] Advance advance)
        {
            if (ModelState.IsValid)
            {
                db.Advances.Add(advance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", advance.employeeId);
            return View(advance);
        }

        // GET: Advances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advance advance = db.Advances.Find(id);
            if (advance == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", advance.employeeId);
            return View(advance);
        }

        // POST: Advances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,employeeId,date,amount,reason")] Advance advance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", advance.employeeId);
            return View(advance);
        }

        // GET: Advances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advance advance = db.Advances.Find(id);
            db.Advances.Remove(advance);
            db.SaveChanges();
            if (advance == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: Advances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advance advance = db.Advances.Find(id);
            db.Advances.Remove(advance);
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
