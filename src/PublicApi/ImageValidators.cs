using System;
using System.IO;

namespace Microsoft.eShopWeb.PublicApi;

public static class ImageValidators
{
    private const int ImageMaximumBytes = 512000;

    private static readonly byte[] JpegSignature = new byte[] { 0xFF, 0xD8, 0xFF };
    private static readonly byte[] PngSignature = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    private static readonly byte[] Gif87aSignature = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };
    private static readonly byte[] Gif89aSignature = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };

    public static bool IsValidImage(this byte[] postedFile, string fileName)
    {
        if (postedFile == null || postedFile.Length == 0 || postedFile.Length > ImageMaximumBytes)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }

        if (!IsExtensionValid(fileName))
        {
            return false;
        }

        var extension = Path.GetExtension(fileName);

        if (string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase))
        {
            return HasSignature(postedFile, JpegSignature);
        }

        if (string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase))
        {
            return HasSignature(postedFile, PngSignature);
        }

        if (string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase))
        {
            return HasSignature(postedFile, Gif87aSignature) || HasSignature(postedFile, Gif89aSignature);
        }

        return false;
    }

    private static bool IsExtensionValid(string fileName)
    {
        var extension = Path.GetExtension(fileName);

        return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasSignature(byte[] file, byte[] signature)
    {
        if (file.Length < signature.Length)
        {
            return false;
        }

        for (int i = 0; i < signature.Length; i++)
        {
            if (file[i] != signature[i])
            {
                return false;
            }
        }

        return true;
    }
}
