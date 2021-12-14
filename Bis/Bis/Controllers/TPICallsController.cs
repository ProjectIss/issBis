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
    public class TPICallsController : Controller
    {
        private BISModel db = new BISModel();

        // GET: TPICalls
        public ActionResult Index()
        {

            //if employee 
            if (Display.Role == "Admin" || Display.Role == "Manager")
            {
                var tPICalls = db.TPICalls.Include(x => x.TPIAllocation).OrderByDescending(x => x.id).Take(100);
                return View(tPICalls.ToList());
            }
            else
            {
                var tPICalls = db.TPICalls.Include(x => x.TPIAllocation).Where(x => x.TPIAllocation.employeeId == Display.UserId).OrderByDescending(x => x.id).Take(100);
                return View(tPICalls.ToList());
            }

        }

        // GET: TPICalls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPICall tPICall = db.TPICalls.Find(id);
            if (tPICall == null)
            {
                return HttpNotFound();
            }
            return View(tPICall);
        }

        // GET: TPICalls/Create
        public ActionResult Create()
        {
            var tpiallocation = db.TPIAllocations.Where(f => f.employeeId == Custom.Display.UserId && f.status != "Completed").Select(x => new { id = x.id, title = x.title + "-" + x.Company.companyName }).ToList();
            ViewBag.tPIAllocationId = new SelectList(tpiallocation, "id", "title");
            return View();
        }

        // POST: TPICalls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tPIAllocationId,date,reportingQC,plant,productGroup,inTime,offeringTime,outTime,idleTime,days,totalQTYoffered,noofOkCasting,ftp,stp,rw,hold,rejected,scopeInspection,status")] TPICall tPICall)
        {
            if (ModelState.IsValid)
            {
                var enAllocation = db.TPIAllocations.FirstOrDefault(x => x.id == tPICall.tPIAllocationId);
                if (enAllocation != null)
                {
                    if (enAllocation.start == null && enAllocation.status == "New")
                    {
                        enAllocation.start = DateTime.Now;
                        enAllocation.status = "InProgress";
                        tPICall.createdAt = DateTime.Now;
                        db.TPICalls.Add(tPICall);
                        db.SaveChanges();
                    }
                }

               
                return RedirectToAction("Index");
            }
            var tpiallocation = db.TPIAllocations.Where(f => f.employeeId == Custom.Display.UserId && f.status != "Completed").Select(x => new { id = x.id, title = x.title + "-" + x.Company.companyName });
            ViewBag.tPIAllocationId = new SelectList(tpiallocation, "id", "title", tPICall.tPIAllocationId);
            return View(tPICall);
        }

        // GET: TPICalls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPICall tPICall = db.TPICalls.Find(id);
            if (tPICall == null)
            {
                return HttpNotFound();
            }
            var tpiallocation = db.TPIAllocations.Where(f => f.employeeId == Custom.Display.UserId && f.status != "Completed").Select(x => new { id = x.id, title = x.title + "-" + x.Company.companyName });
            ViewBag.tPIAllocationId = new SelectList(tpiallocation, "id", "title", tPICall.tPIAllocationId);
            return View(tPICall);
        }

        // POST: TPICalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tPIAllocationId,date,reportingQC,plant,productGroup,inTime,offeringTime,outTime,idleTime,days,totalQTYoffered,noofOkCasting,ftp,stp,rw,hold,rejected,scopeInspection,status")] TPICall tPICall)
        {
            if (ModelState.IsValid)
            {
                tPICall.modifiedAt = DateTime.Now;
                db.Entry(tPICall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var tpiallocation = db.TPIAllocations.Where(f => f.employeeId == Custom.Display.UserId && f.status != "Completed").Select(x => new { id = x.id, title = x.title + "-" + x.Company.companyName });
            ViewBag.tPIAllocationId = new SelectList(tpiallocation, "id", "title", tPICall.tPIAllocationId);
            return View(tPICall);
        }

        // GET: TPICalls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPICall tPICall = db.TPICalls.Find(id);
            db.TPICalls.Remove(tPICall);
            db.SaveChanges();

            if (tPICall == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: TPICalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TPICall tPICall = db.TPICalls.Find(id);
            db.TPICalls.Remove(tPICall);
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
