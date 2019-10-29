using System.Collections.Generic;
using Wtfd.Models;

namespace Wtfd.Verbs.Find
{
	public class FindResponse
	{
		/// <summary>
		/// The paths of configuration files found.
		/// </summary>
		public List<string> ConfigurationFiles { get; set; }

		/// <summary>
		/// List of configurations found.
		/// </summary>
		public Dictionary<string, Configuration> Configurations { get; set; }
	}
}