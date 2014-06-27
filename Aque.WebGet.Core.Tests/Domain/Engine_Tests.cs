using System;
using System.Collections.Generic;
using System.Linq;
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

		private const string Url = "http://www.google.com";
		private const string CssClass = "mcfc-advert";
		private const string InnerHtml = "<input value=\"//g.co/doodle/3e34yp\" class=\"ddl-shortlink\" type=\"hidden\">";

		[SetUp]
		public void Given_An_Engine()
		{
			_selectorsService = Substitute.For<ISelectorsService>();

			_urlsService = Substitute.For<IUrlsService>();
			_urlsService.When(e => e.LoadUrlList())
				.Do(e => _urlsService.LoadUrlListCompleted += Raise.EventWith(new object(), new EventArgs()));

			_webCrawlerService = Substitute.For<IWebCrawlerService>();
			_webCrawlerService.When(e => e.Crawl(Arg.Any<string>(), Arg.Any<IEnumerable<string>>()))
				.Do(e => _webCrawlerService.CrawlCompleted += Raise.EventWith(new object(), new EventArgs()));

			_engine = new Engine(_selectorsService, _urlsService, _webCrawlerService);
			_engine.ScanCompleted += (s, e) => _engine_ScanCompleted(s, (ScanCompletedEventArgs)e);

			_engine.Init();
		}

		private void SelectorsServiceSetUp()
		{
			// arrange
			_selectorsService.When(e => e.LoadSelectorList())
				.Do(e => _selectorsService.LoadSelectorListCompleted += Raise.EventWith(new object(), new EventArgs()));
		}

		[Test]
		public void It_Should_Be_Able_To_Raise_SelectorListEmpty()
		{
			// arrange
			SelectorsServiceSetUp();
			_scanCompletedStatus = ScanCompletedStatus.SelectorListEmpty;
			// act
			_engine.Scan();
		}

		[Test]
		public void It_Should_Be_Able_To_Raise_UrlListEmpty()
		{
			// arrange
			SelectorsServiceSetUp();
			_selectorsService.SelectorList.Returns(new List<string> { CssClass });
			_scanCompletedStatus = ScanCompletedStatus.UrlListEmpty;
			// act
			_engine.Scan();
		}

		[Test]
		public void It_Should_Be_Able_To_Raise_Successfully()
		{
			// arrange
			SelectorsServiceSetUp();
			_selectorsService.SelectorList.Returns(new List<string> { CssClass });
			_urlsService.UrlList.Returns(new List<string> { Url });
			_webCrawlerService.NodeList.Returns(new List<NodeModel> { new NodeModel(Url, InnerHtml) });
			_scanCompletedStatus = ScanCompletedStatus.Successfully;
			// act
			_engine.Scan();
			// assert
			Assert.IsTrue(_engine.Nodes.Any());
		}

		//[Test]
		//public void It_Should_Be_Able_To_Raise_WithErrors()
		//{
		//	// arrange
		//	_selectorsService.When(e => e.LoadSelectorList()).Do(e => { throw new Exception("ISelectorsService has thrown an exception"); });
		//	_scanCompletedStatus = ScanCompletedStatus.WithErrors;
		//	// act
		//	_engine.Scan();
		//}

		private void _engine_ScanCompleted(object sender, ScanCompletedEventArgs e)
		{
			// assert
			Assert.AreEqual(_scanCompletedStatus, e.Status);
		}
	}
}
