namespace Aspose.PSD;

/// <summary>
/// Represents an ordered pair of integer width and height values that defines a size.
/// </summary>
public struct Size : IEquatable<Size>
{
    /// <summary>
    /// Represents a <see cref="Size"/> with width and height left uninitialized.
    /// </summary>
    public static readonly Size Empty = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Size"/> structure with the specified dimensions.
    /// </summary>
    /// <param name="width">The width component.</param>
    /// <param name="height">The height component.</param>
    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets or sets the width component of this <see cref="Size"/>.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height component of this <see cref="Size"/>.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="Size"/> has width and height left uninitialized.
    /// </summary>
    public readonly bool IsEmpty => Width == 0 && Height == 0;

    /// <summary>
    /// Determines whether the specified size is equal to this size.
    /// </summary>
    /// <param name="other">The size to compare.</param>
    /// <returns><see langword="true"/> if the sizes are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Size other)
    {
        return Width == other.Width && Height == other.Height;
    }

    /// <summary>
    /// Determines whether the specified object is equal to this size.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><see langword="true"/> if the object is an equal size; otherwise, <see langword="false"/>.</returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is Size other && Equals(other);
    }

    /// <summary>
    /// Returns a hash code for this size.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Width, Height);
    }

    /// <summary>
    /// Returns a compact size representation.
    /// </summary>
    /// <returns>The size representation.</returns>
    public override readonly string ToString()
    {
        return $"Width={Width}, Height={Height}";
    }

    /// <summary>
    /// Determines whether two sizes are equal.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns><see langword="true"/> if the sizes are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Size left, Size right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two sizes are not equal.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns><see langword="true"/> if the sizes are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Size left, Size right)
    {
        return !left.Equals(right);
    }
}
