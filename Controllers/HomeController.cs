using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myCircle.Models;

//==========================================================================================================
namespace myCircle.Controllers
{
    public class HomeController : Controller
    {
        private Context dbContext;

        public HomeController(Context context)
        {
            dbContext = context;
        }
        //==========================================================================================================
        //USER SETTINGS AND STUFF
        //==========================================================================================================
        [HttpPost("/registration")]//REGISTRATION
        public JsonResult registration([FromBody] register newUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.users.Any(u => u.email == newUser.email))
                {
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("email", "Email is already in use");
                    return Json(error);
                }
                if (dbContext.users.Any(u => u.username == newUser.username))
                {
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("username", "Username is already in use");
                    return Json(error);
                }
                PasswordHasher<register> Hasher = new PasswordHasher<register>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                users usertoAdd= new users{
                   username=newUser.username,
                   password=newUser.password,
                   email=newUser.email,
                };
                dbContext.users.Add(usertoAdd);
                dbContext.SaveChanges();
                users User= dbContext.users.FirstOrDefault(x=>x.email==newUser.email);
                HttpContext.Session.SetInt32("UserId", User.userId);

                Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
            }
            else
            {
                return Json(ModelState);
            }
        }

        [HttpPost("/loggingIn")]//LOGIN
        public IActionResult loginUser([FromBody] Login exUser)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.email == exUser.email);
                Dictionary<string, string> error = new Dictionary<string, string>();
                if (userInDb == null)
                {
                    error.Add("Message", "Error");
                    error.Add("email", "Invalid email");
                    return Json(error);
                }
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(exUser, userInDb.password, exUser.password);

                if (result == 0)
                {
                    error.Add("Message", "Error");
                    error.Add("password", "Invalid password");
                    return Json(error);
                }

                HttpContext.Session.SetInt32("UserId", userInDb.userId);
                Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
            }
            else
            {
                return Json(ModelState);
            }
        }
        [HttpGet("/checkLogin")]
        public IActionResult checkLogin(){
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
            else{
                Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
            }
        }

        [HttpPost("/update")]//USE THIS TO UPDATE USER
        public IActionResult editUser([FromBody] users updateUser)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    return Json(error);
            }
            users RetrievedUser = dbContext.users.FirstOrDefault(users => users.userId == updateUser.userId);
            //===================================================================================================
            List<users> oneUser = dbContext.users.Where(usr => usr.userId == updateUser.userId).ToList();
            List<users> butOne = dbContext.users.Except(oneUser).ToList();
            //===================================================================================================
            if (ModelState.IsValid)
            {
                if (butOne.Any(u => u.email == updateUser.email))
                {
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("email", "Email is already in use");
                    return Json(error);
                }
                else
                {
                    RetrievedUser.username = updateUser.username;
                    RetrievedUser.email = updateUser.email;
                    RetrievedUser.password = updateUser.password;
                    dbContext.SaveChanges();
                    Dictionary<string, string> success = new Dictionary<string, string>();
                    success.Add("Message", "Success");
                    return Json(success);
                }
            }
            else
            {
                return Json(ModelState);
            }
        }
        [HttpGet("/delete/{id}")]//USE THIS TO DELETE
        public IActionResult deletor(int id)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    return Json(error);
            }
            users ReturnedValues = dbContext.users.FirstOrDefault(users => users.userId == id);
            dbContext.users.Remove(ReturnedValues);
            dbContext.SaveChanges();

            Dictionary<string, string> success = new Dictionary<string, string>();
            success.Add("Message", "Success");
            return Json(success);
        }
        [HttpGet("logout")]
        public IActionResult logout(){
            HttpContext.Session.Clear();
            Dictionary<string, string> success = new Dictionary<string, string>();
            success.Add("Message", "Success");
            return Json(success);
        }
        //==========================================================================================================
        //INVITE FUNCTIONS
        //==========================================================================================================
        [HttpGet("/inviteCircle/{id}/{email}")]
        public IActionResult inviteCircle(int id, string email)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("hacking", "hacker");
                    return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            //=========================================
            if(dbContext.users.Any(u=>u.email==email)){
                users addUser = dbContext.users.FirstOrDefault(u=>u.email==email);
                if(dbContext.circles.Any(x=>x.circleId == id)){
                    circles circ = dbContext.circles.FirstOrDefault(x=>x.circleId == id);
                    if(circ.userId == userId){
                        invites newInvite = new invites{
                            userId = addUser.userId,
                            User = addUser,
                            circleId = circ.circleId,
                            circle = circ
                        };
                        dbContext.invites.Add(newInvite);
                        dbContext.SaveChanges();

                        Dictionary<string, string> success = new Dictionary<string, string>();
                        success.Add("Message", "Success");
                        return Json(success);
                    }
                    else{
                        Dictionary<string, string> error = new Dictionary<string, string>();
                        error.Add("Message", "Error");
                        error.Add("hacking", "Not allowed to invite");
                        return Json(error);
                    }
                }
                else{
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("hacking", "Circle does not exist");
                    return Json(error);
                }
            }else{
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                error.Add("email", "Email does not exist");
                return Json(error);
            }
            //=========================================
        }
        [HttpGet("/joinCircle/{inviteId}")]
        public IActionResult joinCircle(int inviteId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("hacking", "hacker");
                    return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            users addUser = dbContext.users.FirstOrDefault(u=>u.userId==userId);
            invites currentInvite = dbContext.invites.FirstOrDefault(i=>i.inviteId==inviteId);
            circles circ = dbContext.circles.FirstOrDefault(x=>x.circleId == currentInvite.circleId);
            if(currentInvite.userId == userId){
            //=========================================
                if(dbContext.circles.Any(c=> c == circ)){
                    usercircles newUCircle = new usercircles{
                        userId = userId,
                        User = addUser,
                        circleId = circ.circleId,
                        circle = circ
                    };
                    dbContext.userCircles.Add(newUCircle);
                    dbContext.SaveChanges();
                    dbContext.invites.Remove(currentInvite);
                    dbContext.SaveChanges();
                    Dictionary<string, string> success = new Dictionary<string, string>();
                    success.Add("Message", "Success");
                    return Json(success);
                }
                else{
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("hacking", "Circle does not exist");
                    return Json(error);
                }
            }
            else{
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                error.Add("hacking", "Invalid invite");
                return Json(error);
            }
            //=========================================
        }
        [HttpGet("/deleteInvite/{id}")]
        public IActionResult deleteInvite(int id)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                error.Add("hacking", "hacker");
                return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            //=========================================
            if(dbContext.invites.Any(u=>u.inviteId==id)){
                invites inviteDelete = dbContext.invites.FirstOrDefault(u=>u.inviteId==id);
                dbContext.invites.Remove(inviteDelete);
                dbContext.SaveChanges();

                Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
            }else{
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                error.Add("hacking", "Invite does not exist");
                return Json(error);
            }
            //=========================================
        }
        //==========================================================================================================
        //CIRCLE FUNCTIONS
        //==========================================================================================================
        [HttpGet("/userCirclesData")]//USE THIS TO RETRIEVE ALL CHANNELS AND MESSAGES IN A SPECIFIC CIRCLE
        public IActionResult userCirclesData()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    return Json(error);
            }
            IndexView index = new IndexView();
            int userID = HttpContext.Session.GetInt32("UserId") ?? default(int);
            index.usercircles = dbContext.userCircles.Where(g=>g.userId==userID).Include(c=>c.circle).ThenInclude(ch=>ch.channels).ThenInclude(m=>m.messages).ThenInclude(u=>u.User).Include(c=>c.circle).ThenInclude(ch=>ch.channels).ThenInclude(m=>m.messages).ThenInclude(l=>l.likes).ToList();
            index.user = dbContext.users.Where(u=>u.userId==userID).Include(i=>i.invites).ThenInclude(c=>c.circle).FirstOrDefault();
            return Json(index);
        }
        [HttpGet("/manageUsers/{id}")]//USE THIS TO RETRIEVE ALL USERS IN A SPECIFIC CIRCLE
        public IActionResult manageUsers(int id)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if(dbContext.userCircles.Any(c=>c.circleId == id)){
                if(dbContext.circles.Any(c=>c.userId==userId)){
                    List<usercircles> userCirc = dbContext.userCircles.Where(c=>c.circleId == id).Include(u=>u.User).Include(c=>c.circle).ThenInclude(ch=>ch.channels).ThenInclude(m=>m.messages).ThenInclude(u=>u.User).Include(c=>c.circle).ThenInclude(ch=>ch.channels).ThenInclude(m=>m.messages).ThenInclude(l=>l.likes).ToList();
                    return Json(userCirc);
                }
                else{
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    return Json(error);
                }
            }
            else{
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
        }

        [HttpGet("/removeUser/{id}/{circleId}")]
        public IActionResult removeUser(int id, int circleId){
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if(dbContext.userCircles.Any(c=>c.circleId == circleId && c.userId==id)){
                if(dbContext.circles.Any(c=>c.circleId == circleId && c.userId==userId)){
                    usercircles userCirc = dbContext.userCircles.Where(c=>c.circleId == circleId && c.userId==id).Include(u=>u.User).FirstOrDefault();
                    dbContext.userCircles.Remove(userCirc);
                    dbContext.SaveChanges();
                    Dictionary<string, string> success = new Dictionary<string, string>();
                    success.Add("Message", "Success");
                    return Json(success);
                }
                else{
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    return Json(error);
                }
            }
            else{
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
        }
        [HttpGet("/getUsersInCircle/{id}")]//USE THIS TO RETRIEVE ALL USERS IN A SPECIFIC CIRCLE
        public IActionResult getUsersInCircle(int id)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
            if(dbContext.userCircles.Where(c=>c.circleId == id) == null){
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
            else{
                List<usercircles> userCirc = dbContext.userCircles.Where(c=>c.circleId == id).ToList();
                return Json(userCirc);
            }
        }

        [HttpPost("/createCircle")]
        public IActionResult CreateCircle([FromBody] circles newCircle)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("hacking", "hacker");
                    return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if (ModelState.IsValid)
            {
                if (dbContext.circles.Any(u => u.title == newCircle.title))
                {
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("title", "Title is already in use by other users");
                    return Json(error);
                }
                newCircle.userId = userId;
                dbContext.circles.Add(newCircle);
                dbContext.SaveChanges();
                //=========================================
                users addUser = dbContext.users.FirstOrDefault(u=>u.userId==userId);
                circles circ = dbContext.circles.FirstOrDefault(x=>x.title == newCircle.title);
                usercircles newUCircle = new usercircles{
                    userId = userId,
                    User = addUser,
                    circleId = circ.circleId,
                    circle = circ
                };
                dbContext.userCircles.Add(newUCircle);
                dbContext.SaveChanges();
                 //=========================================
                Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
            }
            else
            {
                return Json(ModelState);
            }
        }
        //==========================================================================================================
        //CHANNEL FUNCTIONS
        //==========================================================================================================
        [HttpPost("/createChannel/{id}")]
        public IActionResult createChannel(int id, [FromBody] channels newChannel)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("hacking", "hacker");
                    return Json(error);
            }
            if (ModelState.IsValid)
            {
                circles circleAdd = dbContext.circles.Where(c=>c.circleId == id).FirstOrDefault();
                newChannel.circle = circleAdd;
                newChannel.circleId = id;
                dbContext.channels.Add(newChannel);
                dbContext.SaveChanges();
                //=========================================
                
                 //=========================================
                Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
            }
            else
            {
                return Json(ModelState);
            }
        }
        [HttpGet("/removeChannel/{id}/{circleId}")]
        public IActionResult removeChannel(int id, int circleId){
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if(dbContext.channels.Any(c=>c.channelId == id)){
                if(dbContext.circles.Any(c=>c.circleId == circleId && c.userId==userId)){
                    channels userChan = dbContext.channels.Where(c=>c.channelId == id).Include(u=>u.messages).ThenInclude(m=>m.likes).FirstOrDefault();
                    dbContext.channels.Remove(userChan);
                    dbContext.SaveChanges();
                    Dictionary<string, string> success = new Dictionary<string, string>();
                    success.Add("Message", "Success");
                    return Json(success);
                }
                else{
                    Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    return Json(error);
                }
            }
            else{
                Dictionary<string, string> error = new Dictionary<string, string>();
                error.Add("Message", "Error");
                return Json(error);
            }
        }
        //==========================================================================================================
        //MESSAGE FUNCTIONS
        //==========================================================================================================
        [HttpPost("/leaveMessage/{id}")]
        public IActionResult leaveMessage(int id, [FromBody] messages newMessage)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    return Json(error);
            }
            int userId = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if (ModelState.IsValid)
            {
                users addUser = dbContext.users.FirstOrDefault(u=>u.userId==userId);
                channels channelAdd = dbContext.channels.Where(c=>c.channelId == id).FirstOrDefault();
                newMessage.channelId = id;
                newMessage.Channel = channelAdd;
                newMessage.userId = userId;
                newMessage.User = addUser;
                dbContext.messages.Add(newMessage);
                dbContext.SaveChanges();

                Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
            }
            else
            {
                return Json(ModelState);
            }
        }
        [HttpGet("/like/{messageId}")]
        public IActionResult LikeMessage(int messageId){
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("hacking", "hacker");
                    return Json(error);
            }
            int User = HttpContext.Session.GetInt32("UserId") ?? default(int);
            if (dbContext.messagelikes.Any(guest => guest.userId == User && guest.messageId == messageId))
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                    error.Add("Message", "Error");
                    error.Add("error", "Already Liked this message");
                    return Json(error);

            }
            users usertoAdd = dbContext.users.FirstOrDefault(x => x.userId == User);
            messages messagetoAdd = dbContext.messages.FirstOrDefault(x => x.messageId == messageId);
            messagelikes newlike = new messagelikes
            {
                userId = User,
                User = usertoAdd,
                messageId = messageId,
                message = messagetoAdd
            };
            dbContext.messagelikes.Add(newlike);
            dbContext.SaveChanges();
            Dictionary<string, string> success = new Dictionary<string, string>();
                success.Add("Message", "Success");
                return Json(success);
        }
        //==========================================================================================================

    }

}
