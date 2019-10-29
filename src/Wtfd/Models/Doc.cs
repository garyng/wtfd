using System.Collections.Generic;

namespace Wtfd.Models
{
	/// <summary>
	/// Stores the documentation of a folder match pattern.
	/// </summary>
	public class Doc
	{
		/// <summary>
		/// glob pattern for the documentation.
		/// </summary>
		public string Pattern { get; set; }

		/// <summary>
		/// Descriptions for the folder.
		/// Each entry corresponds to a new line.
		/// </summary>
		public List<string> Descriptions { get; set; }
	}
}