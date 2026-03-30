using System.Drawing;

namespace Arkanoid.Logic;

/// <summary>
/// Представляет бонус (усилитель), выпадающий из блоков.
/// </summary>
public class Booster
{
    /// <summary> Границы и положение бустера. </summary>
    public Rectangle Bounds { get; set; }
}