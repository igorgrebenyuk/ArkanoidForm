using System.Drawing;

namespace Arkanoid.Logic
{
    /// <summary>
    /// Логический движок игры. Отвечает за расчеты перемещений и состояние игровых объектов.
    /// </summary>
    public class GameEngine
    {
        private readonly Random randomizer;

        /// <summary> Текущие координаты и размер мяча. </summary>
        public Rectangle Ball { get; private set; }

        /// <summary> Текущие координаты и размер ракетки. </summary>
        public Rectangle Paddle { get; private set; }

        /// <summary> Список активных блоков на поле. </summary>
        public List<Block> Blocks { get; private set; }

        /// <summary> Список падающих бонусов. </summary>
        public List<Booster> Boosters { get; private set; }

        /// <summary> Текущая скорость мяча по горизонтали. </summary>
        public int BallSpeedX { get; private set; } = GameConstants.InitialSpeedX;

        /// <summary> Текущая скорость мяча по вертикали. </summary>
        public int BallSpeedY { get; private set; } = GameConstants.InitialSpeedY;

        /// <summary> Текущий урон, наносимый мячом. </summary>
        public int BallDamage { get; private set; } = GameConstants.InitialBallDamage;

        /// <summary> Состояние игры: потерян ли мяч. </summary>
        public bool IsBallLost { get; private set; }

        public GameEngine()
        {
            randomizer = new Random();
            Blocks = new List<Block>();
            Boosters = new List<Booster>();
            ResetLevel();
        }

        /// <summary>
        /// Сбрасывает состояние уровня к начальным настройкам.
        /// </summary>
        public void ResetLevel()
        {
            Blocks.Clear();
            Boosters.Clear();
            IsBallLost = false;

            BallDamage = GameConstants.InitialBallDamage;
            BallSpeedX = GameConstants.InitialSpeedX;
            BallSpeedY = GameConstants.InitialSpeedY;

            Ball = new Rectangle(
                GameConstants.InitialBallX,
                GameConstants.InitialBallY,
                GameConstants.BallSize,
                GameConstants.BallSize);

            Paddle = new Rectangle(
                GameConstants.InitialPaddleX,
                GameConstants.InitialPaddleY,
                GameConstants.InitialPaddleWidth,
                GameConstants.InitialPaddleHeight);

            for (var rowNumber = 0; rowNumber < GameConstants.Rows; rowNumber++)
            {
                var healthLevel = GameConstants.Rows - rowNumber;

                for (var colNumber = 0; colNumber < GameConstants.Columns; colNumber++)
                {
                    var positionX = GameConstants.BlockLeftOffset + colNumber * (GameConstants.BlockWidth + GameConstants.BlockSpacing);
                    var positionY = GameConstants.BlockTopOffset + rowNumber * (GameConstants.BlockHeight + GameConstants.BlockSpacing);

                    Blocks.Add(new Block { Bounds = new Rectangle(positionX, positionY, GameConstants.BlockWidth, GameConstants.BlockHeight), Health = healthLevel });
                }
            }
        }

        /// <summary>
        /// Обрабатывает перемещение ракетки за курсором мыши.
        /// </summary>
        public void MovePaddle(int mouseX, int clientWidth)
        {
            var newPositionX = mouseX - Paddle.Width / 2;

            if (newPositionX < 0) newPositionX = 0;
            if (newPositionX > clientWidth - Paddle.Width) newPositionX = clientWidth - Paddle.Width;

            Paddle = new Rectangle(newPositionX, Paddle.Y, Paddle.Width, Paddle.Height);
        }

        /// <summary>
        /// Основной цикл физики: движение мяча, отскоки, столкновения и выпадение бонусов.
        /// </summary>
        public void UpdatePhysics(int clientWidth, int clientHeight)
        {
            if (IsBallLost) return;

            var nextPositionX = Ball.X + BallSpeedX;
            var nextPositionY = Ball.Y + BallSpeedY;

            if (nextPositionX <= 0 || nextPositionX + Ball.Width >= clientWidth) BallSpeedX *= -1;
            if (nextPositionY <= 0) BallSpeedY *= -1;

            if (nextPositionY > clientHeight)
            {
                IsBallLost = true;
                return;
            }

            Ball = new Rectangle(Ball.X + BallSpeedX, Ball.Y + BallSpeedY, Ball.Width, Ball.Height);

            if (Ball.IntersectsWith(Paddle) && BallSpeedY > 0)
            {
                BounceY();
                var offsetFromCenter = (Ball.X + Ball.Width / 2) - (Paddle.X + Paddle.Width / 2);
                BallSpeedX = offsetFromCenter / GameConstants.PaddleHitDivisor;
            }

            foreach (var block in Blocks.ToList())
            {
                if (Ball.IntersectsWith(block.Bounds))
                {
                    BounceY();
                    block.Health -= BallDamage;

                    if (block.Health <= 0)
                    {
                        TryDropBooster(block.Bounds.Location);
                        Blocks.Remove(block);
                    }
                    break;
                }
            }

            foreach (var booster in Boosters.ToList())
            {
                booster.Bounds = new Rectangle(booster.Bounds.X, booster.Bounds.Y + GameConstants.BoosterSpeed, booster.Bounds.Width, booster.Bounds.Height);

                if (booster.Bounds.IntersectsWith(Paddle))
                {
                    IncreaseDamage();
                    Boosters.Remove(booster);
                }
                else if (booster.Bounds.Y > clientHeight)
                {
                    Boosters.Remove(booster);
                }
            }
        }

        private void BounceY() => BallSpeedY *= -1;

        private void IncreaseDamage()
        {
            if (BallDamage < GameConstants.MaxBallDamage) BallDamage++;
        }

        private void TryDropBooster(Point dropLocation)
        {
            if (randomizer.Next(GameConstants.RandomRangeMin, GameConstants.RandomRangeMax) <= GameConstants.BoosterChance)
            {
                Boosters.Add(new Booster
                {
                    Bounds = new Rectangle(dropLocation.X, dropLocation.Y, GameConstants.BoosterSize, GameConstants.BoosterSize)
                });
            }
        }
    }
}