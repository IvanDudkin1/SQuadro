using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace SQuadro.Models
{
    public class UsersService
    {
        private static void UpdateUserFromModel(User user, UserModel model, User currentUser, EntityContext context)
        {
            if (context.Users.Any(u => u.Email == model.Email && u.ID != model.ID))
                throw new UserException("User with e-mail {0} already exists in the system.".ToFormat(model.Email));

            user.Name = model.Name;
            user.Email = model.Email;
            user.OrganizationID = currentUser.OrganizationID;
            user.Role = model.SystemRole;
            user.UserRoleID = model.UserRoleID;
        }

        public static UserModel GetViewModel(Guid? userID, EntityContext context)
        {
            UserModel userModel = new UserModel();
            if (userID != null && userID != Guid.Empty)
            {
                var user = GetUser(userID.Value, context);

                userModel.ID = user.ID;
                userModel.Name = user.Name;
                userModel.Email = user.Email;
                userModel.SystemRole = user.Role;
                userModel.UserRoleID = user.UserRoleID;
            }
            return userModel;
        }

        public static User SetUser(UserModel model, User currentUser, EntityContext context)
        {
            if (model == null)
                throw new ArgumentNullException("userModel");

            User user = null;

            if (model.ID != Guid.Empty)
            {
                user = GetUser(model.ID, context);
            }
            else
            {
                user = new User();
                context.Users.AddObject(user);
            }

            UpdateUserFromModel(user, model, currentUser, context);
            return user;
        }

        public static User AddNew(string email, string organizationName, string name, EntityContext context)
        {
            Organization organization = new Organization()
            {
                Name = organizationName,
                Timezone = TimeZoneInfo.Utc.Id
            };
            context.Organizations.AddObject(organization);

            User user = new User()
            {
                Email = email,
                Name = name,
                Role = SystemRole.Admin.Value,
                Organization = organization
            };
            context.Users.AddObject(user);

            ContactType contactType = new ContactType()
            {
                Name = "Email",
                DisplayPattern = "<a href='mailto:{0}'>{0}</a>",
                SystemType = SystemContactType.Email.Value,
                Organization = organization
            };
            context.ContactTypes.AddObject(contactType);

            contactType = new ContactType()
            {
                Name = "Phone",
                DisplayPattern = "<a href='tel:{0}'>{0}</a>",
                Organization = organization
            };
            context.ContactTypes.AddObject(contactType);

            contactType = new ContactType()
            {
                Name = "Fax",
                DisplayPattern = "<a href='fax:{0}'>{0}</a>",
                Organization = organization
            };
            context.ContactTypes.AddObject(contactType);

            contactType = new ContactType()
            {
                Name = "Mobile",
                DisplayPattern = "<a href='tel:{0}'>{0}</a>",
                Organization = organization
            };
            context.ContactTypes.AddObject(contactType);

            contactType = new ContactType()
            {
                Name = "Skype",
                DisplayPattern = "<a href='skype:{0}'>{0}</a>",
                Organization = organization
            };
            context.ContactTypes.AddObject(contactType);

            return user;
        }

        public static User DeleteUser(Guid userID, EntityContext context)
        {
            User user = context.Users.SingleOrDefault(u => u.ID == userID);
            if (user != null)
            {
                // TODO don't delete last superadmin
                context.Users.DeleteObject(user);
            }
            return user;
        }

        public static User GetUser(Guid id, EntityContext context)
        {
            var user = context.Users.SingleOrDefault(u => u.ID == id);
            if (user == null)
                throw new InvalidOperationException("User with ID = {0} does not exist anymore".ToFormat(id));
            return user;
        }
    }
}