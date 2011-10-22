using System;
using System.Web.Mvc;
using TropoCSharp.Tropo;

namespace TropoCSharp.Mvc.ModelBinders
{
    public class SessionModelBinder : TropoModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.RequestContext.HttpContext.Request;
                var json = InputStreamToString(request);
                var session = new Session(json);
                return session;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
