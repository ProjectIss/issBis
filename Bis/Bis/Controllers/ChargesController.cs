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
    public class ChargesController : Controller
    {
        private BISModel db = new BISModel();

        // GET: Charges
        public ActionResult Index()
        {
            var charges = db.Charges.Include(c => c.Company).Include(c => c.Location);
            return View(charges.ToList());
        }

        // GET: Charges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Charge charge = db.Charges.Find(id);
            if (charge == null)
            {
                return HttpNotFound();
            }
            return View(charge);
        }

        // GET: Charges/Create
        public ActionResult Create()
        {
            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName");
            ViewBag.locationId = new SelectList(db.Locations, "id", "name");
            return View();
        }

        // POST: Charges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,locationId,companyId,employeeStayCharge,employeeVisitCharge,companyStayCharge,companyVisitCharge,employeeClaimCharge,companyClaimCharge,remark")] Charge charge)
        {
            if (ModelState.IsValid)
            {
                db.Charges.Add(charge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName", charge.companyId);
            ViewBag.locationId = new SelectList(db.Locations, "id", "name", charge.locationId);
            return View(charge);
        }

        // GET: Charges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Charge charge = db.Charges.Find(id);
            if (charge == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName", charge.companyId);
            ViewBag.locationId = new SelectList(db.Locations, "id", "name", charge.locationId);
            return View(charge);
        }

        // POST: Charges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,locationId,companyId,employeeStayCharge,employeeVisitCharge,companyStayCharge,companyVisitCharge,employeeClaimCharge,companyClaimCharge,remark")] Charge charge)
        {
            if (ModelState.IsValid)
            {
                db.Entry(charge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.companyId = new SelectList(db.Companies, "id", "companyName", charge.companyId);
            ViewBag.locationId = new SelectList(db.Locations, "id", "name", charge.locationId);
            return View(charge);
        }

        // GET: Charges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Charge charge = db.Charges.Find(id);
            db.Charges.Remove(charge);
            db.SaveChanges();
            if (charge == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: Charges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Charge charge = db.Charges.Find(id);
            db.Charges.Remove(charge);
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
