﻿using System;
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
    public class LoansController : Controller
    {
        private BISModel db = new BISModel();

        // GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Employee);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create()
        {
            // ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId");
            var employees = db.Employees.OrderBy(x => x.employeeId).Select(s => new
            {
                Text=s.employeeId +" - "+s.name,
                value=s.id
            }).ToList();
            ViewBag.employeeId = new SelectList(employees, "Value", "Text");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,employeeId,date,loanAmount,reason")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var employees = db.Employees.OrderBy(x => x.employeeId).Select(s => new
            {
                Text = s.employeeId + " - " + s.name,
                value = s.id
            }).ToList();
            ViewBag.employeeId = new SelectList(employees, "Value", "Text",loan.employeeId);
            return View(loan);
        }

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            var employees = db.Employees.OrderBy(x => x.employeeId).Select(s => new
            {
                Text = s.employeeId + " - " + s.name,
                value = s.id
            }).ToList();
            ViewBag.employeeId = new SelectList(employees, "Value", "Text",loan.employeeId);
            // ViewBag.employeeId = new SelectList(db.Employees, "id", "employeeId", loan.employeeId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,employeeId,date,loanAmount,reason")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var employees = db.Employees.OrderBy(x => x.employeeId).Select(s => new
            {
                Text = s.employeeId + " - " + s.name,
                value = s.id
            }).ToList();
            ViewBag.employeeId = new SelectList(employees, "Value", "Text",loan.employeeId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
            db.SaveChanges();

            if (loan == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
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