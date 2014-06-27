using Aque.WebGet.Core.Services;
using NUnit.Framework;
using System.Linq;
using System.Xml.Linq;

namespace Aque.WebGet.Core.Tests.Services
{
	[TestFixture]
	public class UrlsService_Tests
	{
		private IUrlsService _service;

		[SetUp]
		public void Given_A_UrlsService()
		{
			//var map = new XElement("urlset", new XElement("url", new XElement("loc", "http://www.mcfc.co.uk/")));
			var map = XElement.Load(@"C:\Users\mattia\Desktop\sitemap-small.xml");
			XNamespace xNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
			
			_service = new UrlsService(map, xNamespace);
			_service.LoadUrlListCompleted += _service_LoadUrlListCompleted;
		}

		[Test]
		public void It_Should_Be_Able_To_Manage_A_SiteMap()
		{
			_service.LoadUrlList();
		}

		private void _service_LoadUrlListCompleted(object sender, System.EventArgs e)
		{
			Assert.IsTrue(_service.UrlList.Any());

		}
	}
}
