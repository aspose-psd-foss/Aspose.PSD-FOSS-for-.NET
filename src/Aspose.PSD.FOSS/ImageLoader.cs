using Aspose.PSD.FileFormats.Psd;
using System.IO;

namespace Aspose.PSD
{
    /// <summary>
    /// Provides the Load functionality for Images
    /// </summary>
    public abstract class ImageLoader
{
    /// <summary>
    /// Loads a new image from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path to load image from.</param>
    /// <returns>The loaded image.</returns>
    public static object Load(string filePath)
    {
        return PsdImage.Load(filePath);
    }

    /// <summary>
    /// Loads a new image from the specified stream.
    /// </summary>
    /// <param name="stream">The stream to load image from.</param>
    /// <returns>The loaded image.</returns>
    public static object Load(Stream stream)
    {
        return PsdImage.Load(stream);
    }
}
}
