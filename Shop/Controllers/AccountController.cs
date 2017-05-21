using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using Shop.DataService;
using Shop.Filters;
using Shop.Models.Builders;
using Shop.Modules;
using WebMatrix.WebData;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private MenuBuilder menuBuilder { set; get; }
        private IAccountAdminModelBuilder accountAdminModelBuilder;
        private IRegisterBuilder registerBuilder;

        private string Key { set; get; }

        public AccountController(IAccountAdminModelBuilder AccountAdminModelBuilder, IRegisterBuilder RegisterBuilder, IDataService dataService, IImagesPath imagesPath)
        {
            accountAdminModelBuilder = AccountAdminModelBuilder;
            registerBuilder = RegisterBuilder;
            menuBuilder = new MenuBuilder(dataService, imagesPath);
            Key = WebConfigurationManager.AppSettings["LoginKey"];
        }

        [AllowAnonymous]
        public ActionResult Login(string key, string returnUrl)
        {
            if (string.IsNullOrEmpty(key)||key!=Key)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginModel();
            model.topMenuItems = menuBuilder.BuildTopMenu();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }
            model.topMenuItems = menuBuilder.BuildTopMenu();
            ModelState.AddModelError("", "Имя пользователя или пароль некорректны");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");//сделать правильный редирект
        }

  

        [Authorize(Roles = "Admin")]
        public ActionResult Register()//переход на страницу создания пользователя
        {
            var model = new RegisterModel();
            model = registerBuilder.Build();
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Register(RegisterModel model)//создние пользователя
        {
            var result = "Пользователь не создан";
            if (ModelState.IsValid)
            {
                try
                {
                    if (!WebSecurity.UserExists(model.UserName))
                    {
                        //WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { email = model.email, DAXSallerId = model.DAXSallerId, phone = model.phone });
                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Discount = model.Discount });
                        result = "Пользователь создан";
                    }
                    else
                    {
                        result = "Такой пользователь существует";
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(e.Message, ErrorCodeToString(e.StatusCode));
                    return Content(e.Message, "text/html");
                }
            }
            return Content(result, "text/html");
        }



        [Authorize(Roles = "Admin")]
        public ActionResult RolesManager()//Менеджер ролей
        {
            var model = accountAdminModelBuilder.Build();
            return View("AdminRolesManager",model);
        }


        [Authorize(Roles = "admin")]
        public ActionResult RemoveUserFromRole(string user, string role)
        {
            try
            {
                if (WebSecurity.UserExists(user)&&  Roles.RoleExists(role))
                {
                    Roles.RemoveUserFromRole(user, role);
                }
            }
            catch (Exception)
            {

             
            }
            return RedirectToAction("RolesManager");
        }

        [Authorize(Roles = "admin")]
        public ActionResult AddUserToRole(string user, string role)
        {
            try
            {
                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(role))
                {
                    if (WebSecurity.UserExists(user) && Roles.RoleExists(role) && !Roles.IsUserInRole(user, role))
                    {
                        Roles.AddUserToRole(user, role);
                    } 
                }
     
            }
            catch (Exception)
            {


            }
            return RedirectToAction("RolesManager");
        }

        [Authorize(Roles = "admin")]
        public ActionResult RemoveRole(string role)
        {
            try
            {
                if (!string.IsNullOrEmpty(role))
                {
                    if (Roles.GetUsersInRole(role).Length == 0 && !role.IsEmpty())
                    {
                        Roles.DeleteRole(role, true); // true means throw if any users are in this role
                    }
                    else
                    {
                        ViewData["ADMText"] = "Роль не удалена. Возможно у этой роли еще есть пользователи!";
                    }
                }
            }
            catch (Exception err)
            {


            }
            return RedirectToAction("RolesManager");
        }



        [Authorize(Roles = "admin")]
        public ActionResult AddRole(string role)
        {

            if (!string.IsNullOrEmpty(role)&&!Roles.RoleExists(role))
            {
                Roles.CreateRole(role);
            }
            else
            {
                ViewData["ADMText"] = "Роль не добавлена. Такая роль либо существует, либо пришла пустая строка!";
            }

            return RedirectToAction("RolesManager");
        }


        //[Authorize(Roles = "Admin")]
        //public ActionResult ListUsers(string sorting)
        //{
        //    var model = new UserProfilesModel();
        //    if (string.IsNullOrEmpty(sorting))
        //    {
        //        model.UserProfiles = AccountModelsBuilder.ListUserProfileModel();
        //    }
        //    else
        //    {
        //        if (sorting == "login")
        //        {
        //            model.UserProfiles = AccountModelsBuilder.ListUserProfileModel().OrderBy(it => it.UserName).ToList();
        //        }
        //        if (sorting == "email")
        //        {
        //            model.UserProfiles = AccountModelsBuilder.ListUserProfileModel().OrderBy(it => it.email).ToList();
        //        }
        //        if (sorting == "DAXSallerId")
        //        {
        //            model.UserProfiles = AccountModelsBuilder.ListUserProfileModel().OrderBy(it => it.DAXSallerId).ToList();
        //        }

        //    }
        //    return View("ListUsers", model);
        //}

        //[Authorize(Roles = "Admin")]
        //public ActionResult GetUserProfile(long UserId)
        //{
        //    var user = AccountModelsBuilder.GetUserProfileModel(UserId);
        //    if (user == null)
        //    { return PartialView("EditUserProfilePartial", user = new UserProfileModel()); }

        //    return PartialView("EditUserProfilePartial", user);
        //}

        //[HttpPost]
        //[Authorize(Roles = "admin")]
        //[ValidateAntiForgeryToken]
        //public ActionResult SaveUserPrifile(UserProfileModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            AccountModelsBuilder.UpdateUserProfile(model);

        //        }
        //        catch (MembershipCreateUserException e)
        //        {

        //        }
        //    }
        //    var modelresult = new UserProfilesModel();
        //    modelresult.UserProfiles = AccountModelsBuilder.ListUserProfileModel();
        //    return View("ListUsers", modelresult);
        //}




       [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ChangePassword(string user, string password)
        {
         
                try
                {
                    if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
                    {
                        WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(user, 500), password);
                    }

                }
                catch (Exception e)
                {
                   
                }

                return RedirectToAction("RolesManager");
        }

        [Authorize(Roles = "admin")]
        public ActionResult RemoveUser(string user)
        {

            try
            {
                var roles = Roles.GetRolesForUser(user);
                if (roles != null & roles.Any())
                {
                    Roles.RemoveUserFromRoles(user, roles);
                }
                var u = Membership.GetUser(WebSecurity.CurrentUserName);
             
               
                if (Membership.DeleteUser(user, true))
                {

                }

            }
            catch (Exception)
            {


            }

            return RedirectToAction("ListUsers");
        }

     
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

      

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
      
    }
}
