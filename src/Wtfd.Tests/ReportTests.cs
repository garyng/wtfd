using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Wtfd.Commands.Find;
using Wtfd.Commands.Report;

namespace Wtfd.Tests
{
	public class ReportTests
	{
		[TestCase("C:/A/", "C:/A/wtfd.json", new[] {"Descriptions for root directory"})]
		[TestCase("C:/A/B/", "C:/A/wtfd.json", new[] { "Descriptions for directory B" })]
		[TestCase("C:/A/D/D1/", "C:/A/wtfd.json", new[] { "Descriptions for directory D1" })]
		public async Task Should_Return_TheDescriptions(string currentDir, string source, string[] desc)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.NestedConfig(isRoot: true).config.ToMockFileData()},
			}, currentDir);
			MockData.AddDirectories(fs);

			var configs = (await new FindCommandHandler(fs).Handle(new FindRequest(), CancellationToken.None))
				.Configurations;
			var reporter = new ReportCommandHandler(fs);

			// Act
			var result = await reporter.Handle(new ReportRequest
			{
				Configurations = configs
			}, CancellationToken.None);

			// Assert
			result.Source.ToPosixPath().Should().Be(source);
			result.Descriptions.Should().BeEquivalentTo(desc);
		}


		[Test]
		public async Task Should_Return_NotFound()
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.NestedConfig(isRoot: true).config.ToMockFileData()},
			}, "C:/A/E/");
			MockData.AddDirectories(fs);

			var configs = (await new FindCommandHandler(fs).Handle(new FindRequest(), CancellationToken.None))
				.Configurations;
			var reporter = new ReportCommandHandler(fs);

			// Act
			var result = await reporter.Handle(new ReportRequest
			{
				Configurations = configs
			}, CancellationToken.None);

			// Assert
			result.NotFound.Should().BeTrue();
		}
	}
}