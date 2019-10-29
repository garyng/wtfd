using System.IO.Abstractions;

namespace Wtfd.Extensions
{
	public static class DirectoryExtensions
	{
		/// <summary>
		/// Return current directory appended with directory separator.
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static string GetCurrentDirectorySafe(this IDirectory @this) =>
			$"{@this.GetCurrentDirectory()}{@this.FileSystem.Path.DirectorySeparatorChar}";
	}
}