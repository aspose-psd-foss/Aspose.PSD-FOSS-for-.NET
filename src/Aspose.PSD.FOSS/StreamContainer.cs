using System.IO;

namespace Aspose.PSD;

/// <summary>
/// Represents a stream container used by resource save APIs.
/// </summary>
public class StreamContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamContainer"/> class.
    /// </summary>
    /// <param name="stream">The wrapped stream.</param>
    public StreamContainer(Stream stream)
    {
        Stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    /// <summary>
    /// Gets the wrapped stream.
    /// </summary>
    public Stream Stream { get; }
}
