using CommandLine;
using MediatR;

namespace Wtfd.Verbs.Find
{
	[Verb("find", HelpText = "Find all the configurations files for a directory.")]
	public class FindRequest : IRequest<FindResponse>
	{
		[Option('p', "path", Required = false,
			HelpText = "Specify the target directory. Current directory will be used otherwise.")]
		public string Path { get; set; }
	}
}