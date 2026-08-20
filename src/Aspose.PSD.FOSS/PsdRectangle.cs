namespace Aspose.PSD.FOSS;

/// <summary>
/// Represents a rectangle in PSD document coordinates without depending on a rendering-oriented geometry type.
/// </summary>
public readonly record struct PsdRectangle
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PsdRectangle"/> struct.
    /// </summary>
    /// <param name="left">The left edge in document coordinates.</param>
    /// <param name="top">The top edge in document coordinates.</param>
    /// <param name="right">The right edge in document coordinates.</param>
    /// <param name="bottom">The bottom edge in document coordinates.</param>
    public PsdRectangle(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>
    /// Gets the left edge in PSD document coordinates.
    /// </summary>
    public int Left { get; }

    /// <summary>
    /// Gets the top edge in PSD document coordinates.
    /// </summary>
    public int Top { get; }

    /// <summary>
    /// Gets the right edge in PSD document coordinates.
    /// </summary>
    public int Right { get; }

    /// <summary>
    /// Gets the bottom edge in PSD document coordinates.
    /// </summary>
    public int Bottom { get; }

    /// <summary>
    /// Gets the rectangle width in pixels.
    /// </summary>
    public int Width => Right - Left;

    /// <summary>
    /// Gets the rectangle height in pixels.
    /// </summary>
    public int Height => Bottom - Top;

    /// <summary>
    /// Creates a rectangle from PSD edge coordinates.
    /// </summary>
    /// <param name="left">The left edge in document coordinates.</param>
    /// <param name="top">The top edge in document coordinates.</param>
    /// <param name="right">The right edge in document coordinates.</param>
    /// <param name="bottom">The bottom edge in document coordinates.</param>
    /// <returns>The rectangle.</returns>
    public static PsdRectangle FromLTRB(int left, int top, int right, int bottom)
    {
        return new PsdRectangle(left, top, right, bottom);
    }

    /// <summary>
    /// Returns a compact coordinate representation for diagnostics and samples.
    /// </summary>
    /// <returns>The coordinate representation.</returns>
    public override string ToString()
    {
        return $"Left={Left}, Top={Top}, Right={Right}, Bottom={Bottom}, Width={Width}, Height={Height}";
    }
}
