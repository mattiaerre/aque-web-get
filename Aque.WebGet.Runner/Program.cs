using Aque.WebGet.Core.Domain;
using Aque.WebGet.Core.Services;
using Aque.WebGet.Infra.Services;
using System;
using System.Xml.Linq;
using Aque.WebGet.Runner.Properties;

namespace Aque.WebGet.Runner
{
	public class Program
	{
		private static IEngine _engine;
		private static ICsvService _service;
		private const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss.fff";

		private static void Bootstrap()
		{
			// todo: add container

			var selectors = Settings.Default.Selectors;
			var selectorsService = new SelectorsService(selectors);
			var map = XElement.Load(Settings.Default.SiteMapPath);
			//XNamespace xNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
			XNamespace xNamespace = string.Empty;
			var urlsService = new UrlsService(map, xNamespace);
			var webCrawlerService = new WebCrawlerService();
			_engine = new Engine(selectorsService, urlsService, webCrawlerService);
			_engine.ScanCompleted += engine_ScanCompleted;

			var basePath = Settings.Default.CsvBasePath;
			_service = new CsvService(basePath);
			_service.ExportCompleted += service_ExportCompleted;
		}

		public static void Main(string[] args)
		{
			Bootstrap();
			Console.WriteLine("start: {0}", DateTime.UtcNow.ToString(DateTimeFormat));
			Console.WriteLine("waiting for export to complete ...");
			try
			{
				_engine.Init();
				_engine.Scan();
			}
			catch (Exception ex)
			{
				Console.WriteLine("exception Message: {0}", ex.Message);
				Console.WriteLine("exception StackTrace: {0}", ex.StackTrace);
				Console.ReadLine();
			}
		}

		private static void engine_ScanCompleted(object sender, EventArgs e)
		{
			try
			{
				_service.Export(_engine.Nodes, Settings.Default.CsvFileName);
			}
			catch (Exception ex)
			{
				Console.WriteLine("exception Message: {0}", ex.Message);
				Console.WriteLine("exception StackTrace: {0}", ex.StackTrace);
				Console.ReadLine();
			}
		}

		private static void service_ExportCompleted(object sender, EventArgs e)
		{
			Console.WriteLine("export completed");
			Console.WriteLine("stop: {0}", DateTime.UtcNow.ToString(DateTimeFormat));
			Console.ReadLine();
		}
	}
}
