using System;
using System.Collections.Generic;
using Aque.WebGet.Core.Models;

namespace Aque.WebGet.Core.Domain
{
	public interface IEngine
	{
		event EventHandler ScanCompleted;
		IEnumerable<NodeModel> Nodes { get; }
		void Init();
		void Scan();
	}
}