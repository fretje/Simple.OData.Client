﻿using System.Collections.Generic;

namespace Simple.OData.Client
{
	public class ReferenceLink
	{
		public string LinkName { get; set; }
		public object LinkData { get; set; }
		public string ContentId { get; set; }
	}

	public class EntryDetails
	{
		private readonly IDictionary<string, List<ReferenceLink>> _links = new Dictionary<string, List<ReferenceLink>>();

		public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
		public IDictionary<string, List<ReferenceLink>> Links => _links;
		public bool HasOpenTypeProperties { get; set; }

		public void AddProperty(string propertyName, object propertyValue)
		{
			Properties.Add(propertyName, propertyValue);
		}

		public void AddLink(string linkName, object linkData, string contentId = null)
		{
			if (!_links.TryGetValue(linkName, out var links))
			{
				links = new List<ReferenceLink>();
				_links.Add(linkName, links);
			}
			links.Add(new ReferenceLink() { LinkName = linkName, LinkData = linkData, ContentId = contentId });
		}
	}
}

