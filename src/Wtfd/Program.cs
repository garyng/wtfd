using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using CommandLine;
using Wtfd.Commands.Find;
using Wtfd.Commands.Report;

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
				.ParseArguments<FindRequest, ReportRequest>(args)
				.MapResult(
					(FindRequest find) => onFind(mediator, find),
					(ReportRequest report) => onReport(mediator, report),
					errors => Task.CompletedTask);
		}

		private static async Task onFind(IMediator mediator, FindRequest find)
		{
			var result = await mediator.Send(find);
			Console.WriteLine(string.Join(Environment.NewLine, result.Configurations.Select(c => c.Path)));
		}

		private static async Task onReport(IMediator mediator, ReportRequest report)
		{
			var found = await mediator.Send(new FindRequest()
			{
				Target = report.Target
			});

			if (found.Configurations?.Any() == false)
			{
				Console.WriteLine("No configuration files found.");
				return;
			}

			report.Configurations = found.Configurations;
			var result = await mediator.Send(report);
			if (result.NotFound)
			{
				Console.WriteLine("Documentation not found.");
				return;
			}

			Console.WriteLine($"From {result.Source}");
			Console.WriteLine(string.Join(Environment.NewLine, result.Descriptions));
		}
	}
}