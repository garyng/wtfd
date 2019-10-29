using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Wtfd.Models
{
	/// <summary>
	/// Stores the documentation of a folder match pattern.
	/// Can be entry with only descriptions, or expanded to include nested documentations.
	/// </summary>
	[JsonConverter(typeof(DocsConverter))]
	public class Docs
	{
		/// <summary>
		/// Descriptions for the matched directory.
		/// </summary>
		public IEnumerable<string> Descriptions { get; set; }


		/// <summary>
		/// Documentations that are nested.
		/// The match pattern is relative to the parent doc as well.
		/// </summary>
		public Dictionary<string, Docs> NestedDocs { get; set; }
	}
}