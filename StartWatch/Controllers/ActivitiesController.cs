using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StartWatch.Models;
using Microsoft.AspNet.Identity;

namespace StartWatch.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<Activity> GetActivities()
        {
            List<Activity> UserActivities = new List<Activity>();
            foreach (var Activity in db.Activities.ToList())
            {
                if (Activity.UserId == User.Identity.GetUserId())
                {
                    UserActivities.Add(Activity);
                }
            }
            return UserActivities;
        }


        // GET: Activities
        public ActionResult Index()
        {

            // get current activity
            ApplicationUser thisUser = db.Users.Find(User.Identity.GetUserId());
            Activity[] ActivitiesArray = GetActivities().ToArray();
            Activity CurrentActivity;
            if (ActivitiesArray.Count() < 1)
            {
                return RedirectToAction("Manage", "Activities");
            }
            else
            {
                if (thisUser.CurrentActivity < 0)
                {
                    CurrentActivity = new Activity();
                    CurrentActivity.Name = "Pause";
                }
                else
                {
                    CurrentActivity = ActivitiesArray[thisUser.CurrentActivity];
                }

                Ping(User.Identity.GetUserId());

                // set values to show
                ViewBag.CurrentActName = CurrentActivity.Name;
                ViewBag.CurrentActToday = CurrentActivity.ProgressToday;
                ViewBag.CurrentActWeek = CurrentActivity.ProgressWeek;
                ViewBag.CurrentActTotal = CurrentActivity.ProgressTotal;
                ViewBag.CurrentActQuota = CurrentActivity.Quota;
                ViewBag.CurrentActNumber = thisUser.CurrentActivity;
                ViewBag.CurrentActAvgSession = CurrentActivity.SessionCount;

            }
            return View(GetActivities());
        }

        // GET: Activities/Set/?act=5&time=1273373
        // Called via AJAX when selector value is changed
        // Sets current activity while adding time to outgoing activity
        // Param: 'act' previous activity; 'time' time spent on Index page
        public ActionResult Set(int? act)
        {
            if (act == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // add time to outgoing activity
            Ping(User.Identity.GetUserId());


            // update current activity row
            ApplicationUser ThisUser = db.Users.Find(User.Identity.GetUserId());
            ThisUser.CurrentActivity = (int)act;
            db.Entry(ThisUser).State = EntityState.Modified;

            if (act > -1)       // if: setting not pausing, update session count
            {
                Activity[] UserActivities = GetActivities().ToArray();
                Activity CurrentActivity = UserActivities[(int)act];
                CurrentActivity.SessionCount++;
                db.Entry(CurrentActivity).State = EntityState.Modified;

                //Ping(User.Identity.GetUserId());
            }
            Object Lock = new Object();

            lock (Lock)
                db.SaveChanges();
            return RedirectToAction("Index", "Activities");
        }

        public ActionResult AdjustTime(int delta)
        {

            //// add time to outgoing activity
            //Ping(User.Identity.GetUserId());

            // add to session count for current activity
            ApplicationUser ThisUser = db.Users.Find(User.Identity.GetUserId());
            Activity[] UserActivities = GetActivities().ToArray();
            if (ThisUser.CurrentActivity > -1)
            {
                Activity CurrentActivity = UserActivities[(int)ThisUser.CurrentActivity];

                // add or remove time
                CurrentActivity.ProgressToday = Math.Max(CurrentActivity.ProgressToday + delta, 0);
                CurrentActivity.ProgressWeek = Math.Max(CurrentActivity.ProgressWeek + delta, 0);
                CurrentActivity.ProgressTotal = Math.Max(CurrentActivity.ProgressTotal + delta, 0);
                db.Entry(CurrentActivity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Activities");
        }

        // GET: Activities/Manage
        public ActionResult Manage()
        {
            return View(GetActivities());
        }

        // GET: Activities/Analyze
        public ActionResult Analyze()
        {
            Ping(User.Identity.GetUserId());
            return View(GetActivities());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Quota,ProgressToday,ProgressWeek,CreationDate,Required,Name")] Activity activity)
        {
            activity.UserId = User.Identity.GetUserId();
            activity.Quota *= 60;
            activity.ProgressToday = 0;
            activity.ProgressWeek = 0;
            activity.ProgressTotal = 0;
            activity.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();

                //Ping(activity.UserId);
                return RedirectToAction("Manage");
            }

            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            //Ping(User.Identity.GetUserId());
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Quota,ProgressToday,ProgressWeek,ProgressTotal,CreationDate,Required,Name")] Activity activity)
        {
            activity.Quota *= 60;
            activity.ProgressToday *= 60;
            activity.ProgressWeek *= 60;
            activity.ProgressTotal *= 60;
            activity.SessionCount = 1;
            activity.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                //Ping(User.Identity.GetUserId());
                return RedirectToAction("Manage");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            //Ping(User.Identity.GetUserId());
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();
            //Ping(User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

        // Called whenever returning to Select or Analyse pages
        // Updates total time spent doing current or recently changed activity
        private void Ping(string UserId, int act = -1)
        {
            // get current timestamp, user, and activity
            long LatestPing = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            ApplicationUser thisUser = db.Users.Find(UserId);
            Activity[] UserActivities = GetActivities().ToArray();
            Activity CurrentActivity;
            if (thisUser.CurrentActivity > -1)
            {
                if (act == -1)
                {
                    CurrentActivity = UserActivities[thisUser.CurrentActivity];
                }
                else
                {
                    CurrentActivity = UserActivities[act];
                }

                // update current activity progress (max 2 hours)
                int Duration = (int)Math.Min((LatestPing - thisUser.Ping), 7200);
                CurrentActivity.ProgressToday += Duration;
                CurrentActivity.ProgressWeek += Duration;
                CurrentActivity.ProgressTotal += Duration;

                if (Duration >= 7200)   // if: inactive for two hours, pause
                {
                    thisUser.CurrentActivity = -1;
                }

                thisUser.Ping = LatestPing;     // update ping to latest time
                if (ModelState.IsValid)
                {
                    db.Entry(thisUser).State = EntityState.Modified;
                    db.Entry(CurrentActivity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Ping(User.Identity.GetUserId());
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
