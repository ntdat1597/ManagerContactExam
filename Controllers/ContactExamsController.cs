using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagerContactExam.Models;
using PagedList;

namespace ManagerContactExam.Controllers
{
    public class ContactExamsController : Controller
    {
        private ManagerContactsInformationEntities db = new ManagerContactsInformationEntities();

        // GET: ContactExams
        public ActionResult Index()
        {
            return View(db.ContactExams.ToList());
        }

        public ViewResult Index(string sortOrder, string search, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;
            var contacts = from s in db.ContactExams select s;
            if (!String.IsNullOrEmpty(search))
            {
                contacts = contacts.Where(s => s.ContactName.Contains(search));

            }
            switch (sortOrder)
            {
                case "name desc":
                    contacts = contacts.OrderByDescending(s => s.ContactName);
                    break;

                default:
                    contacts = contacts.OrderBy(s => s.ContactName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(contacts.ToPagedList(pageNumber, pageSize));

        }


        // GET: ContactExams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactExam contactExam = db.ContactExams.Find(id);
            if (contactExam == null)
            {
                return HttpNotFound();
            }
            return View(contactExam);
        }

        // GET: ContactExams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactExams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ContactName,ContactNumber,GroupName,Hiredate,Birthday")] ContactExam contactExam)
        {
            if (ModelState.IsValid)
            {
                db.ContactExams.Add(contactExam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactExam);
        }

        // GET: ContactExams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactExam contactExam = db.ContactExams.Find(id);
            if (contactExam == null)
            {
                return HttpNotFound();
            }
            return View(contactExam);
        }

        // POST: ContactExams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ContactName,ContactNumber,GroupName,Hiredate,Birthday")] ContactExam contactExam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactExam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactExam);
        }

        // GET: ContactExams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactExam contactExam = db.ContactExams.Find(id);
            if (contactExam == null)
            {
                return HttpNotFound();
            }
            return View(contactExam);
        }

        // POST: ContactExams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactExam contactExam = db.ContactExams.Find(id);
            db.ContactExams.Remove(contactExam);
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
