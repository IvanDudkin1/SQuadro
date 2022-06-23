using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.ModelBinders;

namespace SQuadro.Filters
{
    public class BindToValueTextAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new ValueTextModelBinder();
        }
    }
}