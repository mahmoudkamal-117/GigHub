﻿using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        public HomeController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult Index(string query=null)
        {
            var upcomingGigs = db.Gigs
                .Include(g => g.Artist)
                .Include(g=> g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);
            if (!String.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs.Where(g =>
                    g.Artist.Name.Contains(query) ||
                    g.Genre.Name.Contains(query) ||
                    g.Venue.Contains(query));
            }

            var userId = User.Identity.GetUserId();
            var attendances = db.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);



            var viewModel = new GigsViewModel()
            {
                UpcomingGigs=upcomingGigs,
                ShowActions=User.Identity.IsAuthenticated,
                Heading="Upcoming Gigs ",
                SearchTerm=query,
                Attendances=attendances
            };
            return View("Gigs",viewModel);
        }
       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}