using Aque.WebGet.Core.Services;
using Aque.WebGet.Infra.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Aque.WebGet.Infra.Tests.Services
{
	[TestFixture]
	public class WebCrawlerService_Tests
	{
		private IWebCrawlerService _service;

		[SetUp]
		public void Given_A_WebCrawlerService()
		{
			_service = new WebCrawlerService();
			_service.CrawlCompleted += _service_CrawlCompleted;
		}

		[TestCase("http://stackoverflow.com/questions/2656178/parsing-html-page-with-htmlagilitypack-to-select-divs-by-class", "question")]
		[TestCase("http://www.mcfc.co.uk/", "mcfc-advert")]
		public void It_Should_Be_Able_To_Get_The_Content_Of_Divs(string url, string cssClass)
		{
			var xpaths = new List<string> { string.Format("//div[@class='{0}']", cssClass) };

			_service.Crawl(url, xpaths);
		}

		private void _service_CrawlCompleted(object sender, System.EventArgs e)
		{
			Assert.IsTrue(_service.NodeList.Any());
		}
	}
}
