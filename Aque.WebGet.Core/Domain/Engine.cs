using Aque.WebGet.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Aque.WebGet.Core.Models;
using Aque.WebGet.Core.Services;

namespace Aque.WebGet.Core.Domain
{
	public class Engine : IEngine
	{
		public event EventHandler ScanCompleted;

		private readonly ISelectorsService _selectorsService;
		private readonly IUrlsService _urlsService;
		private readonly IWebCrawlerService _webCrawlerService;

		private IEnumerable<string> _selectors = Enumerable.Empty<string>();
		private IEnumerable<string> _urls = Enumerable.Empty<string>();
		private IEnumerable<TagModel> _tags = Enumerable.Empty<TagModel>();
		public IEnumerable<TagModel> Tags
		{
			get { return _tags; }
		}

		public Engine(ISelectorsService selectorsService, IUrlsService urlsService, IWebCrawlerService webCrawlerService)
		{
			_selectorsService = selectorsService;
			_urlsService = urlsService;
			_webCrawlerService = webCrawlerService;
		}

		private void _webCrawlerService_CrawlCompleted(object sender, EventArgs e)
		{
			_tags = _webCrawlerService.TagsList;
			// info: no need to specify empty list
			ScanCompleted(this, new ScanCompletedEventArgs(ScanCompletedStatus.Successfully));
		}

		private void _urlsService_LoadUrlListCompleted(object sender, EventArgs e)
		{
			_urls = _urlsService.UrlList;
			if (!_urls.Any())
				ScanCompleted(this, new ScanCompletedEventArgs(ScanCompletedStatus.UrlListEmpty));
			else
				_webCrawlerService.Crawl();
		}

		private void _selectorsService_LoadSelectorListCompleted(object sender, EventArgs e)
		{
			_selectors = _selectorsService.SelectorList;
			if (!_selectors.Any())
				ScanCompleted(this, new ScanCompletedEventArgs(ScanCompletedStatus.SelectorListEmpty));
			else
				_urlsService.LoadUrlList();
		}

		public void Scan()
		{
			// todo: move into init method ???
			_selectorsService.LoadSelectorListCompleted += _selectorsService_LoadSelectorListCompleted;
			_urlsService.LoadUrlListCompleted += _urlsService_LoadUrlListCompleted;
			_webCrawlerService.CrawlCompleted += _webCrawlerService_CrawlCompleted;

			try
			{
				_selectorsService.LoadSelectorList();
			}
			catch (Exception ex)
			{
				ScanCompleted(this, new ScanCompletedEventArgs(ScanCompletedStatus.WithErrors, ex));
			}
		}
	}
}