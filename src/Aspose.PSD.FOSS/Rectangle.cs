namespace Aspose.PSD;

/// <summary>
/// Represents a rectangle using Aspose.PSD-compatible X, Y, Width, and Height coordinates.
/// </summary>
public readonly record struct Rectangle
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="x">The left coordinate.</param>
    /// <param name="y">The top coordinate.</param>
    /// <param name="width">The rectangle width.</param>
    /// <param name="height">The rectangle height.</param>
    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the left coordinate.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the top coordinate.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the rectangle width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the rectangle height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the left edge.
    /// </summary>
    public int Left => X;

    /// <summary>
    /// Gets the top edge.
    /// </summary>
    public int Top => Y;

    /// <summary>
    /// Gets the right edge.
    /// </summary>
    public int Right => X + Width;

    /// <summary>
    /// Gets the bottom edge.
    /// </summary>
    public int Bottom => Y + Height;

    /// <summary>
    /// Creates a rectangle from edge coordinates.
    /// </summary>
    /// <param name="left">The left edge.</param>
    /// <param name="top">The top edge.</param>
    /// <param name="right">The right edge.</param>
    /// <param name="bottom">The bottom edge.</param>
    /// <returns>The rectangle.</returns>
    public static Rectangle FromLTRB(int left, int top, int right, int bottom)
    {
        return new Rectangle(left, top, right - left, bottom - top);
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
