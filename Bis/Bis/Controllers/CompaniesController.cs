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
    [CustomAuthorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private BISModel db = new BISModel();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.Location);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.locationId = new SelectList(db.Locations, "id", "name");
            ViewBag.companyCategoryId = new SelectList(db.CompanyCategories, "id", "name","description");

            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,locationId,companyName,companyCategoryId,phoneNo,companyDescription,companyAddress,email,gSTIN,pinCode,stateCode,cityCode,vendorCode,state,city,companyDetails")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.locationId = new SelectList(db.Locations, "id", "name", company.locationId);
            ViewBag.companyCategoryId = new SelectList(db.CompanyCategories, "id", "name", "description", company.companyCategoryId);
            return View();
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.locationId = new SelectList(db.Locations, "id", "name", company.locationId);
            ViewBag.companyCategoryId = new SelectList(db.CompanyCategories, "id", "name", "description", company.companyCategoryId);
            return View();
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,locationId,companyName,companyCategoryId,phoneNo,companyDescription,companyAddress,email,gSTIN,pinCode,stateCode,cityCode,vendorCode,state,city,companyDetails")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.locationId = new SelectList(db.Locations, "id", "name", company.locationId);
            ViewBag.companyCategoryId = new SelectList(db.CompanyCategories, "id", "name", "description", company.companyCategoryId);
            return View();
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            if (company == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
