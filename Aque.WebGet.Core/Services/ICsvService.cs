using System;
using System.Collections.Generic;
using Aque.WebGet.Core.Models;

namespace Aque.WebGet.Core.Services
{
	public interface ICsvService
	{
		event EventHandler ExportCompleted;
		void Export(IEnumerable<NodeModel> list, string fileName);
	}
}