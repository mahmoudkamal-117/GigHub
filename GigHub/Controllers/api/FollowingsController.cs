using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using GigHub.Dtos;
namespace GigHub.Controllers.api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private ApplicationDbContext db;
        public FollowingsController()
        {
            db = new ApplicationDbContext();
        }

        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            if (db.Followings.Any(f => f.FollowerId == userId && f.FolloweeId== dto.FolloweeId))
                return BadRequest("Following Already Exist!");
            var following = new Following
            {
                FollowerId=userId,
                FolloweeId=dto.FolloweeId
            };
            db.Followings.Add(following);
            db.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = db.Followings.SingleOrDefault(f => f.FollowerId == userId && f.FolloweeId == id);
            if (following == null)
                return NotFound();
            db.Followings.Remove(following);
            db.SaveChanges();
            return Ok(id);
        }
    }
}
