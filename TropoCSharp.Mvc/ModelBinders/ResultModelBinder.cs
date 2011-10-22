using System;
using System.Web.Mvc;
using TropoCSharp.Tropo;

namespace TropoCSharp.Mvc.ModelBinders
{
    public class ResultModelBinder : TropoModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.RequestContext.HttpContext.Request;
                var json = InputStreamToString(request);
                var result = new Result(json);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}