using System;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using TextEncoder;

namespace TextEncoderTests;

[TestFixture]
public class SerializationTests
{
    // ReSharper disable once MemberCanBePrivate.Global
    // only public types can be serialized by XmlSerializer
    public class DataContainer
    {
        public Base32? Encoded32 { get; set; }
        public Base64? Encoded64 { get; set; }
    }

    [Test]
    public void GetSchema_ShouldReturnNull()
    {
        // Arrange
        byte[] sourceData = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"

        Base32 base32Value = new Base32(sourceData, Base32Format.RFC4648);
        Base64 base64Value = new Base64(sourceData, Base64Format.XmlEncoding);

        // Act
        var schema32 = base32Value.GetSchema();
        var schema64 = base64Value.GetSchema();

        // Assert
        Assert.That(schema32, Is.Null);
        Assert.That(schema64, Is.Null);
    }

    [Test]
    public void XmlSerialization_ShouldWorkInRoundtrip()
    {
        // Arrange
        byte[] sourceData = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"

        Base32 base32Value = new Base32(sourceData, Base32Format.RFC4648);
        Base64 base64Value = new Base64(sourceData, Base64Format.XmlEncoding);

        DataContainer originalContainer = new DataContainer
        {
            Encoded32 = base32Value,
            Encoded64 = base64Value
        };

        // Act
        XmlSerializer serializer = new XmlSerializer(typeof(DataContainer));
        string xmlOutput;

        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, originalContainer);
            xmlOutput = writer.ToString();
        }

        Console.WriteLine(xmlOutput);

        DataContainer? deserializedContainer;
        using (StringReader reader = new StringReader(xmlOutput))
        {
            deserializedContainer = serializer.Deserialize(reader) as DataContainer;
        }

        // Assert
        Assert.That(deserializedContainer, Is.Not.Null);

        Base32? result32 = deserializedContainer.Encoded32;
        Base64? result64 = deserializedContainer.Encoded64;

        Assert.That(result32, Is.EqualTo(base32Value));
        Assert.That(result64, Is.EqualTo(base64Value));

        Assert.That(result32?.Raw, Is.EqualTo(sourceData).AsCollection);
        Assert.That(result64?.Raw, Is.EqualTo(sourceData).AsCollection);
    }
}