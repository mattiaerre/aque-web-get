using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Aque.WebGet.Core.Services
{
	public class UrlsService : IUrlsService
	{
		private readonly XElement _map;
		private readonly XNamespace _xNamespace;

		public UrlsService(XElement map, XNamespace xNamespace)
		{
			_map = map;
			_xNamespace = xNamespace;
		}

		public event EventHandler LoadUrlListCompleted;
		public IEnumerable<string> UrlList { get; private set; }
		public void LoadUrlList()
		{
			var list = new List<string>();
			foreach (var loc in _map.Descendants(_xNamespace + "loc"))
			{
				list.Add(loc.Value);
			}
			UrlList = list;
			LoadUrlListCompleted(this, new EventArgs());
		}
	}
}