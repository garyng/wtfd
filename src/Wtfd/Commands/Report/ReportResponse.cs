using System.Collections.Generic;

namespace Wtfd.Commands.Report
{
	public class ReportResponse
	{
		/// <summary>
		/// Path to the configuration file containing the matched descriptions.
		/// </summary>
		public string Source { get; set; }

		public IEnumerable<string> Descriptions { get; set; }

		public bool NotFound { get; set; }
	}
}