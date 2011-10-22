using System.IO;
using System.Xml;

namespace TropoCSharp.Tropo
{
    public class NewSession
    {
        public NewSession(Stream stream)
        {
            var doc = new XmlDocument();
            doc.Load(stream);
            Id = doc.SelectSingleNode("session/id").InnerText;
            Token = doc.SelectSingleNode("session/token").InnerText;
            bool success;
            bool.TryParse(doc.SelectSingleNode("session/success").InnerText, out success);
            Success = success;
        }

        public string Id { get; protected set; }
        public bool Success { get; protected set; }
        public string Token { get; protected set; }
    }
}
