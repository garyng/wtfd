using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wtfd.Extensions;
using Wtfd.Models;

namespace Wtfd.Commands.Find
{
	public class FindCommandHandler : IRequestHandler<FindRequest, FindResponse>
	{
		private readonly IFileSystem _fs;

		public FindCommandHandler(IFileSystem fs)
		{
			_fs = fs;
		}

		public async Task<FindResponse> Handle(FindRequest request, CancellationToken cancellationToken)
		{
			var configs = new List<ConfigurationRo>();
			var current = _fs.DirectoryInfo.FromDirectoryName(request.Target ?? _fs.Directory.GetCurrentDirectorySafe());
			do
			{
				// absolute path to the config filename
				var configFile = _fs.Path.Combine(current.FullName, Constants.CONFIG_FILENAME);
				if (_fs.File.Exists(configFile))
				{
					var config = Json.Deserialize<Configuration>(await _fs.File.ReadAllTextAsync(configFile));
					configs.Add(new ConfigurationRo
					{
						Configuration = config,
						Path = configFile
					});

					// stop searching if the current config is marked as the root config
					if (config.IsRoot) break;
				}

				current = current.Parent;
			} while (current != null);

			return new FindResponse
			{
				Configurations = configs
			};
		}
	}
}