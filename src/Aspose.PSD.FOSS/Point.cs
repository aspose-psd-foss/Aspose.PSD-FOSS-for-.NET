namespace Aspose.PSD;

/// <summary>
/// Represents an ordered pair of integer x- and y-coordinates that defines a point in a two-dimensional plane.
/// </summary>
public struct Point : IEquatable<Point>
{
    /// <summary>
    /// Represents a <see cref="Point"/> with coordinates left uninitialized.
    /// </summary>
    public static readonly Point Empty = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> structure with the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets or sets the x-coordinate of this <see cref="Point"/>.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of this <see cref="Point"/>.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="Point"/> has coordinates left uninitialized.
    /// </summary>
    public readonly bool IsEmpty => X == 0 && Y == 0;

    /// <summary>
    /// Determines whether the specified point is equal to this point.
    /// </summary>
    /// <param name="other">The point to compare.</param>
    /// <returns><see langword="true"/> if the points are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    /// <summary>
    /// Determines whether the specified object is equal to this point.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><see langword="true"/> if the object is an equal point; otherwise, <see langword="false"/>.</returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is Point other && Equals(other);
    }

    /// <summary>
    /// Returns a hash code for this point.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    /// <summary>
    /// Returns a compact coordinate representation.
    /// </summary>
    /// <returns>The coordinate representation.</returns>
    public override readonly string ToString()
    {
        return $"X={X}, Y={Y}";
    }

    /// <summary>
    /// Determines whether two points are equal.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns><see langword="true"/> if the points are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two points are not equal.
    /// </summary>
    /// <param name="left">The first point.</param>
    /// <param name="right">The second point.</param>
    /// <returns><see langword="true"/> if the points are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Point left, Point right)
    {
        return !left.Equals(right);
    }
}
