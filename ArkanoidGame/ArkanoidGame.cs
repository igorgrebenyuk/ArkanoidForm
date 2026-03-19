using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    /// <summary>
    /// Главное окно игры Arkanoid. Управляет интерфейсом и игровым процессом.
    /// </summary>
    public partial class ArkanoidGame : Form
    {
        private readonly GameEngine engine;
        private readonly List<PictureBox> blocks;
        private readonly List<PictureBox> boosters;
        private readonly Random randomizer;

        /// <summary>
        /// Инициализирует новый экземпляр класса ArkanoidGame и настраивает компоненты.
        /// </summary>
        public ArkanoidGame()
        {
            InitializeComponent();

            engine = new GameEngine();
            blocks = new();
            boosters = new();
            randomizer = new Random();

            GenerateBlocks();
            UpdateWindowText(); 

            gameTimer.Tick += GameTimer_Tick;
            this.MouseMove += ArkanoidGame_MouseMove; 
        }

        private void GenerateBlocks()
        {
            for (int rowNumber = 0; rowNumber < GameSettings.Rows; rowNumber++)
            {
                int healthLevel = GameSettings.Rows - rowNumber;

                for (int colNumber = 0; colNumber < GameSettings.Columns; colNumber++)
                {
                    PictureBox block = new()
                    {
                        Size = new Size(GameSettings.BlockWidth, GameSettings.BlockHeight),
                        Left = GameSettings.BlockLeftOffset + colNumber * (GameSettings.BlockWidth + GameSettings.BlockSpacing),
                        Top = GameSettings.BlockTopOffset + rowNumber * (GameSettings.BlockHeight + GameSettings.BlockSpacing),
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = healthLevel,
                        BackColor = GetBlockColor(healthLevel)
                    };

                    this.Controls.Add(block);
                    blocks.Add(block);
                }
            }
        }

        private static Color GetBlockColor(int healthPoints) => healthPoints switch
        {
            GameSettings.MaxBlockHealth => Color.Purple,
            GameSettings.HighHealth => Color.Red,
            GameSettings.MediumHealth => Color.Orange,
            GameSettings.LowHealth => Color.Yellow,
            _ => Color.Green
        };

        private void UpdateWindowText()
        {
            this.Text = $"ArkanoidGame | Урон мяча: {engine.BallDamage}";
        }

        private void ArkanoidGame_MouseMove(object sender, MouseEventArgs mouseEvent)
        {
            int newPaddlePositionX = mouseEvent.X - pbPaddle.Width / 2;

            if (newPaddlePositionX < 0)
                newPaddlePositionX = 0;

            if (newPaddlePositionX > this.ClientSize.Width - pbPaddle.Width)
                newPaddlePositionX = this.ClientSize.Width - pbPaddle.Width;

            pbPaddle.Left = newPaddlePositionX;
        }

        private void GameTimer_Tick(object sender, EventArgs timerEvent)
        {
            pbBall.Location = engine.CalculateNewPosition(pbBall.Location, pbBall.Size, this.ClientSize);

            if (pbBall.Bounds.IntersectsWith(pbPaddle.Bounds) && engine.BallSpeedY > 0)
            {
                engine.HitPaddle(pbBall.Left + pbBall.Width / 2, pbPaddle.Left + pbPaddle.Width / 2);
            }

            for (int blockIndex = blocks.Count - 1; blockIndex >= 0; blockIndex--)
            {
                if (pbBall.Bounds.IntersectsWith(blocks[blockIndex].Bounds))
                {
                    engine.BallSpeedY *= -1;
                    int currentHealth = (int)blocks[blockIndex].Tag;
                    currentHealth -= engine.BallDamage;

                    if (currentHealth <= 0)
                    {
                        TryDropBooster(blocks[blockIndex].Location);
                        this.Controls.Remove(blocks[blockIndex]);
                        blocks.RemoveAt(blockIndex);
                    }
                    else
                    {
                        blocks[blockIndex].Tag = currentHealth;
                        blocks[blockIndex].BackColor = GetBlockColor(currentHealth);
                    }
                    break;
                }
            }

            ProcessFallingBoosters();

            if (pbBall.Top > this.ClientSize.Height)
            {
                RestartGame();
            }
        }

        private void TryDropBooster(Point dropLocation)
        {
            if (randomizer.Next(1, GameSettings.RandomRangeMax) <= GameSettings.BoosterChance)
            {
                PictureBox booster = new()
                {
                    Size = GameSettings.BoosterSize,
                    Location = dropLocation,
                    BackColor = Color.Pink,
                    BorderStyle = BorderStyle.Fixed3D
                };
                this.Controls.Add(booster);
                boosters.Add(booster);
            }
        }

        private void ProcessFallingBoosters()
        {
            for (int boosterIndex = boosters.Count - 1; boosterIndex >= 0; boosterIndex--)
            {
                boosters[boosterIndex].Top += GameSettings.BoosterSpeed;

                if (boosters[boosterIndex].Bounds.IntersectsWith(pbPaddle.Bounds))
                {
                    if (engine.BallDamage < GameSettings.MaxBallDamage)
                    {
                        engine.BallDamage++;
                        UpdateWindowText();
                    }
                    RemoveBooster(boosterIndex);
                }
                else if (boosters[boosterIndex].Top > this.ClientSize.Height)
                {
                    RemoveBooster(boosterIndex);
                }
            }
        }

        private void RemoveBooster(int indexToRemove)
        {
            this.Controls.Remove(boosters[indexToRemove]);
            boosters.RemoveAt(indexToRemove);
        }

        private void RestartGame()
        {
            gameTimer.Stop();
            MessageBox.Show("Мяч упал! Начинаем заново.");

            foreach (var block in blocks) this.Controls.Remove(block);
            foreach (var booster in boosters) this.Controls.Remove(booster);

            blocks.Clear();
            boosters.Clear();

            engine.BallDamage = GameSettings.InitialBallDamage;
            engine.BallSpeedX = GameSettings.InitialSpeedX;
            engine.BallSpeedY = GameSettings.InitialSpeedY;

            pbBall.Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

            GenerateBlocks();
            UpdateWindowText();
            gameTimer.Start();
        }
    }
}