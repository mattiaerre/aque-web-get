using System;
using System.Collections.Generic;

namespace Aque.WebGet.Core.Services
{
	public interface IUrlsService
	{
		event EventHandler LoadUrlListCompleted;
		IEnumerable<string> UrlList { get; }
		void LoadUrlList();
	}
}