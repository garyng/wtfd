using System.Collections.Generic;

namespace Wtfd.Models
{
	/// <summary>
	/// Represent a `wtfd.json` file.
	/// </summary>
	public class Configuration
	{
		/// <summary>
		/// The version of the configuration file.
		/// </summary>
		public int Version { get; set; }

		/// <summary>
		/// Whether the configuration file is the root configuration.
		/// </summary>
		public bool IsRoot { get; set; }

		/// <summary>
		/// All documentations, the `key` is the match pattern for the directory.
		/// </summary>
		public Dictionary<string, Docs> Docs { get; set; }
	}
}