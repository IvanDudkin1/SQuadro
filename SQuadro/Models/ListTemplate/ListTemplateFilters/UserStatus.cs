using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public sealed class UserStatus
    {

        private readonly string name;
        private readonly int value;

        private static readonly Dictionary<string, UserStatus> instance = new Dictionary<string, UserStatus>();

        public static readonly UserStatus All = new UserStatus(1, "All Users");
        public static readonly UserStatus ActiveOnly = new UserStatus(2, "Active Only");
        public static readonly UserStatus InactiveOnly = new UserStatus(3, "Inactive Only");

        private UserStatus(int value, String name)
        {
            this.name = name;
            this.value = value;
            instance[name] = this;
        }

        public override String ToString()
        {
            return name;
        }

        public static explicit operator UserStatus(string value)
        {
            UserStatus result;
            if (instance.TryGetValue(value, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        public static string[] GetUserStatusesList()
        {
            return new string[] { UserStatus.All.ToString(), UserStatus.ActiveOnly.ToString(), UserStatus.InactiveOnly.ToString() };
        }
    }
}