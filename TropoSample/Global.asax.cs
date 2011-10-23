using System;
using System.Web.Mvc;
using TropoCSharp.Mvc.ModelBinders;
using TropoCSharp.Tropo;

namespace TropoSample
{
	public class Global : System.Web.HttpApplication
	{
		protected virtual void Application_Start (Object sender, EventArgs e)
		{
            ModelBinders.Binders.Add(typeof(Session), new SessionModelBinder());
            ModelBinders.Binders.Add(typeof(Result), new ResultModelBinder());
		}
	}
}
