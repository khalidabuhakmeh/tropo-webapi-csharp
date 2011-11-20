using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using TropoCSharp.Mvc;
using TropoCSharp.Mvc.ModelBinders;
using TropoCSharp.Structs;
using TropoCSharp.Tropo;

namespace TropoClassesTests
{
    [TestFixture]
    public class TropoControllerTests
    {
        private const string testSessionId = "3FA7C70A1DD211B286B7A583D7B46DDD0xac106207";
        private const string testApiToken = "MockApiToken";

        [TestFixtureSetUp]
        public void Init()
        {
            WebRequest.RegisterPrefix("https://api.tropo.com/1.0/sessions/", new TestSignalWebRequest("QUEUED"));
        }

        [Test]
        public void Can_Create_Signal_Url()
        {
            var testController = new TestController();
            var url = testController.TestSignalTo(testSessionId);
            var expectedUrl = string.Format(TestController.TropoSignalTemplate, testApiToken, testSessionId, Event.Continue);

            Assert.NotNull(url);
            Assert.AreEqual(expectedUrl, url);
        }

        [Test]
        public void Can_Parse_Signal_Response()
        {
            var testController = new TestController();
            var result = testController.TestSignal(testSessionId);

            Assert.NotNull(result);
            Assert.AreEqual(result, "QUEUED");
        }

        internal class TestSignalWebRequest : IWebRequestCreate
        {
            private const string ResponseTemplate = "<signal><status>{0}</status></signal>";
            private readonly string signalResponse;

            public TestSignalWebRequest(string responseType = "QUEUED")
            {
                signalResponse = String.Format(ResponseTemplate, responseType);
            }

            public WebRequest Create(Uri uri)
            {
                var response = new Mock<WebResponse>();
                response.Setup(r => r.GetResponseStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes(signalResponse)));
                var request = new Mock<WebRequest>();
                request.Setup(r => r.GetResponse()).Returns(response.Object);
                return request.Object;
            }
        }

        internal class TestController: TropoController
        {
            // copied from TropoController source
            private const string TropoUrl =
                "https://api.tropo.com/1.0/sessions/{1}/signals?action=signal&value={2}&token=#{0}";

            public TestController()
            {
                ApiToken = testApiToken;
            }

            public static string TropoSignalTemplate
            {
                get { return TropoUrl; }
            }

            public string TestSignalTo(string sessionId, string eventName = Event.Continue)
            {
                return SignalTo(sessionId, eventName);
            }

            public string TestSignal(string sessionId, string eventName = Event.Continue)
            {
                return Signal(sessionId, eventName);
            }
        }
    }
}
