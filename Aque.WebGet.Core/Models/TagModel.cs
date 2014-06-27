namespace Aque.WebGet.Core.Models
{
	public class TagModel
	{
		public string Url { get; private set; }
		public string InnerHtml { get; private set; }

		public TagModel(string url, string innerHtml)
		{
			Url = url;
			InnerHtml = innerHtml;
		}
	}
}