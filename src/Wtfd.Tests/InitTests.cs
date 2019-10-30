using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Wtfd.Commands.Find;
using Wtfd.Commands.Init;

namespace Wtfd.Tests
{
	public class InitTests
	{
		[Test]
		public async Task Should_CreateConfig()
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
			}, "C:/A/");
			MockData.AddDirectories(fs);

			var handler = new InitCommandHandler(fs);

			// Act
			var result = await handler.Handle(new InitRequest(), CancellationToken.None);

			// Assert
			result.Response.Should().Be(InitResponses.Success);
			result.Target.ToPosixPath().Should().Be("C:/A/wtfd.json");
		}


		[Test]
		public async Task Should_NotOverwriteExistingConfig()
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.FlatConfig().config.ToMockFileData()}
			}, "C:/A/");
			MockData.AddDirectories(fs);

			var handler = new InitCommandHandler(fs);

			// Act
			var result = await handler.Handle(new InitRequest
			{
				Overwrite = false
			}, CancellationToken.None);

			// Assert
			result.Response.Should().Be(InitResponses.FileExists);
		}

		[Test]
		public async Task Should_OverwriteExistingConfig()
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{"C:/A/wtfd.json", MockData.FlatConfig().config.ToMockFileData()}
			}, "C:/A/");
			MockData.AddDirectories(fs);

			var handler = new InitCommandHandler(fs);

			// Act
			var result = await handler.Handle(new InitRequest
			{
				Overwrite = true
			}, CancellationToken.None);

			// Assert
			result.Response.Should().Be(InitResponses.Success);
		}
	}
}