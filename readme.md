Overview
========

TropoCSharp is a set of C# classes for working with the [Tropo cloud communication service](http://tropo.com/). Tropo allows a developer to create applications that run over the phone, IM, SMS, and Twitter using web technologies. This library communicates with Tropo over JSON.

Usage
=====

You can download this project directly and build the source or install fron NuGet.

PM > Install-Package tropo-webapi

You can test samples in the TropoSamples solution by viewing them in a web browser, or via HTTP POST (which is how the Tropo WebAPI interacts with web applications).

Tests for various Tropo action classes are in the TropoClassesTests solution.

Project with samples (ASP.NET web application) will use a directory called TropoSample (e.g., http://localhost/TropoSample).

TropoCSharp Example
======

<pre>
using System;
using TropoCSharp.Tropo;

namespace TropoSamples
{
	public partial class HelloWorld : System.Web.UI.Page
	{
		
		public void Page_Load (object sender, EventArgs args)
		{
            // Create a new instance of the Tropo object.
            Tropo tropo = new Tropo();

            // Call the say method of the Tropo object and give it a prompt to say.
            tropo.Say("Hello World!");

            // Render the JSON for Tropo to consume.
            Response.Write(tropo.RenderJSON());
		}
	}
}
</pre>

TropoCSharp.Mvc Example
======

<pre>
using System;
using TropoCSharp.Tropo;

namespace TropoSamples
{
	public class HelloWorldController: TropoController {
    
    public HelloWorldController() {
      ApiToken = "XXXXXXXXXXXXXXXXXXXXXXXXXXX";
    }
    
    public ActionResult Index(Session session) {
      Script.Say("Hello, World");
      Script.On(Event.Continue, To("Next"), new Say("thanks"));
      return Write();
    }
    
    public ActionResult Next(Result result) {
      Script.Say("Cool");
      Script.HangUp();
      return Write();
    }
	
	}
}
</pre>
