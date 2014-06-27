using Aque.WebGet.Core.Models;
using Aque.WebGet.Core.Services;
using System;
using System.Collections.Generic;
using LINQtoCSV;

namespace Aque.WebGet.Infra.Services
{
	public class CsvService : ICsvService
	{
		private readonly string _basePath;

		public CsvService(string basePath)
		{
			_basePath = basePath;
		}

		public event EventHandler ExportCompleted;

		public void Export(IEnumerable<NodeModel> list, string fileName)
		{
			var outputFileDescription = new CsvFileDescription
			{
				SeparatorChar = '\t',
				FirstLineHasColumnNames = true,
				FileCultureName = "en-GB"
			};

			var context = new CsvContext();

			context.Write(list, string.Format(@"{0}\{1}.csv", _basePath, fileName), outputFileDescription);

			ExportCompleted(this, new EventArgs());
		}
	}
}