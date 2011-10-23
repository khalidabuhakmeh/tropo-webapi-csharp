using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using TropoCSharp.Structs;
using TropoCSharp.Tropo;

namespace TropoCSharp.Mvc
{
    public abstract class TropoController : Controller
    {
        private const string TropoUrl =
            "https://api.tropo.com/1.0/sessions/{1}/signals?action=signal&value={2}&token=#{0}";

        /// <summary>
        /// Remember to intialize the ApiToken property
        /// </summary>
        protected TropoController()
        {
            Script = new Tropo.Tropo();
        }

        protected Tropo.Tropo Script { get; private set; }

        /// <summary>
        /// Either Tropo Voice Token, can be found in the Applications page, under the specific application
        /// </summary>
        protected string ApiToken { get; set; }

        /// <summary>
        /// Will write out the current script up to this point and return a ContentResult with the corresponding JSON.
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Write()
        {
            return Content(Script.RenderJSON(), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// Will generate a absolute url to a controller action in your current application, helpful with call flows.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="values"></param>
        /// <param name="protocol"></param>
        /// <returns></returns>
        protected string To(string action, string controller = null, object values = null, string protocol = null)
        {
            string controllerName = controller ?? GetControllerName();
            protocol = protocol ?? (Request.IsSecureConnection ? "https" : "http");
            return Url.Action(action, controllerName, values, protocol);
        }

        /// <summary>
        /// Will perform a web request to signal an existing session, please remember to specify the event if necessary.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        protected string Signal(string sessionId, string eventName = Event.Continue)
        {
            ValidateApiToken();

            string url = string.Format(TropoUrl, ApiToken, sessionId, eventName);
            WebRequest httpRequest = WebRequest.Create(url);
            httpRequest.Method = "GET";

            string result;
            try
            {
                WebResponse response = httpRequest.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    var doc = new XmlDocument();
                    doc.Load(stream);
                    result = doc.SelectSingleNode("status").Value;
                }
            }
            catch (Exception)
            {
                return "Error";
            }

            return result;
        }

        private void ValidateApiToken()
        {
            if (string.IsNullOrWhiteSpace(ApiToken))
                throw new ArgumentException("please remember to set your api token.");
        }

        /// <summary>
        /// Will provide you with a Url that will be used to signal another session
        /// </summary>
        /// <example>
        /// If you current session hangs up, then tropo could use this url to tell another session to continue to the next step.
        /// </example>
        /// <param name="sessionId"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        protected string SignalTo(string sessionId, string eventName = Event.Continue)
        {
            return string.Format(TropoUrl, ApiToken, sessionId, eventName);
        }

        /// <summary>
        /// Will create a new session, pass in parameters to use on the next session
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected NewSession CreateNewSession(IDictionary<string, string> parameters = null)
        {
            ValidateApiToken();

            var par = parameters ?? new Dictionary<string, string>();
            return new NewSession(Script.CreateSession(ApiToken, par));
        }

        private string GetControllerName()
        {
            string controllerName = GetType().Name;
            return controllerName.Substring(0, controllerName.Length - "Controller".Length).ToLowerInvariant();
        }
    }
}