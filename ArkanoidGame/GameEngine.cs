using System.Drawing;

namespace ArkanoidGame
{
    /// <summary>
    /// Логический движок игры. Отвечает за расчеты перемещений и состояние игровых объектов.
    /// </summary>
    public class GameEngine
    {
        /// <summary> Текущая скорость мяча по оси X. </summary>
        public int BallSpeedX { get; set; } = GameSettings.InitialSpeedX;

        /// <summary> Текущая скорость мяча по оси Y. </summary>
        public int BallSpeedY { get; set; } = GameSettings.InitialSpeedY;

        /// <summary> Количество урона, которое наносит мяч при ударе о блок. </summary>
        public int BallDamage { get; set; } = GameSettings.InitialBallDamage;

        /// <summary>
        /// Рассчитывает новую позицию мяча с учетом отскоков от границ окна.
        /// </summary>
        public Point CalculateNewPosition(Point currentPosition, Size ballSize, Size clientWindowSize)
        {
            int nextPositionX = currentPosition.X + BallSpeedX;
            int nextPositionY = currentPosition.Y + BallSpeedY;

            if (nextPositionX <= 0 || nextPositionX + ballSize.Width >= clientWindowSize.Width)
                BallSpeedX *= -1;

            if (nextPositionY <= 0)
                BallSpeedY *= -1;

            return new Point(nextPositionX, nextPositionY);
        }

        /// <summary>
        /// Вычисляет физику отскока мяча при попадании в ракетку.
        /// </summary>
        public void HitPaddle(int ballCenterX, int paddleCenterX)
        {
            BallSpeedY *= -1;
            int offsetFromCenter = ballCenterX - paddleCenterX;
            BallSpeedX = offsetFromCenter / 15;
        }
    }
}