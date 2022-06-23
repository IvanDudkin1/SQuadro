using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQuadro.Models;

namespace SQuadro
{
    public partial class Company
    {
        public string Phone
        {
            get
            {
                return String.Join(", ", this.Contacts.Where(c => c.ContactTypeID == 1).OrderByDescending(c => c.IsPrimary));
            }
        }

        public string Fax
        {
            get
            {
                return String.Join(", ", this.Contacts.Where(c => c.ContactTypeID == 2).OrderByDescending(c => c.IsPrimary));
            }
        }

        public string Email
        {
            get
            {
                return String.Join(", ", this.Contacts.Where(c => c.ContactTypeID == 3).OrderByDescending(c => c.IsPrimary));
            }
        }
    }
}