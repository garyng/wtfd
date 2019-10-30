using System.Collections.Generic;
using CommandLine;
using MediatR;
using Wtfd.Commands.Find;

namespace Wtfd.Commands.Report
{

	[Verb("report", HelpText = "Print the description for a directory.")]
	public class ReportRequest : IRequest<ReportResponse>
	{
		[Option('t', "target", Required = false,
			HelpText = "Specify the target directory. Current directory will be used otherwise.")]
		public string Target { get; set; }

		/// <summary>
		/// Lis of configuration files, from <see cref="FindResponse"/>.
		/// </summary>
		public List<ConfigurationRo> Configurations { get; set; }
	}
}