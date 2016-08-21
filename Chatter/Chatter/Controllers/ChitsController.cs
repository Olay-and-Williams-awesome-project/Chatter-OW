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
using QRCoder;
using System.Drawing;
using System.IO;

namespace Chatter.Controllers
{
    public class ChitsController : Controller
    {
        byte[] byteArray;
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser
        {
            get
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
                return currentUser;
            }
        }

        // GET: Chits
        public ActionResult Index()
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

            if(CurrentUser.Followers.Count() == 0)
            {
                ViewBag.Followers = null;
            }else
            {
                ViewBag.Followers = CurrentUser.Followers.ToList();
            }

            ViewBag.CurrentUser = CurrentUser;

            return View(db.Chits.OrderByDescending(o => o.ChitCreatedAt).ToList());
        }

        private Bitmap renderQRCode()
        {
            string url = Request.Url.ToString();
            PayloadGenerator.Url myUrl = new PayloadGenerator.Url(url);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(myUrl.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            //Bitmap qrCodeImage = qrCode.GetGraphic(20);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen, true);

            return qrCodeImage;
        }

        public FileContentResult myAction()
        {
            Bitmap qrCodeImage = renderQRCode();
            byteArray = ImageToByte2(qrCodeImage);
            return new FileContentResult(byteArray, "image/jpg");
        }

        public static byte[] ImageToByte2(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public void getFollowers()
        {
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser CurrentUser = UserManager.FindById(User.Identity.GetUserId());

            var followingID = from u in CurrentUser.Following
                              select u.Id;

            var chits = db.Chits.Where(p => followingID.Contains(p.User.Id)).ToList();

            ViewBag.AllUsers = from u in UserManager.Users
                               select u.UserName;

            if (CurrentUser.Followers.Count() == 0)
            {
                ViewBag.Followers = null;
            }
            else
            {
                ViewBag.Followers = CurrentUser.Followers.ToList();
            }

            ViewBag.AllUSers1 = UserManager.Users;
            ViewBag.CurrentUser = CurrentUser;
        }

        //public void getChitStream()
        //{
        //    UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //    ApplicationUser CurrentUser = UserManager.FindById(User.Identity.GetUserId());

            

        //}

        public List<Chit> search(string String)
        {
            var chits = from item in db.Chits
                        select item;
            if (!String.IsNullOrEmpty(String))
            {
                chits = from item in chits
                        where item.User.DisplayTitle.Contains(String) ||
                              item.User.UserName.Contains(String)
                        select item;
            }
            return chits.ToList();
        }

        // GET: Chits/Following
        public ActionResult Following(string searchString)
        {
            //var chits = from item in db.Chits
            //            select item;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    chits = from item in chits
            //               where item.User.DisplayTitle.Contains(searchString) ||
            //                     item.User.UserName.Contains(searchString)
            //               select item;
            //}
            var chits = search(searchString);
            getFollowers();
            return View(chits);
        }

        // GET: Chits/Feed
        public ActionResult Feed()
        {
            getFollowers();
            return View(db.Chits.OrderByDescending(o=>o.ChitCreatedAt).ToList());
        }

        // POST: Chits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feed([Bind(Include = "ChitID,ChitText")] Chit chit)
        {
            chit.ChitCreatedAt = DateTime.Now;
            chit.User = CurrentUser;
            if (ModelState.IsValid)
            {
                db.Chits.Add(chit);
                db.SaveChanges();
                return RedirectToAction("Feed");
            }
            var chits = from item in db.Chits
                        select item;

            chits = from item in chits
                    orderby item.ChitCreatedAt
                    select item;

            return View(chits);
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
        public ActionResult Create([Bind(Include = "ChitID,ChitText")] Chit chit)
        {
            chit.ChitCreatedAt = DateTime.Now;
            chit.User = CurrentUser;
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
