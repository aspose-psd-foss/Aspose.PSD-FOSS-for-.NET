namespace Aspose.PSD;

/// <summary>
/// Stores a set of four floating-point numbers that represent the location and size of a rectangle.
/// </summary>
public struct RectangleF : IEquatable<RectangleF>
{
    /// <summary>
    /// Represents a <see cref="RectangleF"/> structure with its properties left uninitialized.
    /// </summary>
    public static readonly RectangleF Empty = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleF"/> structure with the specified coordinates.
    /// </summary>
    /// <param name="x">The left coordinate.</param>
    /// <param name="y">The top coordinate.</param>
    /// <param name="width">The rectangle width.</param>
    /// <param name="height">The rectangle height.</param>
    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets or sets the x-coordinate of the upper-left corner of this <see cref="RectangleF"/>.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the upper-left corner of this <see cref="RectangleF"/>.
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// Gets or sets the width of this <see cref="RectangleF"/>.
    /// </summary>
    public float Width { get; set; }

    /// <summary>
    /// Gets or sets the height of this <see cref="RectangleF"/>.
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    /// Gets the y-coordinate of the bottom edge of this <see cref="RectangleF"/>.
    /// </summary>
    public readonly float Bottom => Y + Height;

    /// <summary>
    /// Gets a value indicating whether this <see cref="RectangleF"/> has no location or size.
    /// </summary>
    public readonly bool IsEmpty => X == 0 && Y == 0 && Width == 0 && Height == 0;

    /// <summary>
    /// Gets the x-coordinate of the left edge of this <see cref="RectangleF"/>.
    /// </summary>
    public readonly float Left => X;

    /// <summary>
    /// Gets the x-coordinate of the right edge of this <see cref="RectangleF"/>.
    /// </summary>
    public readonly float Right => X + Width;

    /// <summary>
    /// Gets the y-coordinate of the top edge of this <see cref="RectangleF"/>.
    /// </summary>
    public readonly float Top => Y;

    /// <summary>
    /// Determines whether the specified rectangle is equal to this rectangle.
    /// </summary>
    /// <param name="other">The rectangle to compare.</param>
    /// <returns><see langword="true"/> if the rectangles are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(RectangleF other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
    }

    /// <summary>
    /// Determines whether the specified object is equal to this rectangle.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><see langword="true"/> if the object is an equal rectangle; otherwise, <see langword="false"/>.</returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is RectangleF other && Equals(other);
    }

    /// <summary>
    /// Returns a hash code for this rectangle.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y, Width, Height);
    }

    /// <summary>
    /// Returns a compact coordinate representation.
    /// </summary>
    /// <returns>The coordinate representation.</returns>
    public override readonly string ToString()
    {
        return $"Left={Left}, Top={Top}, Right={Right}, Bottom={Bottom}, Width={Width}, Height={Height}";
    }

    /// <summary>
    /// Determines whether two rectangles are equal.
    /// </summary>
    /// <param name="left">The first rectangle.</param>
    /// <param name="right">The second rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(RectangleF left, RectangleF right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two rectangles are not equal.
    /// </summary>
    /// <param name="left">The first rectangle.</param>
    /// <param name="right">The second rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(RectangleF left, RectangleF right)
    {
        return !left.Equals(right);
    }
}
