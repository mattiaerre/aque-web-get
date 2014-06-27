namespace Aque.WebGet.Core.Models
{
	public class NodeModel
	{
		public string Url { get; private set; }
		public string InnerHtml { get; private set; }

		public NodeModel(string url, string innerHtml)
		{
			Url = url;
			InnerHtml = innerHtml;
		}
	}
}