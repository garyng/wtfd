using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Globbing;
using MediatR;
using Wtfd.Extensions;

namespace Wtfd.Commands.Report
{
	public class ReportCommandHandler : IRequestHandler<ReportRequest, ReportResponse>
	{
		private readonly IFileSystem _fs;

		public ReportCommandHandler(IFileSystem fs)
		{
			_fs = fs;
		}

		public async Task<ReportResponse> Handle(ReportRequest request, CancellationToken cancellationToken)
		{
			// full path ends with directory separator char
			var targetDirectory = _fs.Path.GetFullPath(request.Target ?? _fs.Directory.GetCurrentDirectorySafe());
			// todo: check for target directory?

			var desc =
				from config in request.Configurations
				from doc in config.FlattenDocs.Value
				where Glob.Parse(doc.Pattern).IsMatch(targetDirectory)
				select new ReportResponse
				{
					Source = config.Path,
					Descriptions = doc.Descriptions
				};

			return desc.FirstOrDefault() ?? new ReportResponse() {NotFound = true};
		}
	}
}