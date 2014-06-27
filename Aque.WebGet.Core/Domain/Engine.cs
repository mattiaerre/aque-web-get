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
		private readonly List<NodeModel> _nodes = new List<NodeModel>();
		private int _urlsRemaining;

		public IEnumerable<NodeModel> Nodes
		{
			get { return _nodes; }
		}

		public Engine(ISelectorsService selectorsService, IUrlsService urlsService, IWebCrawlerService webCrawlerService)
		{
			_selectorsService = selectorsService;
			_urlsService = urlsService;
			_webCrawlerService = webCrawlerService;
		}

		private void _webCrawlerService_CrawlCompleted(object sender, EventArgs e)
		{
			_nodes.AddRange(_webCrawlerService.NodeList);
			if (_urlsRemaining == 0)
				ScanCompleted(this, new ScanCompletedEventArgs(ScanCompletedStatus.Successfully));
		}

		private void _urlsService_LoadUrlListCompleted(object sender, EventArgs e)
		{
			_urls = _urlsService.UrlList.ToList();
			_urlsRemaining = _urls.Count();
			if (!_urls.Any())
				ScanCompleted(this, new ScanCompletedEventArgs(ScanCompletedStatus.UrlListEmpty));
			else
			{
				foreach (var url in _urls)
				{
					var list = new List<string>();
					foreach (var selector in _selectors)
					{
						// todo: make this configurable as well
						list.Add(string.Format("//div[@class='{0}']", selector));
					}
					_urlsRemaining = _urlsRemaining - 1;
					_webCrawlerService.Crawl(url, list);
				}
			}
		}

		private void _selectorsService_LoadSelectorListCompleted(object sender, EventArgs e)
		{
			_selectors = _selectorsService.SelectorList;
			if (!_selectors.Any())
				ScanCompleted(this, new ScanCompletedEventArgs(ScanCompletedStatus.SelectorListEmpty));
			else
				_urlsService.LoadUrlList();
		}

		public void Init()
		{
			_selectorsService.LoadSelectorListCompleted += _selectorsService_LoadSelectorListCompleted;
			_urlsService.LoadUrlListCompleted += _urlsService_LoadUrlListCompleted;
			_webCrawlerService.CrawlCompleted += _webCrawlerService_CrawlCompleted;
		}

		public void Scan()
		{
			_selectorsService.LoadSelectorList();
		}
	}
}