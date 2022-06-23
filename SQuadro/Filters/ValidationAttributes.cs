using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SQuadro.Models
{
    public class EmailValidationAttribute : RegularExpressionAttribute
    {
        public EmailValidationAttribute()
            : base(ValidationExpression)
        {
            this.ErrorMessage = "Email Address is not in a correct format";
        }

        public static string ValidationExpression { get { return @"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$"; } }
    }

    public class BooleanRequiredAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }
    }
}