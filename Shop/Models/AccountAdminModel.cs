using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class AccountAdminModel : MenuModel
    {
        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        [DisplayName("Название роли")]
        public string role { get; set; }
        public List<UserProfileModel> users { get; set; }
        public List<string> roles { get; set; }

        public AccountAdminModel():base()
        {
            users=new List<UserProfileModel>();
            roles=new List<string>();
        }
    }

    public class UserProfileModel : UserProfile
    {

        public List<string> roles { get; set; }
        public UserProfileModel():base()
        {
            roles = new List<string>();
        }
    }
}