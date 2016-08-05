using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chatter.Models;

namespace Chatter.Controllers
{
    public class ChitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chits
        public ActionResult Index()
        {
            return View(db.Chits.ToList());
        }

        // GET: Chits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chit chit = db.Chits.Find(id);
            if (chit == null)
            {
                return HttpNotFound();
            }
            return View(chit);
        }

        // GET: Chits/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult Create([Bind(Include = "ChitID,ChitText,ChitCreatedAt")] Chit chit)
        {
            if (ModelState.IsValid)
            {
                db.Chits.Add(chit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chit);
        }

        // GET: Chits/Edit/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chit chit = db.Chits.Find(id);
            if (chit == null)
            {
                return HttpNotFound();
            }
            return View(chit);
        }

        // POST: Chits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit([Bind(Include = "ChitID,ChitText,ChitCreatedAt")] Chit chit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chit);
        }

        // GET: Chits/Delete/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chit chit = db.Chits.Find(id);
            if (chit == null)
            {
                return HttpNotFound();
            }
            return View(chit);
        }

        // POST: Chits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult DeleteConfirmed(int id)
        {
            Chit chit = db.Chits.Find(id);
            db.Chits.Remove(chit);
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
