using System;
using System.Web.Mvc;

namespace SchematBlokowy.Utils
{
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            if (!valueResult.AttemptedValue.IsNullOrEmpty())
            {
                try
                {
                    var value = valueResult.AttemptedValue.Replace('.', ',');
                    actualValue = Convert.ToDouble(value);
                }
                catch (FormatException e)
                {
                    modelState.Errors.Add(e);
                }
            }
            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}