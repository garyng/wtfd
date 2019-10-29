using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Wtfd.Commands.Find;

namespace Wtfd.Tests
{
	public class FindTests
	{
		[TestCase("C:/A/", new[] {"C:/A/wtfd.json"})]
		[TestCase("C:/A/B/", new[] {"C:/A/B/wtfd.json", "C:/A/wtfd.json"})]
		[TestCase("C:/A/D/D1/", new[] {"C:/A/D/D1/wtfd.json", "C:/A/wtfd.json"})]
		public async Task Should_Return_AllConfigFilesFound(string currentDir, string[] expected)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{"C:/A/B/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{"C:/A/D/D1/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
			}, currentDir);
			MockData.AddDirectories(fs);

			var finder = new FindCommandHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);

			// Assert
			result.Configurations.Select(c => c.Path.ToPosixPath()).Should().BeEquivalentTo(expected);
		}


		[TestCase("C:/A/", new[] {"C:/A/wtfd.json"})]
		[TestCase("C:/A/B/", new[] {"C:/A/B/wtfd.json", "C:/A/wtfd.json"})]
		[TestCase("C:/A/D/D1/", new[] {"C:/A/D/D1/wtfd.json", "C:/A/wtfd.json"})]
		public async Task Should_Return_AllConfigFilesFound_UsingSpecifiedPath(string currentDir, string[] expected)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{"C:/A/B/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{"C:/A/D/D1/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
			}, "C:/");
			MockData.AddDirectories(fs);

			var finder = new FindCommandHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest
			{
				Target = currentDir
			}, CancellationToken.None);

			// Assert
			result.Configurations.Select(c => c.Path.ToPosixPath()).Should().BeEquivalentTo(expected);
		}


		[TestCase("C:/A/")]
		[TestCase("C:/A/B/")]
		[TestCase("C:/A/D/D1/")]
		public async Task Should_ReturnNothing_When_NoConfigFiles(string currentDir)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>(), currentDir);
			MockData.AddDirectories(fs);

			var finder = new FindCommandHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);

			// Assert
			result.Configurations.Select(c => c.Path).Should().BeEmpty();
		}

		[Test]
		public async Task Should_StopAtIsRoot()
		{
			// Arrange
			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{"C:/A/D/wtfd.json", MockData.RootConfig().ToMockFileData()},
				{"C:/A/D/D1/wtfd.json", MockData.EmptyConfig().ToMockFileData()},
			}, "C:/A/D/D1/");
			MockData.AddDirectories(fs);

			var finder = new FindCommandHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);


			// Assert
			result.Configurations.Select(c => c.Path.ToPosixPath()).Should().BeEquivalentTo(new[]
			{
				"C:/A/D/D1/wtfd.json",
				"C:/A/D/wtfd.json",
			});
		}


		[Test]
		public async Task Should_FlattenConfigsFound()
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.NestedConfig(isRoot: true).config.ToMockFileData()},
				{"C:/A/B/wtfd.json", MockData.FlatConfig().config.ToMockFileData()},
			}, "C:/A/B/");
			MockData.AddDirectories(fs);

			var finder = new FindCommandHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);
			var flatten = result.Configurations.SelectMany(c => c.FlattenDocs.Value);
			var patterns = flatten.Select(i => i.Pattern.ToPosixPath());

			// Assert
			patterns.Should().BeEquivalentTo(new[]
			{
				"C:/A/B/",
				"C:/A/B/B/",
				"C:/A/B/C/",
				"C:/A/",
				"C:/A/B/",
				"C:/A/C/",
				"C:/A/D/",
				"C:/A/D/D1/",
			}.ToPosixPath());
		}
	}
}