using System.Drawing;

namespace Arkanoid.Logic;

/// <summary>
/// Представляет разрушаемый блок на игровом поле.
/// </summary>
public class Block
{
    /// <summary> Границы и положение блока. </summary>
    public Rectangle Bounds { get; set; }

    /// <summary> Текущее количество очков прочности блока. </summary>
    public int Health { get; set; }
}