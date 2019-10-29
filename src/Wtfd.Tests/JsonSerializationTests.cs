using NUnit.Framework;
using Wtfd.Models;

namespace Wtfd.Tests
{
	public class JsonSerializationTests
	{
		[Test]
		public void Should_SerializeFlatConfig()
		{
			// Arrange
			var (config, json) = MockData.FlatConfig();

			// Act
			var result = Json.Serialize(config);
			
			// round trip
			var result2 = Json.Serialize(Json.Deserialize<Configuration>(json));


			// Assert
			Assert.That(result, Is.EqualTo(json));
			Assert.That(result2, Is.EqualTo(json));
		}

		[Test]
		public void Should_SerializeNestedConfig()
		{
			// Arrange
			var (config, json) = MockData.NestedConfig();

			// Act
			var result = Json.Serialize(config);

			// round trip
			var result2 = Json.Serialize(Json.Deserialize<Configuration>(json));


			// Assert
			Assert.That(result, Is.EqualTo(json));
			Assert.That(result2, Is.EqualTo(json));
		}
	}
}