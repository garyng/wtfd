using System.Collections.Generic;
using System.IO;
using Wtfd.Models;

namespace Wtfd.Commands.Find
{
	/// <summary>
	/// Similar to <see cref="Docs"/>, but no nested docs.
	/// </summary>
	public class FlattenDoc
	{
		public string Pattern { get; set; }

		public IEnumerable<string> Descriptions { get; set; }

		public static List<FlattenDoc> FromDictionary(Dictionary<string, Docs> docs, string parentPattern = "")
		{
			var flattenDocs = new List<FlattenDoc>();
			foreach (var (pattern, value) in docs)
			{
				var combinedPattern = Path.Combine(parentPattern, pattern);

				if (value.IsExpanded)
				{
					flattenDocs.AddRange(FlattenDoc.FromDictionary(value.NestedDocs, combinedPattern));
				}
				else
				{
					flattenDocs.Add(new FlattenDoc()
					{
						Pattern = combinedPattern,
						Descriptions = value.Descriptions
					});
				}
			}

			return flattenDocs;
		}
	}
}