using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQuadro.Filters;

namespace SQuadro.Models
{
    [BindToValueText]
    public class SystemContactType
    {
        private SystemContactType()
        { }

        private SystemContactType(int value, string text)
        {
            Value = value;
            Text = text;
            instance[value] = this;
        }

        private static readonly Dictionary<int, SystemContactType> instance = new Dictionary<int, SystemContactType>();

        public int Value { get; private set; }
        public string Text { get; private set; }

        public static explicit operator SystemContactType(int value)
        {
            SystemContactType result;
            if (instance.TryGetValue(value, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        public override String ToString()
        {
            return Value.ToString();
        }

        public static SystemContactType Email = new SystemContactType(0, "Email");

        public static SystemContactType[] SystemContactTypes()
        {
            return new SystemContactType[] { Email };
        }
    }
}