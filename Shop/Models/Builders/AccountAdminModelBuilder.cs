using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Shop.DataService;
using Shop.Modules;
using WebMatrix.Data;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{
    public interface IAccountAdminModelBuilder
    {
        AccountAdminModel Build();
        UserProfileModel BuildOneUser(string login);
    }

    public class AccountAdminModelBuilder : MenuBuilder,IAccountAdminModelBuilder
    {

        private Database db;
        private string sqlGetAllusers;
        public AccountAdminModelBuilder(IDataService dataService, IImagesPath imagesPath)
            : base(dataService, imagesPath)
        {
            db = Database.Open("ConnectDB");
            sqlGetAllusers = "SELECT UserId, UserName, Discount FROM UserProfile";
        }
        public AccountAdminModel Build()
        {
            
            var model = new AccountAdminModel();
            model.roles = Roles.GetAllRoles().ToList();
            model.users = GetAllUsers();
            if (model.users!=null&&model.users.Any())
            {
               model.users= model.users.Select(
                    us =>
                        new UserProfileModel()
                        {
                            UserId = us.UserId,
                            UserName = us.UserName,
                            Discount = (us.Discount != null ? us.Discount : 0),
                            roles = Roles.GetRolesForUser(us.UserName).ToList()
                        }).ToList();
            }
            model.menu = BuildMenu();
            model.topMenuItems = BuildTopMenu();
            return model;
        }

        public UserProfileModel BuildOneUser(string login)
        {
            var userProfile = new UserProfileModel();

            if (string.IsNullOrEmpty(login))
            {
                if (WebSecurity.IsAuthenticated)
                {
                    userProfile = GetOneUser(WebSecurity.CurrentUserName);
                }
            }
            else
            {
                    userProfile = GetOneUser(login);
            }

            return userProfile;
        }

        private List<UserProfileModel> GetAllUsers()
        {
            return
                db.Query(sqlGetAllusers)
                    .Select(us => new UserProfileModel() { UserId = us.UserId, UserName = us.UserName, Discount = (us.Discount??0) }).ToList();
        }

        private UserProfileModel GetOneUser(string login)
        {
            return db.Query(sqlGetAllusers + " where UserName='" + login + "'").Select(us => new UserProfileModel() { UserId = us.UserId, UserName = us.UserName, Discount = (us.Discount??0)}).First();        
        }

    }
}