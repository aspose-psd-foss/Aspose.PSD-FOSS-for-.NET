namespace Aspose.PSD;

/// <summary>
/// Stores a set of four integers that represent the location and size of a rectangle.
/// </summary>
public struct Rectangle : IEquatable<Rectangle>
{
    /// <summary>
    /// Represents a <see cref="Rectangle"/> structure with its properties left uninitialized.
    /// </summary>
    public static readonly Rectangle Empty = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> structure with the specified location and size.
    /// </summary>
    /// <param name="location">The upper-left corner of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    public Rectangle(Point location, Size size)
        : this(location.X, location.Y, size.Width, size.Height)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> structure with the specified coordinates.
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
    /// Gets or sets the x-coordinate of the upper-left corner of this <see cref="Rectangle"/>.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the upper-left corner of this <see cref="Rectangle"/>.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Gets or sets the width of this <see cref="Rectangle"/>.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of this <see cref="Rectangle"/>.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the bottom edge of this <see cref="Rectangle"/>.
    /// </summary>
    public int Bottom
    {
        readonly get => Y + Height;
        set => Height = value - Y;
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="Rectangle"/> has no location or size.
    /// </summary>
    public readonly bool IsEmpty => X == 0 && Y == 0 && Width == 0 && Height == 0;

    /// <summary>
    /// Gets or sets the x-coordinate of the left edge of this <see cref="Rectangle"/>.
    /// </summary>
    public int Left
    {
        readonly get => X;
        set
        {
            int right = Right;
            X = value;
            Width = right - value;
        }
    }

    /// <summary>
    /// Gets or sets the coordinates of the upper-left corner of this <see cref="Rectangle"/>.
    /// </summary>
    public Point Location
    {
        readonly get => new(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    /// <summary>
    /// Gets or sets the x-coordinate of the right edge of this <see cref="Rectangle"/>.
    /// </summary>
    public int Right
    {
        readonly get => X + Width;
        set => Width = value - X;
    }

    /// <summary>
    /// Gets or sets the size of this <see cref="Rectangle"/>.
    /// </summary>
    public Size Size
    {
        readonly get => new(Width, Height);
        set
        {
            Width = value.Width;
            Height = value.Height;
        }
    }

    /// <summary>
    /// Gets or sets the y-coordinate of the top edge of this <see cref="Rectangle"/>.
    /// </summary>
    public int Top
    {
        readonly get => Y;
        set
        {
            int bottom = Bottom;
            Y = value;
            Height = bottom - value;
        }
    }

    /// <summary>
    /// Converts a <see cref="RectangleF"/> structure to a <see cref="Rectangle"/> structure by rounding up the coordinates and size.
    /// </summary>
    /// <param name="value">The rectangle to convert.</param>
    /// <returns>The converted rectangle.</returns>
    public static Rectangle Ceiling(RectangleF value)
    {
        return new Rectangle(
            (int)Math.Ceiling(value.X),
            (int)Math.Ceiling(value.Y),
            (int)Math.Ceiling(value.Width),
            (int)Math.Ceiling(value.Height));
    }

    /// <summary>
    /// Creates a rectangle from edge coordinates.
    /// </summary>
    /// <param name="left">The left edge.</param>
    /// <param name="top">The top edge.</param>
    /// <param name="right">The right edge.</param>
    /// <param name="bottom">The bottom edge.</param>
    /// <returns>The rectangle.</returns>
    public static Rectangle FromLeftTopRightBottom(int left, int top, int right, int bottom)
    {
        return new Rectangle(left, top, right - left, bottom - top);
    }

    /// <summary>
    /// Creates a rectangle from edge coordinates for internal PSD record parsing.
    /// </summary>
    /// <param name="left">The left edge.</param>
    /// <param name="top">The top edge.</param>
    /// <param name="right">The right edge.</param>
    /// <param name="bottom">The bottom edge.</param>
    /// <returns>The rectangle.</returns>
    internal static Rectangle FromLTRB(int left, int top, int right, int bottom)
    {
        return FromLeftTopRightBottom(left, top, right, bottom);
    }

    /// <summary>
    /// Creates a rectangle that spans the specified points.
    /// </summary>
    /// <param name="point1">The first point.</param>
    /// <param name="point2">The second point.</param>
    /// <returns>The rectangle that spans the two points.</returns>
    public static Rectangle FromPoints(Point point1, Point point2)
    {
        int left = Math.Min(point1.X, point2.X);
        int top = Math.Min(point1.Y, point2.Y);
        int right = Math.Max(point1.X, point2.X);
        int bottom = Math.Max(point1.Y, point2.Y);
        return FromLeftTopRightBottom(left, top, right, bottom);
    }

    /// <summary>
    /// Creates and returns an inflated copy of the specified rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle to inflate.</param>
    /// <param name="x">The amount to inflate horizontally.</param>
    /// <param name="y">The amount to inflate vertically.</param>
    /// <returns>The inflated rectangle.</returns>
    public static Rectangle Inflate(Rectangle rectangle, int x, int y)
    {
        Rectangle result = rectangle;
        result.Inflate(x, y);
        return result;
    }

    /// <summary>
    /// Returns a rectangle that represents the intersection of two rectangles.
    /// </summary>
    /// <param name="a">The first rectangle.</param>
    /// <param name="b">The second rectangle.</param>
    /// <returns>The intersection rectangle, or <see cref="Empty"/> when there is no overlap.</returns>
    public static Rectangle Intersect(Rectangle a, Rectangle b)
    {
        int left = Math.Max(a.Left, b.Left);
        int top = Math.Max(a.Top, b.Top);
        int right = Math.Min(a.Right, b.Right);
        int bottom = Math.Min(a.Bottom, b.Bottom);

        return right > left && bottom > top
            ? FromLeftTopRightBottom(left, top, right, bottom)
            : Empty;
    }

    /// <summary>
    /// Converts a <see cref="RectangleF"/> structure to a <see cref="Rectangle"/> structure by rounding the coordinates and size.
    /// </summary>
    /// <param name="value">The rectangle to convert.</param>
    /// <returns>The converted rectangle.</returns>
    public static Rectangle Round(RectangleF value)
    {
        return new Rectangle(
            (int)Math.Round(value.X),
            (int)Math.Round(value.Y),
            (int)Math.Round(value.Width),
            (int)Math.Round(value.Height));
    }

    /// <summary>
    /// Converts a <see cref="RectangleF"/> structure to a <see cref="Rectangle"/> structure by truncating the coordinates and size.
    /// </summary>
    /// <param name="value">The rectangle to convert.</param>
    /// <returns>The converted rectangle.</returns>
    public static Rectangle Truncate(RectangleF value)
    {
        return new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
    }

    /// <summary>
    /// Returns a rectangle that contains the union of two rectangles.
    /// </summary>
    /// <param name="a">The first rectangle.</param>
    /// <param name="b">The second rectangle.</param>
    /// <returns>The union rectangle.</returns>
    public static Rectangle Union(Rectangle a, Rectangle b)
    {
        int left = Math.Min(a.Left, b.Left);
        int top = Math.Min(a.Top, b.Top);
        int right = Math.Max(a.Right, b.Right);
        int bottom = Math.Max(a.Bottom, b.Bottom);
        return FromLeftTopRightBottom(left, top, right, bottom);
    }

    /// <summary>
    /// Determines whether the specified point is contained within this <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="point">The point to test.</param>
    /// <returns><see langword="true"/> if the point is contained in this rectangle; otherwise, <see langword="false"/>.</returns>
    public readonly bool Contains(Point point)
    {
        return Contains(point.X, point.Y);
    }

    /// <summary>
    /// Determines whether the rectangular region represented by the specified rectangle is entirely contained within this <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rectangle">The rectangle to test.</param>
    /// <returns><see langword="true"/> if the rectangle is contained in this rectangle; otherwise, <see langword="false"/>.</returns>
    public readonly bool Contains(Rectangle rectangle)
    {
        return Left <= rectangle.Left
            && rectangle.Right <= Right
            && Top <= rectangle.Top
            && rectangle.Bottom <= Bottom;
    }

    /// <summary>
    /// Determines whether the specified point is contained within this <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <returns><see langword="true"/> if the point is contained in this rectangle; otherwise, <see langword="false"/>.</returns>
    public readonly bool Contains(int x, int y)
    {
        return x >= Left && x < Right && y >= Top && y < Bottom;
    }

    /// <summary>
    /// Determines whether the specified rectangle is equal to this rectangle.
    /// </summary>
    /// <param name="other">The rectangle to compare.</param>
    /// <returns><see langword="true"/> if the rectangles are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Rectangle other)
    {
        return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
    }

    /// <summary>
    /// Determines whether the specified object is equal to this rectangle.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><see langword="true"/> if the object is an equal rectangle; otherwise, <see langword="false"/>.</returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is Rectangle other && Equals(other);
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
    /// Inflates this rectangle by the specified size.
    /// </summary>
    /// <param name="size">The amount to inflate.</param>
    public void Inflate(Size size)
    {
        Inflate(size.Width, size.Height);
    }

    /// <summary>
    /// Inflates this rectangle by the specified horizontal and vertical amounts.
    /// </summary>
    /// <param name="width">The amount to inflate horizontally.</param>
    /// <param name="height">The amount to inflate vertically.</param>
    public void Inflate(int width, int height)
    {
        X -= width;
        Y -= height;
        Width += width * 2;
        Height += height * 2;
    }

    /// <summary>
    /// Replaces this rectangle with its intersection with the specified rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle to intersect with this rectangle.</param>
    public void Intersect(Rectangle rectangle)
    {
        this = Intersect(this, rectangle);
    }

    /// <summary>
    /// Determines whether this rectangle intersects with the specified rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle to test.</param>
    /// <returns><see langword="true"/> if the rectangles intersect; otherwise, <see langword="false"/>.</returns>
    public readonly bool IntersectsWith(Rectangle rectangle)
    {
        return rectangle.Left < Right
            && Left < rectangle.Right
            && rectangle.Top < Bottom
            && Top < rectangle.Bottom;
    }

    /// <summary>
    /// Normalizes this rectangle so that width and height are non-negative.
    /// </summary>
    public void Normalize()
    {
        if (Width < 0)
        {
            X += Width;
            Width = -Width;
        }

        if (Height < 0)
        {
            Y += Height;
            Height = -Height;
        }
    }

    /// <summary>
    /// Moves this rectangle by the specified point.
    /// </summary>
    /// <param name="point">The offset to apply.</param>
    public void Offset(Point point)
    {
        Offset(point.X, point.Y);
    }

    /// <summary>
    /// Moves this rectangle by the specified horizontal and vertical amounts.
    /// </summary>
    /// <param name="x">The amount to move horizontally.</param>
    /// <param name="y">The amount to move vertically.</param>
    public void Offset(int x, int y)
    {
        X += x;
        Y += y;
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
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two rectangles are not equal.
    /// </summary>
    /// <param name="left">The first rectangle.</param>
    /// <param name="right">The second rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !left.Equals(right);
    }
}
