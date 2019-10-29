using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Wtfd.Models;

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
					["B/"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory B"}
					},
					["C/"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory C"}
					}
				}
			};
			var json =
				"{\"version\":1,\"isRoot\":false,\"docs\":{\"\":[\"Descriptions for root directory\"],\"B/\":[\"Descriptions for directory B\"],\"C/\":[\"Descriptions for directory C\"]}}";
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
					["B/"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory B"}
					},
					["C/"] = new Docs
					{
						Descriptions = new[] {"Descriptions for directory C"}
					},
					["D/"] = new Docs
					{
						NestedDocs = new Dictionary<string, Docs>
						{
							[""] = new Docs
							{
								Descriptions = new[] {"Descriptions for directory D"}
							},
							["D1/"] = new Docs
							{
								Descriptions = new[] {"Descriptions for directory D1"}
							},
						}
					}
				}
			};
			var json =
				"{\"version\":1,\"isRoot\":false,\"docs\":{\"\":[\"Descriptions for root directory\"],\"B/\":[\"Descriptions for directory B\"],\"C/\":[\"Descriptions for directory C\"],\"D/\":{\"\":[\"Descriptions for directory D\"],\"D1/\":[\"Descriptions for directory D1\"]}}}";
			return (config, json);
		}

		public static void AddDirectories(MockFileSystem fs)
		{
			fs.AddDirectory("C:/A/");
			fs.AddDirectory("C:/A/B/");
			fs.AddDirectory("C:/A/C/");
			fs.AddDirectory("C:/A/D/");
			fs.AddDirectory("C:/A/D/D1/");
			fs.AddDirectory("C:/A/D/D2/");
		}
	}
}