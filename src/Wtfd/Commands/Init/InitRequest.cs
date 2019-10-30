using CommandLine;
using MediatR;

namespace Wtfd.Commands.Init
{
	[Verb("init", HelpText = "Initialize skeleton documentation json file.")]
	public class InitRequest : IRequest<InitResponse>
	{
		[Option('t', "target", Required = false,
			HelpText = "Specify the target directory. Current directory will be used otherwise.")]
		public string Target { get; set; }

		[Option('o', "overwrite", Required = false, Default = false, HelpText = "Overwrite if target exists.")]
		public bool Overwrite { get; set; }

		[Option('r', "root", Required = false, Default = true, HelpText = "Create a root configuration file")]
		public bool IsRoot { get; set; }
	}
}