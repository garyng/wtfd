using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Wtfd
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			// Autofac + MediatR + CommandLineParser
		}
	}

	public class FindCommandHandler
	{
		private IFileSystem _fs;

		public FindCommandHandler(IFileSystem fs)
		{
			_fs = fs;
		}

		public FindCommandHandler() : this(new FileSystem())
		{
		}

		public async Task<string> Execute()
		{
			var current = _fs.DirectoryInfo.FromDirectoryName(_fs.Directory.GetCurrentDirectory());
			do
			{
				var configFile = _fs.Path.Combine(current.FullName, "wtfd.json");
				if (_fs.File.Exists(configFile))
				{
					return configFile;
				}
				else
				{
					current = current.Parent;
				}
			} while (current != null);

			return "";
		}
	}

	/// <summary>
	/// Represent a `wtfd.json` file.
	/// </summary>
	public class Configuration
	{
		/// <summary>
		/// The version of the configuration file.
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// Whether the configuration file is the root configuration.
		/// </summary>
		public bool IsRoot { get; set; }

		/// <summary>
		/// All documentations.
		/// </summary>
		public List<Doc> Docs { get; set; }
	}

	/// <summary>
	/// Stores the documentation of a folder match pattern.
	/// </summary>
	public class Doc
	{
		/// <summary>
		/// glob pattern for the documentation.
		/// </summary>
		public string Pattern { get; set; }

		/// <summary>
		/// Descriptions for the folder.
		/// Each entry corresponds to a new line.
		/// </summary>
		public List<string> Descriptions { get; set; }
	}
}