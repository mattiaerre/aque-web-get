using Aque.WebGet.Core.Services;
using NUnit.Framework;
using System.Linq;

namespace Aque.WebGet.Core.Tests.Services
{
	[TestFixture]
	public class SelectorsService_Tests
	{
		private ISelectorsService _service;

		[SetUp]
		public void Given_A_SelectorsService()
		{
			const string selectors = "class-one|class-two|class-three";

			_service = new SelectorsService(selectors);
			_service.LoadSelectorListCompleted += _service_LoadSelectorListCompleted;
		}

		[Test]
		public void It_Should_Be_Able_To_Populate_A_List_Of_Selectors()
		{
			_service.LoadSelectorList();
		}

		private void _service_LoadSelectorListCompleted(object sender, System.EventArgs e)
		{
			Assert.IsTrue(_service.SelectorList.Any());
		}
	}
}
