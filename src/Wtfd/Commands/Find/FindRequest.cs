using CommandLine;
using MediatR;

namespace Wtfd.Commands.Find
{
	[Verb("find", HelpText = "Find all the configurations files for a directory.")]
	public class FindRequest : IRequest<FindResponse>
	{
		[Option('t', "target", Required = false,
			HelpText = "Specify the target directory. Current directory will be used otherwise.")]
		public string Target { get; set; }
	}
}