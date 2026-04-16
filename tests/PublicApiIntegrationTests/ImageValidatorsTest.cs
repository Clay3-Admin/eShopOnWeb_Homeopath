using Microsoft.eShopWeb.PublicApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PublicApiIntegrationTests;

[TestClass]
public class ImageValidatorsTest
{
    [DataTestMethod]
    [DataRow("photo.jpg")]
    [DataRow("photo.jpeg")]
    public void IsValidImageReturnsTrueGivenJpegSignatureAndJpegExtension(string fileName)
    {
        var image = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10 };

        Assert.IsTrue(image.IsValidImage(fileName));
    }

    [TestMethod]
    public void IsValidImageReturnsTrueGivenPngSignatureAndPngExtension()
    {
        var image = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00 };

        Assert.IsTrue(image.IsValidImage("photo.png"));
    }

    [DataTestMethod]
    [DataRow(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61, 0x00 })]
    [DataRow(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x00 })]
    public void IsValidImageReturnsTrueGivenGifSignatureAndGifExtension(byte[] image)
    {
        Assert.IsTrue(image.IsValidImage("photo.gif"));
    }

    [TestMethod]
    public void IsValidImageReturnsFalseGivenAllowedExtensionAndWrongSignature()
    {
        var image = new byte[] { 0x25, 0x50, 0x44, 0x46 };

        Assert.IsFalse(image.IsValidImage("photo.png"));
    }

    [TestMethod]
    public void IsValidImageReturnsFalseGivenSignatureAndMismatchedExtension()
    {
        var image = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        Assert.IsFalse(image.IsValidImage("photo.jpg"));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void IsValidImageReturnsFalseGivenMissingFileName(string? fileName)
    {
        var image = new byte[] { 0xFF, 0xD8, 0xFF };

        Assert.IsFalse(image.IsValidImage(fileName!));
    }

    [TestMethod]
    public void IsValidImageReturnsFalseGivenTruncatedSignature()
    {
        var image = new byte[] { 0x89, 0x50, 0x4E };

        Assert.IsFalse(image.IsValidImage("photo.png"));
    }
}
