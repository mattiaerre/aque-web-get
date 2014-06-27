using System;
using System.Collections.Generic;
using System.Linq;

namespace Aque.WebGet.Core.Services
{
	public class SelectorsService : ISelectorsService
	{
		private readonly string _selectors;

		public SelectorsService(string selectors)
		{
			_selectors = selectors;
		}

		public event EventHandler LoadSelectorListCompleted;
		public IEnumerable<string> SelectorList { get; private set; }
		public void LoadSelectorList()
		{
			SelectorList = _selectors.Split('|').ToList(); ;
			LoadSelectorListCompleted(this, new EventArgs());
		}
	}
}