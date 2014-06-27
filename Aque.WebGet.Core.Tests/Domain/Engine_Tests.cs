using System;
using System.Collections.Generic;
using Aque.WebGet.Core.Domain;
using Aque.WebGet.Core.Events;
using Aque.WebGet.Core.Models;
using Aque.WebGet.Core.Services;
using NSubstitute;
using NUnit.Framework;

namespace Aque.WebGet.Core.Tests.Domain
{
	[TestFixture]
	public class Engine_Tests
	{
		private IEngine _engine;
		private ScanCompletedStatus _scanCompletedStatus;
		private ISelectorsService _selectorsService;
		private IUrlsService _urlsService;
		private IWebCrawlerService _webCrawlerService;

		const string url = "http://www.google.com";

		[SetUp]
		public void Given_An_Engine()
		{

			_selectorsService = Substitute.For<ISelectorsService>();
			_selectorsService.When(e => e.LoadSelectorList())
				.Do(e => _selectorsService.LoadSelectorListCompleted += Raise.EventWith(new object(), new EventArgs()));

			_urlsService = Substitute.For<IUrlsService>();
			_urlsService.When(e => e.LoadUrlList())
				.Do(e => _urlsService.LoadUrlListCompleted += Raise.EventWith(new object(), new EventArgs()));

			_webCrawlerService = Substitute.For<IWebCrawlerService>();
			_webCrawlerService.When(e => e.Crawl())
				.Do(e => _webCrawlerService.CrawlCompleted += Raise.EventWith(new object(), new EventArgs()));

			_engine = new Engine(_selectorsService, _urlsService, _webCrawlerService);
			_engine.ScanCompleted += (s, e) => _engine_ScanCompleted(s, (ScanCompletedEventArgs)e);
		}

		[Test]
		public void It_Should_Be_Able_To_Raise_SelectorListEmpty()
		{
			// arrange
			_scanCompletedStatus = ScanCompletedStatus.SelectorListEmpty;
			// act
			_engine.Scan();
		}

		[Test]
		public void It_Should_Be_Able_To_Raise_UrlListEmpty()
		{
			// arrange
			_selectorsService.SelectorList.Returns(new List<string> { "mcfc-advert" });
			_scanCompletedStatus = ScanCompletedStatus.UrlListEmpty;
			// act
			_engine.Scan();
		}

		[Test]
		public void It_Should_Be_Able_To_Raise_Successfully()
		{
			// arrange
			_selectorsService.SelectorList.Returns(new List<string> { "mcfc-advert" });
			_urlsService.UrlList.Returns(new List<string> { url });
			_webCrawlerService.TagsList.Returns(new List<TagModel> { new TagModel(url, "<input value=\"//g.co/doodle/3e34yp\" class=\"ddl-shortlink\" type=\"hidden\">") }); // todo: add this test also!
			_scanCompletedStatus = ScanCompletedStatus.Successfully;
			// act
			_engine.Scan();
		}

		private void _engine_ScanCompleted(object sender, ScanCompletedEventArgs e)
		{
			// assert
			Assert.AreEqual(_scanCompletedStatus, e.Status);
		}
	}
}
