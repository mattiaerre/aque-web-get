using System.Collections.Generic;
using System.IO;
using Aque.WebGet.Core.Models;
using Aque.WebGet.Core.Services;
using Aque.WebGet.Infra.Services;
using NUnit.Framework;

namespace Aque.WebGet.Infra.Tests.Services
{
	[TestFixture]
	public class CsvService_Tests
	{
		private ICsvService _service;

		private const string BasePath = @"D:\TMP";
		private const string Url = "http://www.google.com";
		private const string InnerHtml = "<input value=\"//g.co/doodle/3e34yp\" class=\"ddl-shortlink\" type=\"hidden\">";
		private const string FileName = "001";

		[SetUp]
		public void Given_A_CsvService()
		{
			_service = new CsvService(BasePath);
			_service.ExportCompleted += _service_ExportCompleted;
		}

		[Test]
		public void It_Should_Be_Able_To_Export_Into_Csv()
		{
			var list = new List<NodeModel> { new NodeModel(Url, InnerHtml) };

			_service.Export(list, FileName);
		}

		private void _service_ExportCompleted(object sender, System.EventArgs e)
		{
			var exists = File.Exists(string.Format(@"{0}\{1}.csv", BasePath, FileName));

			Assert.IsTrue(exists);
		}
	}
}
