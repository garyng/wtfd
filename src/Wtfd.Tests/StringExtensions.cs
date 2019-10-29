using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wtfd.Tests
{
	public static class StringExtensions
	{
		/// <summary>
		/// Replace `\` to `/` on Windows. Ugghhh...
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static string ToPosixPath(this string @this)
		{
			return @this.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}

		public static IEnumerable<string> ToPosixPath(this IEnumerable<string> @this) =>
			@this.Select(p => ToPosixPath((string) p));
	}
}