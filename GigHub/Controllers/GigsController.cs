using GigHub.Models;
using GigHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext db;
        public GigsController()
        {
            db = new ApplicationDbContext();
        }
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = db.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime>DateTime.Now && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();
            return View(gigs);
        }
        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            var gigs = db.Followings.Where(f => f.FollowerId == userId).Include(f => f.Followee).ToList();
            return View(gigs);
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = db.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

           
            var attendances = db.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);


            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading="Gigs I'm Attending",
                Attendances=attendances
            };
            return View("Gigs",viewModel);
        } 
        // GET: Gigs
        [Authorize]
        public ActionResult Create()
        {
            var viewmodel= new GigsFormViewModel
            {
                Genres=db.Genres.ToList(),
                Heading="Add a Gig"
            };
            return View("GigForm",viewmodel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigsFormViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                viewmodel.Genres = db.Genres.ToList();
                return View("GigForm", viewmodel);
            }
            var gig = new Gig
            {
                
                ArtistId=User.Identity.GetUserId(),
                DateTime= viewmodel.GetDateTime(),
                GenreId=viewmodel.Genre,
                Venue=viewmodel.Venue,
                
            };
            db.Gigs.Add(gig);
            db.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = db.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewmodel = new GigsFormViewModel
            {
                Id=gig.Id,
                Genres = db.Genres.ToList(),
               Venue=gig.Venue,
               Date=gig.DateTime.ToString("dd MMM yyyy"),
               Time=gig.DateTime.ToString("HH:mm"),
               Genre=gig.GenreId,
               Heading="Edit a Gig"
            };
            return View("GigForm",viewmodel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigsFormViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                viewmodel.Genres = db.Genres.ToList();
                return View("GigForm", viewmodel);
            }
            var userId=User.Identity.GetUserId();
            var gig = db.Gigs.Include(g=>g.Attendances.Select(a=>a.Attendee))
                .Single(g => g.Id == viewmodel.Id && g.ArtistId == userId);
            gig.Modify(viewmodel.GetDateTime(), viewmodel.Venue, viewmodel.Genre);
            db.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }
        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        public ActionResult Details(int id)
        {
            var gig = db.Gigs.Include(g => g.Artist).Include(g => g.Genre).SingleOrDefault(g => g.Id == id);

            if (gig == null)
                return HttpNotFound();
            var viewModel = new GigDetailsViewModel { Gig = gig };
                if(User.Identity.IsAuthenticated)
                {
                    var userId = User.Identity.GetUserId();
                    viewModel.IsAttending = db.Attendances.Any(a => a.GigId == gig.Id && a.AttendeeId == userId);
                    viewModel.IsFollowing = db.Followings.Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
                }
                return View("Details", viewModel);
        }

    }
}