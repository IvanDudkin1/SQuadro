using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;
using System.Reflection;

namespace SQuadro.ModelBinders
{
    public class ValueTextModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };

            object actualValue = null;
            int parseResult;

            string attemptedValue = null;
            if (valueResult.AttemptedValue.ToLower() != "null" && !String.IsNullOrWhiteSpace(valueResult.AttemptedValue))
                attemptedValue = valueResult.AttemptedValue;

            if (attemptedValue != null)
            {
                if (Int32.TryParse(valueResult.AttemptedValue, out parseResult))
                {
                    MethodInfo castMethod = this.GetType().GetMethod("Cast").MakeGenericMethod(bindingContext.ModelType);
                    actualValue = castMethod.Invoke(null, new object[] { parseResult });
                }
                else
                    modelState.Errors.Add("Input string was in incorrect format");
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }

        public static T Cast<T>(object obj)
        {
            return (T)(dynamic)obj; 
        }
    }
}