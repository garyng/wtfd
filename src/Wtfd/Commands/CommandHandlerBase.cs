using System.IO.Abstractions;

namespace Wtfd.Commands
{
	public abstract class CommandHandlerBase
	{
		protected readonly IFileSystem _fs;

		public CommandHandlerBase(IFileSystem fs)
		{
			_fs = fs;
		}
	}
}