using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Wtfd.Tests
{
	public class FindConfigurationFileTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[TestCase(@"C:\A\", @"C:\A\wtfd.json")]
		[TestCase(@"C:\A\B\", @"C:\A\B\wtfd.json")]
		[TestCase(@"C:\A\D\D1", @"C:\A\D\D1\wtfd.json")]
		public async Task Should_Return_ConfigFileInTheSameDirectory(string currentDir, string expected)
		{
			// Arrange
			
			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{@"C:\A\wtfd.json", MockFileData.NullObject},
				{@"C:\A\B\wtfd.json", MockFileData.NullObject},
				{@"C:\A\D\D1\wtfd.json", MockFileData.NullObject},
			}, currentDir);
			addDirectories(fs);

			var finder = new FindCommandHandler(fs);

			// Act
			var result = await finder.Execute();

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}


		[TestCase(@"C:\A\B", @"C:\A\wtfd.json")]
		[TestCase(@"C:\A\D\D1", @"C:\A\D\wtfd.json")]
		public async Task Should_Return_ConfigFileInTheParentDirectory(string currentDir, string expected)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{@"C:\A\wtfd.json", MockFileData.NullObject},
				{@"C:\A\D\wtfd.json", MockFileData.NullObject},
			}, currentDir);
			addDirectories(fs);

			var finder = new FindCommandHandler(fs);

			// Act
			var result = await finder.Execute();

			// Assert
			Assert.That(result, Is.EqualTo(expected));
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
	}
}