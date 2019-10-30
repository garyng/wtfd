using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wtfd.Extensions;
using Wtfd.Models;

namespace Wtfd.Commands.Init
{
	public class InitCommandHandler : CommandHandlerBase, IRequestHandler<InitRequest, InitResponse>
	{
		public InitCommandHandler(IFileSystem fs) : base(fs)
		{
		}

		public async Task<InitResponse> Handle(InitRequest request, CancellationToken cancellationToken)
		{
			var target =
				_fs.Path.Combine(_fs.Path.GetFullPath(request.Target ?? _fs.Directory.GetCurrentDirectorySafe()),
					Constants.CONFIG_FILENAME);

			var config = new Configuration()
			{
				IsRoot = request.IsRoot,
				Version = 1,
				Docs = new Dictionary<string, Docs>
				{
					[""] = new Docs
					{
						Descriptions = new[] {"Documentation for root folder."}
					}
				}
			};
			var json = Json.Serialize(config);

			if (_fs.File.Exists(target))
			{
				if (request.Overwrite)
				{
					await _fs.File.WriteAllTextAsync(target, json);
				}
				else
				{
					return new InitResponse
					{
						Response = InitResponses.FileExists
					};
				}
			}
			else
			{
				await _fs.File.WriteAllTextAsync(target, json);
			}

			return new InitResponse
			{
				Response = InitResponses.Success,
				Target = target
			};
		}
	}
}