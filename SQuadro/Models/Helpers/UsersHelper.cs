using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public class UsersHelper : IUsersHelper
    {
        public UsersHelper()
        {
            httpContext = HttpContext.Current;

            if (httpContext == null)
                throw new InvalidOperationException("Method called from non Asp.Net environment!");
        }

        private HttpContext httpContext;

        public User CurrentUser
        {
            get
            {
                if (!httpContext.Request.IsAuthenticated)
                    throw new HttpException(401, "Unauthorized access!");

                Guid personID;
                if (!Guid.TryParse(HttpContext.Current.User.Identity.Name, out personID))
                    throw new HttpException(403, "Access denied!");

                User user = EntityContext.Current.Users.FirstOrDefault(u => u.ID == personID);
                if (user == null)
                    throw new HttpException(403, "Access denied!");

                return user;
            }
        }
    }

    public interface IUsersHelper
    {
        User CurrentUser { get; }
    }
}