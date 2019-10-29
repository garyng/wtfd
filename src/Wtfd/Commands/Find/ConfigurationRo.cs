using System;
using System.Collections.Generic;
using System.IO;
using Wtfd.Models;

namespace Wtfd.Commands.Find
{
	/// <summary>
	/// A response object for <see cref="Configuration"/>
	/// </summary>
	public class ConfigurationRo
	{
		/// <summary>
		/// The deserialized configuration.
		/// </summary>
		public Configuration Configuration { get; set; }

		/// <summary>
		/// The absolute path to the configuration file.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// <see cref="Configuration"/>'s <code>Docs</code> flatten.
		/// </summary>
		public Lazy<IEnumerable<FlattenDoc>> FlattenDocs =>
			new Lazy<IEnumerable<FlattenDoc>>(() =>
				FlattenDoc.FromDictionary(Configuration.Docs,
					// ensure the directory ends with a separator character
					$"{Directory.GetParent(Path).FullName}{System.IO.Path.DirectorySeparatorChar}"));
	}
}