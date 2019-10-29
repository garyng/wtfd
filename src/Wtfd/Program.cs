using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using CommandLine;
using Wtfd.Verbs.Find;

namespace Wtfd
{
	class Program
	{
		static Task Main(string[] args)
		{
			var builder = new ContainerBuilder();
			builder.AddMediatR(typeof(IMediatorMarker).Assembly);
			builder.RegisterType<FileSystem>().As<IFileSystem>();

			var container = builder.Build();

			var mediator = container.Resolve<IMediator>();
			return Parser.Default
				.ParseArguments<FindRequest, EditRequest>(args)
				.MapResult(
					(FindRequest find) => onFind(mediator, find),
					(EditRequest edit) => Task.CompletedTask,
					errors => Task.CompletedTask);
		}

		private static async Task onFind(IMediator mediator, FindRequest find)
		{
			var result = await mediator.Send(find);
			Console.WriteLine(string.Join(Environment.NewLine, result.ConfigurationFiles));
		}
	}

	[Verb("edit", HelpText = "Edit the description for a directory.")]
	public class EditRequest : IRequest
	{
	}

	[Verb("report", HelpText = "Print the description for a directory.")]
	public class ReportRequest : IRequest
	{

	}
}