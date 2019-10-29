using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wtfd.Models;

namespace Wtfd.Verbs.Find
{
	public class FindVerbHandler : IRequestHandler<FindRequest, FindResponse>
	{
		private readonly IFileSystem _fs;

		public FindVerbHandler(IFileSystem fs)
		{
			_fs = fs;
		}

		public async Task<FindResponse> Handle(FindRequest request, CancellationToken cancellationToken)
		{
			var configs = new List<Configuration>();
			var files = new List<string>();
			var current = _fs.DirectoryInfo.FromDirectoryName(request.Path ?? _fs.Directory.GetCurrentDirectory());
			do
			{
				var configFile = _fs.Path.Combine(current.FullName, Constants.CONFIG_FILENAME);
				if (_fs.File.Exists(configFile))
				{
					var config = Json.Deserialize<Configuration>(await _fs.File.ReadAllTextAsync(configFile));
					files.Add(configFile);
					configs.Add(config);

					// stop searching if the current config is marked as the root config
					if (config.IsRoot) break;
				}

				current = current.Parent;
			} while (current != null);

			return new FindResponse
			{
				ConfigurationFiles = files,
				Configurations = configs
			};
		}
	}
}