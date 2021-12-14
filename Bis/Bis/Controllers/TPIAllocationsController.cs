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

namespace Bis.Controllers
{
    [CustomAuthorize(Roles = "Admin,Manager,Employee")]
    public class TPIAllocationsController : Controller
    {
        private BISModel db = new BISModel();

        // GET: TPIAllocations
        public ActionResult Index()
        {
            if (Display.Role == "Employee")
            {
                return RedirectToAction("Employee");
            }

            var tPIAllocations = db.TPIAllocations.Include(t => t.Company).Include(t => t.Employee).OrderByDescending(x => x.id).Take(100);
            return View(tPIAllocations.ToList());
        }

        public ActionResult Employee()
        {
            var tPIAllocations = db.TPIAllocations.Include(t => t.Company).Include(t => t.Employee).Where(x => x.employeeId == Display.UserId).OrderByDescending(x => x.id).Take(100);
            return View(tPIAllocations.ToList());
        }

        // GET: TPIAllocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPIAllocation tPIAllocation = db.TPIAllocations.Find(id);
            if (tPIAllocation == null)
            {
                return HttpNotFound();
            }
            return View(tPIAllocation);
        }

        [CustomAuthorize(Roles = "Admin,Manager")]
        // GET: TPIAllocations/Create
        public ActionResult Create()
        {
            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName");
            ViewBag.LocationId = new SelectList(db.Locations, "id", "name");
            ViewBag.vendorId = new SelectList(db.Vendors, "id", "name");
            var employee = db.Employees.Select(x => new { id = x.id, employeeId = x.employeeId + "-" + x.name });
            ViewBag.employeeId = new SelectList(employee, "id", "employeeId");
            return View();
        }

        // POST: TPIAllocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [CustomAuthorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,employeeId,companyId,vendorId,LocationId,date,start,finish,status,remark")] TPIAllocation tPIAllocation)
        {
            if (ModelState.IsValid)
            {

                tPIAllocation.title = DateTime.Now.ToString("yyMMddhhmmss");
                db.TPIAllocations.Add(tPIAllocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //var catEmployees = db.Employees.Where(x => x.categoryId == category);
            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName", tPIAllocation.companyId);
            ViewBag.vendorId = new SelectList(db.Vendors, "id", "name");
            ViewBag.LocationId = new SelectList(db.Locations, "id", "name");
            var employee = db.Employees.Select(x => new { id = x.id, employeeId = x.employeeId + "-" + x.name });
            ViewBag.employeeId = new SelectList(employee, "id", "employeeId", tPIAllocation.employeeId);
           
            return View(tPIAllocation);
        }

        // GET: TPIAllocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPIAllocation tPIAllocation = db.TPIAllocations.Find(id);
            if (tPIAllocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName", tPIAllocation.companyId);
            ViewBag.vendorId = new SelectList(db.Vendors, "id", "name");
            ViewBag.LocationId = new SelectList(db.Locations, "id", "name");
            var employee = db.Employees.Select(x => new { id = x.id, employeeId = x.employeeId + "-" + x.name });
            ViewBag.employeeId = new SelectList(employee, "id", "employeeId", tPIAllocation.employeeId);
           // ViewBag.date = "01/07/2020 0:00:00";
            return View(tPIAllocation);
        }

        // POST: TPIAllocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,employeeId,vendorId,LocationId,companyId,date,start,finish,status,remark")] TPIAllocation tPIAllocation)
        {
            if (ModelState.IsValid)
            {
                if (tPIAllocation.status == "Completed" && tPIAllocation.finish == null)
                {
                    tPIAllocation.finish = DateTime.Now;
                }
                db.Entry(tPIAllocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName", tPIAllocation.companyId);
            ViewBag.vendorId = new SelectList(db.Vendors, "id", "name");
            ViewBag.LocationId = new SelectList(db.Locations, "id", "name");
            var employee = db.Employees.Select(x => new { id = x.id, employeeId = x.employeeId + "-" + x.name });
            ViewBag.employeeId = new SelectList(employee, "id", "employeeId", tPIAllocation.employeeId);
            return View(tPIAllocation);
        }

        // GET: TPIAllocations/Delete/5
        [CustomAuthorize(Roles = "Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPIAllocation tPIAllocation = db.TPIAllocations.Find(id);
            db.TPIAllocations.Remove(tPIAllocation);
            db.SaveChanges();

            if (tPIAllocation == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: TPIAllocations/Delete/5
        [CustomAuthorize(Roles = "Admin,Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TPIAllocation tPIAllocation = db.TPIAllocations.Find(id);
            db.TPIAllocations.Remove(tPIAllocation);
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
