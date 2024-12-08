namespace AoCCore;

public readonly record struct Dot(int Down, int Right)
{
    public int D => Down;

    public int R => Right;

    /// <summary>
    /// Performs vector addition of two <see cref='Dot'/> objects.
    /// </summary>
    public static Dot operator +(Dot sz1, Dot sz2) => new(unchecked(sz1.Down + sz2.Down), unchecked(sz1.Right + sz2.Right));

    /// <summary>
    /// Contracts a <see cref='Dot'/> by another <see cref='Dot'/>
    /// </summary>
    public static Dot operator -(Dot sz1, Dot sz2) => new(unchecked(sz1.Down - sz2.Down), unchecked(sz1.Right - sz2.Right));

    /// <summary>
    /// Multiplies a <see cref="Dot"/> by an <see cref="int"/> producing <see cref="Dot"/>.
    /// </summary>
    /// <param name="left">Multiplier of type <see cref="int"/>.</param>
    /// <param name="right">Multiplicand of type <see cref="Dot"/>.</param>
    /// <returns>Product of type <see cref="Dot"/>.</returns>
    public static Dot operator *(int left, Dot right) => right * left;

    /// <summary>
    /// Multiplies <see cref="Dot"/> by an <see cref="int"/> producing <see cref="Dot"/>.
    /// </summary>
    /// <param name="left">Multiplicand of type <see cref="Dot"/>.</param>
    /// <param name="right">Multiplier of type <see cref="int"/>.</param>
    /// <returns>Product of type <see cref="Dot"/>.</returns>
    public static Dot operator *(Dot left, int right) => new(unchecked(left.Down * right), unchecked(left.Right * right));

    public override string ToString()
    {
        return $"[D={D}, R={R}]";
    }
}