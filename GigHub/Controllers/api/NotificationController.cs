using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using GigHub.Dtos;
using AutoMapper;
namespace GigHub.Controllers.api
{
    [Authorize]
    public class NotificationController : ApiController
    {
        private ApplicationDbContext db;
        public NotificationController()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notification = db.UserNotifications.Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist).ToList();
            return notification.Select(Mapper.Map<Notification, NotificationDto>);
        }
        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            var notification = db.UserNotifications.Where(un => un.UserId == userId && !un.IsRead).ToList();
            notification.ForEach(n => n.Read());
            db.SaveChanges();
            return Ok();
        }
    }
}
