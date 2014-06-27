using System;
using System.Collections.Generic;
using Aque.WebGet.Core.Models;

namespace Aque.WebGet.Core.Services
{
	public interface IWebCrawlerService
	{
		event EventHandler CrawlCompleted;
		IEnumerable<TagModel> TagsList { get; }
		void Crawl();
	}
}