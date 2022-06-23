using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace SQuadro
{
    public partial class EntityContext
    {
        public EntityContext(HttpContext httpContext)
            : this()
        {
            httpContext.Items[ContextID] = this; 
        }

        private static Cache cache = System.Web.HttpContext.Current.Cache;

        public const string ContextID = "_EntityContext";

        public static EntityContext Current
        {
            get
            {
                return (EntityContext)HttpContext.Current.Items[ContextID];
            }
        }
    }
}