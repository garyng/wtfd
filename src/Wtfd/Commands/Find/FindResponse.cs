using System.Collections.Generic;

namespace Wtfd.Commands.Find
{
	public class FindResponse
	{
		/// <summary>
		/// List of configurations found
		/// </summary>
		public List<ConfigurationRo> Configurations { get; set; }
	}
}