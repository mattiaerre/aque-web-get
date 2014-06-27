using System;
using System.Collections.Generic;

namespace Aque.WebGet.Core.Services
{
	public interface ISelectorsService
	{
		event EventHandler LoadSelectorListCompleted;
		IEnumerable<string> SelectorList { get; }
		void LoadSelectorList();
	}
}