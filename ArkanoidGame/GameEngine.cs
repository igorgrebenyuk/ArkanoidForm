using System.Drawing;

namespace ArkanoidGame
{
    public class GameEngine
    {
        public int BallSpeedX { get; set; } = GameSettings.InitialSpeedX;
        public int BallSpeedY { get; set; } = GameSettings.InitialSpeedY;

        public int BallDamage { get; set; } = 1;

        public Point CalculateNewPosition(Point currentPos, Size ballSize, Size clientSize)
        {
            int nextX = currentPos.X + BallSpeedX;
            int nextY = currentPos.Y + BallSpeedY;

            if (nextX <= 0 || nextX + ballSize.Width >= clientSize.Width) BallSpeedX *= -1;
            if (nextY <= 0) BallSpeedY *= -1;

            return new Point(nextX, nextY);
        }

        public void HitPaddle(int ballCenterX, int paddleCenterX)
        {
            BallSpeedY *= -1;
            int offset = ballCenterX - paddleCenterX;
            BallSpeedX = offset / 15;
        }
    }
}