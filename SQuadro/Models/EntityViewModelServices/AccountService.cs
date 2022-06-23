using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace SQuadro.Models
{
    public class AccountService
    {
        private static void UpdateUserFromModel(User user, AccountModel model, User currentUser, EntityContext context)
        {
            if (context.Users.Any(u => u.Email == model.Email && u.ID != model.ID))
                throw new UserException("User with e-mail {0} already exists in the system.".ToFormat(model.Email));

            user.Name = model.Name;
            user.Email = model.Email;
            
            if (user.Role != SystemRole.Admin.Value && model.Organization != user.Organization.Name)
                throw new HttpException(403, "Access denied");
            if (context.Organizations.Any(o => o.Name == model.Organization && o.ID != user.OrganizationID))
                throw new UserException("Organization {0} already exists in the system.".ToFormat(model.Organization));

            user.Organization.Name = model.Organization;
            user.Organization.Timezone = model.Timezone;
        }

        public static AccountModel GetViewModel(Guid userID, EntityContext context)
        {
            AccountModel model = new AccountModel();
            var user = UsersService.GetUser(userID, context);

            model.ID = user.ID;
            model.Name = user.Name;
            model.Email = user.Email;
            model.Organization = user.Organization.Name;
            model.Timezone = user.Organization.Timezone;

            return model;
        }

        public static User SetUser(AccountModel model, User currentUser, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("AccountModel");

            User user = UsersService.GetUser(model.ID, context);

            UpdateUserFromModel(user, model, currentUser, context);
            return user;
        }
    }
}