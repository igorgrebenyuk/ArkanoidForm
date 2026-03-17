using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class Form1 : Form
    {
        private GameEngine engine;
        private List<PictureBox> blocks;
        private List<PictureBox> boosters;
        private Random randomizer;

        public Form1()
        {
            InitializeComponent();
            engine = new GameEngine();
            blocks = new List<PictureBox>();
            boosters = new List<PictureBox>();
            randomizer = new Random();

            GenerateBlocks();

            gameTimer.Tick += GameTimer_Tick;
            this.MouseMove += Form1_MouseMove;
        }

        private void GenerateBlocks()
        {
            for (int row = 0; row < GameSettings.Rows; row++)
            {
                int health = GameSettings.Rows - row;

                for (int col = 0; col < GameSettings.Columns; col++)
                {
                    PictureBox block = new PictureBox
                    {
                        Size = new Size(GameSettings.BlockWidth, GameSettings.BlockHeight),
                        Left = 30 + col * (GameSettings.BlockWidth + GameSettings.BlockSpacing),
                        Top = 50 + row * (GameSettings.BlockHeight + GameSettings.BlockSpacing),
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = health,
                        BackColor = GetBlockColor(health)
                    };

                    this.Controls.Add(block);
                    blocks.Add(block);
                }
            }
        }

        private Color GetBlockColor(int hp) => hp switch
        {
            5 => Color.Purple,
            4 => Color.Red,
            3 => Color.Orange,
            2 => Color.Yellow,
            _ => Color.Green
        };

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int newX = e.X - pbPaddle.Width / 2;
            if (newX < 0) newX = 0;
            if (newX > this.ClientSize.Width - pbPaddle.Width) newX = this.ClientSize.Width - pbPaddle.Width;
            pbPaddle.Left = newX;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            pbBall.Location = engine.CalculateNewPosition(pbBall.Location, pbBall.Size, this.ClientSize);

            if (pbBall.Bounds.IntersectsWith(pbPaddle.Bounds) && engine.BallSpeedY > 0)
            {
                engine.HitPaddle(pbBall.Left + pbBall.Width / 2, pbPaddle.Left + pbPaddle.Width / 2);
            }

            for (int i = blocks.Count - 1; i >= 0; i--)
            {
                if (pbBall.Bounds.IntersectsWith(blocks[i].Bounds))
                {
                    engine.BallSpeedY *= -1;
                    int currentHp = (int)blocks[i].Tag;

                    currentHp -= engine.BallDamage;

                    if (currentHp <= 0)
                    {
                        TryDropBooster(blocks[i].Location);
                        this.Controls.Remove(blocks[i]);
                        blocks.RemoveAt(i);
                    }
                    else
                    {
                        blocks[i].Tag = currentHp;
                        blocks[i].BackColor = GetBlockColor(currentHp);
                    }
                    break;
                }
            }

            HandleBoosters();

            if (pbBall.Top > this.ClientSize.Height) ResetGame();
        }

        private void TryDropBooster(Point location)
        {
            if (randomizer.Next(1, 101) <= GameSettings.BoosterChance)
            {
                PictureBox booster = new PictureBox
                {
                    Size = new Size(20, 20),
                    Location = location,
                    BackColor = Color.Pink,
                    BorderStyle = BorderStyle.Fixed3D
                };
                this.Controls.Add(booster);
                boosters.Add(booster);
            }
        }

        private void HandleBoosters()
        {
            for (int i = boosters.Count - 1; i >= 0; i--)
            {
                boosters[i].Top += GameSettings.BoosterSpeed;

                if (boosters[i].Bounds.IntersectsWith(pbPaddle.Bounds))
                {
                    if (engine.BallDamage < GameSettings.MaxBallDamage)
                    {
                        engine.BallDamage++;
                    }
                    RemoveBooster(i);
                }
                else if (boosters[i].Top > this.ClientSize.Height)
                {
                    RemoveBooster(i);
                }
            }
        }

        private void RemoveBooster(int index)
        {
            this.Controls.Remove(boosters[index]);
            boosters.RemoveAt(index);
        }

        private void ResetGame()
        {
            gameTimer.Stop();
            MessageBox.Show("Мяч упал! Игра окончена.");
        }
    }
}