using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chatter.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chatter.Controllers
{
    public class ChitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chits
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser CurrentUser = UserManager.FindById(User.Identity.GetUserId());

                var followingID = from u in CurrentUser.Following
                                  select u.Id;

                var chits = db.Chits.Where(p => followingID.Contains(p.User.Id)).ToList();


                //if (chits.Count == 0)
                //{
                //    chits = db.Chits.ToList();
                //}

                ViewBag.AllUsers = from u in UserManager.Users
                                   select u.UserName;

                ViewBag.CurrentUser = CurrentUser;
                return View(chits);
            }

           
             
            return View(db.Chits.ToList());
        }

        public ActionResult Follow(int? id)
        {
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser CurrentUser = UserManager.FindById(User.Identity.GetUserId());
            CurrentUser.Following.Add(db.Users.Find(id));
            db.SaveChanges();

            return View();
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
