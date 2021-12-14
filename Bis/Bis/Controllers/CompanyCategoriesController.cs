using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bis.Models;

namespace Bis.Controllers
{
    public class CompanyCategoriesController : Controller
    {
        private BISModel db = new BISModel();

        // GET: CompanyCategories
        public ActionResult Index()
        {
            return View(db.CompanyCategories.ToList());
        }

        // GET: CompanyCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyCategory companyCategory = db.CompanyCategories.Find(id);
            if (companyCategory == null)
            {
                return HttpNotFound();
            }
            return View(companyCategory);
        }

        // GET: CompanyCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,categoryDayCost,categoryOTCost,description")] CompanyCategory companyCategory)
        {
            if (ModelState.IsValid)
            {
                db.CompanyCategories.Add(companyCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyCategory);
        }

        // GET: CompanyCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyCategory companyCategory = db.CompanyCategories.Find(id);
            if (companyCategory == null)
            {
                return HttpNotFound();
            }
            return View(companyCategory);
        }

        // POST: CompanyCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,categoryDayCost,categoryOTCost,description")] CompanyCategory companyCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyCategory);
        }

        // GET: CompanyCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyCategory companyCategory = db.CompanyCategories.Find(id);
            db.CompanyCategories.Remove(companyCategory);
            db.SaveChanges();
            if (companyCategory == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: CompanyCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyCategory companyCategory = db.CompanyCategories.Find(id);
            db.CompanyCategories.Remove(companyCategory);
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
