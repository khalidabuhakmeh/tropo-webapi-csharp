using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TropoCSharp.Mvc.ModelBinders
{
    public abstract class TropoModelBinder : DefaultModelBinder
    {
        public abstract override object BindModel(ControllerContext controllerContext,
                                                  ModelBindingContext bindingContext);

        protected string InputStreamToString(HttpRequestBase request)
        {
            var stream = request.InputStream;
            stream.Position = 0;
            var sb = new StringBuilder();
            var streamLength = Convert.ToInt32(stream.Length);
            var streamArray = new Byte[streamLength];

            stream.Read(streamArray, 0, streamLength);

            for (var i = 0; i < streamLength; i++)
            {
                sb.Append(Convert.ToChar(streamArray[i]));
            }

            return sb.ToString();
        }
    }
}