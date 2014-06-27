using Aque.WebGet.Core.Models;
using Aque.WebGet.Core.Services;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;

namespace Aque.WebGet.Infra.Services
{
	public class WebCrawlerService : IWebCrawlerService
	{
		public event EventHandler CrawlCompleted;
		public IEnumerable<NodeModel> NodeList { get; private set; }
		public void Crawl(string url, IEnumerable<string> xpaths)
		{
			using (var client = new WebClient())
			{
				var html = client.DownloadString(url);

				var doc = new HtmlDocument();
				doc.LoadHtml(html);

				var list = new List<NodeModel>();
				foreach (var xpath in xpaths)
				{
					var nodes = doc.DocumentNode.SelectNodes(xpath);
					if (nodes == null) continue;
					foreach (var node in nodes)
					{
						var innerHtml = MakeInline(node.InnerHtml);

						list.Add(new NodeModel(url, innerHtml));
					}
				}
				NodeList = list;
				CrawlCompleted(this, new EventArgs());
			}
		}

		private static string MakeInline(string innerHtml)
		{
			innerHtml = innerHtml.Replace("\r", "");
			innerHtml = innerHtml.Replace("\n", "");
			return innerHtml;
		}
	}
}