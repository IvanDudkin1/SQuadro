using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQuadro.ModelBinders
{
    public class GuidModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };

            object actualValue = null;
            Guid parseResult;

            string attemptedValue = null;
            if (valueResult.AttemptedValue.ToLower() != "null" && !String.IsNullOrWhiteSpace(valueResult.AttemptedValue))
                attemptedValue = valueResult.AttemptedValue;

            if (attemptedValue != null)
            {
                if (Guid.TryParse(valueResult.AttemptedValue, out parseResult))
                    actualValue = parseResult;
                else
                    modelState.Errors.Add("Input string was in incorrect format");
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}