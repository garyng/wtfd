using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wtfd.Models;
using Wtfd.Verbs.Find;

namespace Wtfd.Tests
{
	public static class MockData
	{
		public static Configuration EmptyConfig()
		{
			return new Configuration();
		}

		public static Configuration RootConfig()
		{
			return new Configuration
			{
				IsRoot = true
			};
		}


		public static MockFileData ToMockFileData(this Configuration @this)
		{
			return new MockFileData(Json.Serialize(@this));
		}

		public static (Configuration config, string json) FlatConfig(bool isRoot = false)
		{
			var config = new Configuration
			{
				Version = 1,
				IsRoot = isRoot,
				Docs = new Dictionary<string, Docs>
				{
					[""] = new Docs
					{
						Descriptions = new[] {"Descriptions for root directory"}
					},
					["B"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory B"}
					},
					["C"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory C"}
					}
				}
			};
			var json =
				"{\"version\":1,\"isRoot\":false,\"docs\":{\"\":[\"Descriptions for root directory\"],\"B\":[\"Descriptions for directory B\"],\"C\":[\"Descriptions for directory C\"]}}";
			return (config, json);
		}

		public static (Configuration config, string json) NestedConfig(bool isRoot = false)
		{
			var config = new Configuration
			{
				Version = 1,
				IsRoot = isRoot,
				Docs = new Dictionary<string, Docs>
				{
					[""] = new Docs
					{
						Descriptions = new[] {"Descriptions for root directory"}
					},
					["B"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory B"}
					},
					["C"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory C"}
					},
					["D"] = new Docs
					{
						NestedDocs = new Dictionary<string, Docs>
						{
							[""] = new Docs
							{
								Descriptions = new[] {"Descriptions for directory D1"}
							},
							["D1"] = new Docs
							{
								Descriptions = new[] {"Descriptions for directory D1"}
							},
						}
					}
				}
			};
			var json =
				"{\"version\":1,\"isRoot\":false,\"docs\":{\"\":[\"Descriptions for root directory\"],\"B\":[\"Descriptions for directory B\"],\"C\":[\"Descriptions for directory C\"],\"D\":{\"\":[\"Descriptions for directory D1\"],\"D1\":[\"Descriptions for directory D1\"]}}}";
			return (config, json);
		}
	}

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
				{@"C:\A\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{@"C:\A\B\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{@"C:\A\D\D1\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
			}, currentDir);
			addDirectories(fs);

			var finder = new FindVerbHandler(fs);

			// Act
			var result = await finder.Handle(new FindRequest(), CancellationToken.None);

			// Assert
			Assert.That(result.ConfigurationFiles, Is.EquivalentTo(expected));
		}


		[TestCase(@"C:\A\", new[] {@"C:\A\wtfd.json"})]
		[TestCase(@"C:\A\B\", new[] {@"C:\A\B\wtfd.json", @"C:\A\wtfd.json"})]
		[TestCase(@"C:\A\D\D1", new[] {@"C:\A\D\D1\wtfd.json", @"C:\A\wtfd.json"})]
		public async Task Should_Return_AllConfigFilesFound_UsingSpecifiedPath(string currentDir, string[] expected)
		{
			// Arrange

			var fs = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{@"C:\A\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{@"C:\A\B\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{@"C:\A\D\D1\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
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
				{@"C:\A\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
				{@"C:\A\D\wtfd.json", MockData.RootConfig().ToMockFileData()},
				{@"C:\A\D\D1\wtfd.json", MockData.EmptyConfig().ToMockFileData()},
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
	}
}