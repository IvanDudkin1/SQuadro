using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQuadro.Models
{
    public class SystemRole
    {
        private SystemRole()
        { }

        private SystemRole(int value, string text)
        {
            Value = value;
            Text = text;
            instance[value] = this;
        }

        private static readonly Dictionary<int, SystemRole> instance = new Dictionary<int, SystemRole>();

        public int Value { get; private set; }
        public string Text { get; private set; }

        public static explicit operator SystemRole(int value)
        {
            SystemRole result;
            if (instance.TryGetValue(value, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        public override String ToString()
        {
            return Text;
        }

        public static SystemRole Member = new SystemRole(0, "Member");
        public static SystemRole Admin = new SystemRole(1, "Admin");

        public static SystemRole[] SystemRoles()
        {
            return new SystemRole[] { Member, Admin };
        }
    }
}
