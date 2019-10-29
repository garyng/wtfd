using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wtfd.Models;
using Wtfd.Verbs.Find;

namespace Wtfd.Tests
{
	public class FindConfigurationFileTests
	{
		[TestCase(@"C:\A\", new[] {@"C:\A\wtfd.json"})]
		[TestCase(@"C:\A\B\", new[] {@"C:\A\B\wtfd.json", @"C:\A\wtfd.json"})]
		[TestCase(@"C:\A\D\D1", new[] {@"C:\A\D\D1\wtfd.json", @"C:\A\wtfd.json"})]
		public async Task Should_Return_AllConfigFilesFound(string currentDir, string[] expected)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{@"C:\A\wtfd.json", emptyConfig()},
				{@"C:\A\B\wtfd.json", emptyConfig()},
				{@"C:\A\D\D1\wtfd.json", emptyConfig()},
			}, currentDir);
			addDirectories(fs);

			var finder = new FindVerbHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);

			// Assert
			Assert.That(result.ConfigurationFiles, Is.EquivalentTo(expected));
		}


		[TestCase(@"C:\A\", new[] { @"C:\A\wtfd.json" })]
		[TestCase(@"C:\A\B\", new[] { @"C:\A\B\wtfd.json", @"C:\A\wtfd.json" })]
		[TestCase(@"C:\A\D\D1", new[] { @"C:\A\D\D1\wtfd.json", @"C:\A\wtfd.json" })]
		public async Task Should_Return_AllConfigFilesFound_UsingSpecifiedPath(string currentDir, string[] expected)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{@"C:\A\wtfd.json", emptyConfig()},
				{@"C:\A\B\wtfd.json", emptyConfig()},
				{@"C:\A\D\D1\wtfd.json", emptyConfig()},
			}, @"C:\");
			addDirectories(fs);

			var finder = new FindVerbHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest
			{
				Path = currentDir
			}, CancellationToken.None);

			// Assert
			Assert.That(result.ConfigurationFiles, Is.EquivalentTo(expected));
		}


		[TestCase(@"C:\A\")]
		[TestCase(@"C:\A\B\")]
		[TestCase(@"C:\A\D\D1")]
		public async Task Should_ReturnNothing_When_NoConfigFiles(string currentDir)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>(), currentDir);
			addDirectories(fs);

			var finder = new FindVerbHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);

			// Assert
			Assert.That(result.ConfigurationFiles, Is.Empty);
		}

		[Test]
		public async Task Should_StopAtIsRoot()
		{
			// Arrange
			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{@"C:\A\wtfd.json", emptyConfig()},
				{@"C:\A\D\wtfd.json", rootConfig()},
				{@"C:\A\D\D1\wtfd.json", emptyConfig()},
			}, @"C:\A\D\D1\");
			addDirectories(fs);

			var finder = new FindVerbHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);


			// Assert
			Assert.That(result.ConfigurationFiles, Is.EquivalentTo(new[]
			{
				@"C:\A\D\D1\wtfd.json",
				@"C:\A\D\wtfd.json"
			}));
		}

		private void addDirectories(MockFileSystem fs)
		{
			fs.AddDirectory(@"C:\A\");
			fs.AddDirectory(@"C:\A\B");
			fs.AddDirectory(@"C:\A\C");
			fs.AddDirectory(@"C:\A\D");
			fs.AddDirectory(@"C:\A\D\D1");
			fs.AddDirectory(@"C:\A\D\D2");
		}

		private MockFileData emptyConfig()
		{
			return new MockFileData(Json.Serialize(new Configuration()));
		}

		private MockFileData rootConfig()
		{
			return new MockFileData(Json.Serialize(new Configuration
				{
					IsRoot = true
				})
			);
		}
	}
}