using Aspose.PSD.FileFormats.Psd;

namespace Aspose.PSD;

/// <summary>
/// Provides the Aspose.PSD-compatible base image entry point for loading PSD/PSB documents.
/// </summary>
public abstract class Image : IDisposable
{
    /// <summary>
    /// Gets the image width in pixels.
    /// </summary>
    public abstract int Width { get; }

    /// <summary>
    /// Gets the image height in pixels.
    /// </summary>
    public abstract int Height { get; }

    /// <summary>
    /// Gets the image bounds.
    /// </summary>
    public Rectangle Bounds => new(0, 0, Width, Height);

    /// <summary>
    /// Loads a new image from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path to load image from.</param>
    /// <returns>The loaded image.</returns>
    public static Image Load(string filePath)
    {
        return PsdImage.Load(filePath);
    }

    /// <summary>
    /// Loads a new image from the specified stream.
    /// </summary>
    /// <param name="stream">The stream to load image from.</param>
    /// <returns>The loaded image.</returns>
    public static Image Load(Stream stream)
    {
        return PsdImage.Load(stream);
    }

    /// <summary>
    /// Saves the image data to the specified file path.
    /// </summary>
    /// <param name="filePath">The destination file path.</param>
    public abstract void Save(string filePath);

    /// <summary>
    /// Saves the image data to the specified stream.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    public abstract void Save(Stream stream);

    /// <summary>
    /// Releases resources used by the image.
    /// </summary>
    public abstract void Dispose();
}
