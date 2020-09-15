using muscshop.Context;
using muscshop.filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace muscshop.Controllers
{
    [Authorize(Roles = "Admin")]
    [SessionFilter]
    public class UserController : Controller
    {
        StoreContext _storeContext = new StoreContext();
        public ActionResult Index()
        {
            var users = _storeContext.Users.Include("Roles");
            return View(users);
        }
        [HttpPost]
        public ActionResult ChangeRole(int? id)
        {
            var user = _storeContext.Users.Include("Roles").Where(x => x.UserId == id).FirstOrDefault();
            var currentrole = string.Empty;
            var roleslist = user.Roles.Select(x => x.RoleName).ToList();
            if (roleslist.Contains("Manager"))
            {
                user.Roles = _storeContext.Roles.Where(x => x.RoleName == "Customer").ToList();
                currentrole = "Become Manager";  
            }
            else
            {
                user.Roles = _storeContext.Roles.Where(x => x.RoleName == "Manager").ToList();
                currentrole = "Deomte from Manager";   
            }
            _storeContext.SaveChanges();
            return Json(currentrole, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string par)
        {
            var user = _storeContext.Users.Where(x => x.Username.ToLower().Contains(par.ToLower()));
            return View("index", user);
        }
    }
}