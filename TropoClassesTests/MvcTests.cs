using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using TropoCSharp.Mvc.ModelBinders;
using TropoCSharp.Tropo;

namespace TropoClassesTests
{
    [TestFixture]
    public class MvcTests
    {
        [Test]
        public void Can_Bind_A_Tropo_Session()
        {
            var binder = new SessionModelBinder();
            var session = @"{'session':{'id':'17bc3787a1623646bcd8440a7d1121d3','accountId':'99999','timestamp':'2011-10-22T21:49:40.794Z','userType':'NONE','initialText':null,'callId':null,'parameters':{'lead_message':'hello world','action':'create'}}}";
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            request.Setup(r => r.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(session)));
            context.Setup(x => x.Request).Returns(request.Object);

            var result = binder.BindModel(new ControllerContext(new RequestContext(context.Object, new RouteData()), new TestController()), null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Session>(result);
        }

        [Test]
        public void Can_Bind_A_Tropo_Result()
        {
            var tropoResult =
@"{'result':{
     'sessionId':'3FA7C70A1DD211B286B7A583D7B46DDD0xac106207',
     'state':'ANSWERED',
     'sessionDuration':1,
     'sequence':1,
     'complete':true,
     'error':null,
     'actions':{
        'name':'account_number',
        'attempts':1,
        'disposition':'SUCCESS',
        'confidence':100,
        'interpretation':'12345',
        'utterance':'1 2 3 4 5'
     }
  }
}";
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            request.Setup(r => r.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(tropoResult)));
            context.Setup(x => x.Request).Returns(request.Object);

            var binder = new ResultModelBinder();
            var result = binder.BindModel(new ControllerContext(new RequestContext(context.Object, new RouteData()), new TestController()), null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Result>(result);

        }

        internal class TestController: Controller {}
    }
}
