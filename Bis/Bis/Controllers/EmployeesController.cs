using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bis.Custom;
using Bis.Models;

namespace Bis.Controllers
{
    [CustomAuthorize(Roles = "Admin,Manager")]
    public class EmployeesController : Controller
    {
        private BISModel db = new BISModel();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Category).Include(e => e.Department).Include(e => e.SubCategory);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.Categories, "id", "name");
            ViewBag.departmentId = new SelectList(db.Departments, "id", "name");
            ViewBag.subCategoryId = new SelectList(db.SubCategories, "id", "name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,employeeId,categoryId,subCategoryId,departmentId,name,fatherNmae,age,gender,dob,maritalStatus,email,phoneNumber,adharNumber,bloodGroup,address,communicationAddress,designation,doj,bankName,branchName,holderName,accountNo,ifscCode,panNo,institutionName,degree,yearofCompletion,university,percentage,ndeQualificationType,expiryDate,industryName,salary,periodFrom,periodTo,reason,salaryType,bisSalary,uniformIssueDate,shoeIssueDate,status,grade,esi,pf,insuranceCategory,photo,createUser")] Employee employee, HttpPostedFileBase photo,string employeeId)
        {
           
            if (ModelState.IsValid)
            {

                if (db.Employees.Any(x => x.employeeId.Equals(employeeId)))
                {
                    ViewBag.employeeID = "The employeeId is already exist";
                }
                else
                {
                    if (photo != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(photo.FileName));
                        photo.SaveAs(path);
                        employee.photo = "/Content/Images/" + Path.GetFileName(photo.FileName);
                    }
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    if (employee.createUser) { AddUser(employee); }
                    return RedirectToAction("Index");
                }
               
            }

            ViewBag.categoryId = new SelectList(db.Categories, "id", "name", employee.categoryId);
            ViewBag.departmentId = new SelectList(db.Departments, "id", "name", employee.departmentId);
            ViewBag.subCategoryId = new SelectList(db.SubCategories, "id", "name", employee.subCategoryId);
            return View(employee);
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Method 2 Get file details from HttpPostedFileBase class    

                    if (file != null)
                    {
                        string path = Path.Combine(Server.MapPath("D:/GitHub/bis/Bis/Bis/Content/images"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                    }
                    ViewBag.FileStatus = "File uploaded successfully.";
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading."; ;
                }
            }
            return View("Index");
        }


        public void AddUser(Employee employee)
        {
            //add entry to user List
            User enUser = db.Users.FirstOrDefault(x => x.username == employee.employeeId);
            if (enUser == null)
            {
                enUser = new User();
                enUser.name = employee.name;
                enUser.username = employee.employeeId;
                enUser.password = "123456";
                enUser.status = "Active";
                enUser.role = "Employee";
                db.Users.Add(enUser);
                db.SaveChanges();
            }
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryId = new SelectList(db.Categories, "id", "name", employee.categoryId);
            ViewBag.departmentId = new SelectList(db.Departments, "id", "name", employee.departmentId);
            ViewBag.subCategoryId = new SelectList(db.SubCategories, "id", "name", employee.subCategoryId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,employeeId,categoryId,subCategoryId,departmentId,name,fatherNmae,age,gender,dob,maritalStatus,email,phoneNumber,adharNumber,bloodGroup,address,communicationAddress,designation,doj,bankName,branchName,holderName,accountNo,ifscCode,panNo,institutionName,degree,yearofCompletion,university,percentage,ndeQualificationType,expiryDate,industryName,salary,periodFrom,periodTo,reason,salaryType,bisSalary,uniformIssueDate,shoeIssueDate,status,grade,esi,pf,insuranceCategory,createUser")] Employee employee, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(photo.FileName));
                    photo.SaveAs(path);
                    employee.photo = "/Content/Images/" + Path.GetFileName(photo.FileName);
                }
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                if (employee.createUser) { AddUser(employee); }
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.Categories, "id", "name", employee.categoryId);
            ViewBag.departmentId = new SelectList(db.Departments, "id", "name", employee.departmentId);
            ViewBag.subCategoryId = new SelectList(db.SubCategories, "id", "name", employee.subCategoryId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();

            if (employee == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
