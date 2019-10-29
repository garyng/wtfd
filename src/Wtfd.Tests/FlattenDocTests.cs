using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Wtfd.Commands.Find;

namespace Wtfd.Tests
{
	public class FlattenDocTests
	{
		[Test]
		public void Should_EndPathWithSeparator()
		{
			// Arrange
			var (config, _) = MockData.FlatConfig();

			// Act
			var flatten = FlattenDoc.FromDictionary(config.Docs);


			// Assert
			flatten.Should().BeEquivalentTo(new[]
			{
				new FlattenDoc
				{
					Pattern = "",
					Descriptions = new[] {"Descriptions for root directory"}
				},
				new FlattenDoc
				{
					Pattern = "B/",
					Descriptions = new[] {"Descriptions for directory B"}
				},
				new FlattenDoc
				{
					Pattern = "C/",
					Descriptions = new[] {"Descriptions for directory C"}}
			});
		}

		[Test]
		public void Should_FlattenNestedConfig()
		{
			// Arrange
			var (config, _) = MockData.NestedConfig();

			// Act
			var flatten = FlattenDoc.FromDictionary(config.Docs);

			// Assert
			flatten.Should().BeEquivalentTo(new[]
			{
				new FlattenDoc
				{
					Pattern = "",
					Descriptions = new[] {"Descriptions for root directory"}
				},
				new FlattenDoc
				{
					Pattern = "B/",
					Descriptions = new[] {"Descriptions for directory B"}
				},
				new FlattenDoc
				{
					Pattern = "C/",
					Descriptions = new[] {"Descriptions for directory C"}
				},
				new FlattenDoc
				{
					Pattern = "D/",
					Descriptions = new[] {"Descriptions for directory D"}
				},
				new FlattenDoc
				{
					Pattern = "D/D1/",
					Descriptions = new[] {"Descriptions for directory D1"}
				}
			});
		}
	}
}