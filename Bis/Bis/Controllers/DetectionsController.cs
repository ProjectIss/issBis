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
    [CustomAuthorize(Roles = "Admin,Manager")]
    public class DetectionsController : Controller
    {
        private BISModel db = new BISModel();

        // GET: Detections
        public ActionResult Index()
        {
            var detections = db.Detections.Include(d => d.Employee);
          
            return View(detections.ToList());
        }
       
        // GET: Detections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detection detection = db.Detections.Find(id);
            if (detection == null)
            {
                return HttpNotFound();
            }
            return View(detection);
        }

        // GET: Detections/Create
        public ActionResult Create()
        {
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId");
            return View();
        }

        // POST: Detections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,employeeId,date,advance,loan,bonus,tds,certificationFees,travelAllowance,otherAllowance,cashVoucher,remak")] Detection detection)
        {
            if (ModelState.IsValid)
            {
                db.Detections.Add(detection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", detection.employeeId);
            return View(detection);
        }

        // GET: Detections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detection detection = db.Detections.Find(id);
            if (detection == null)
            {
                return HttpNotFound();
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", detection.employeeId);
            return View(detection);
        }

        // POST: Detections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,employeeId,date,advance,loan,bonus,tds,certificationFees,travelAllowance,otherAllowance,cashVoucher,remak")] Detection detection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", detection.employeeId);
            return View(detection);
        }

        // GET: Detections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detection detection = db.Detections.Find(id);
            db.Detections.Remove(detection);
            db.SaveChanges();

            if (detection == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: Detections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detection detection = db.Detections.Find(id);
            db.Detections.Remove(detection);
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
