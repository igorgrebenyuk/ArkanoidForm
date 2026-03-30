using Arkanoid.Logic;

namespace Arkanoid.UI;

/// <summary>
/// Окно игры. Отвечает ТОЛЬКО за отрисовку (UI) и перехват ввода.
/// </summary>
public partial class GameForm 
{
    private readonly GameEngine engine;
    private readonly Dictionary<Block, PictureBox> blockViews;
    private readonly Dictionary<Booster, PictureBox> boosterViews;

    /// <summary>
    /// Инициализирует компоненты формы, связывает её с игровым движком и настраивает обработку пользовательского ввода и таймера отрисовки.
    /// </summary>
    public GameForm()
    {
        InitializeComponent();

        engine = new GameEngine();
        blockViews = new Dictionary<Block, PictureBox>();
        boosterViews = new Dictionary<Booster, PictureBox>();

        SyncUiWithEngine();

        gameTimer.Tick += GameTimer_Tick;
        this.MouseMove += GameForm_MouseMove;
    }

    private void SyncUiWithEngine()
    {
        foreach (var blockPictureBox in blockViews.Values)
        {
            this.Controls.Remove(blockPictureBox);
        }
        foreach (var boosterPictureBox in boosterViews.Values)
        {
            this.Controls.Remove(boosterPictureBox);
        }

        blockViews.Clear();
        boosterViews.Clear();

        foreach (var block in engine.Blocks)
        {
            var blockPictureBox = new PictureBox
            {
                Bounds = block.Bounds,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = GetBlockColor(block.Health)
            };
            this.Controls.Add(blockPictureBox);
            blockViews.Add(block, blockPictureBox);
        }

        UpdateWindowText();
    }

    private static Color GetBlockColor(int healthPoints) => healthPoints switch
    {
        GameConstants.MaxBlockHealth => Color.Purple,
        GameConstants.HighHealth => Color.Red,
        GameConstants.MediumHealth => Color.Orange,
        GameConstants.LowHealth => Color.Yellow,
        _ => Color.Green
    };

    private void UpdateWindowText()
    {
        this.Text = $"Arkanoid | Урон мяча: {engine.BallDamage}";
    }

    private void GameForm_MouseMove(object? _, MouseEventArgs mouseEvent)
    {
        engine.MovePaddle(mouseEvent.X, this.ClientRectangle.Width);
        pbPaddle.Location = engine.Paddle.Location; 
    }

    private void GameTimer_Tick(object? _, EventArgs __)
    {
        engine.UpdatePhysics(this.ClientRectangle.Width, this.ClientRectangle.Height);

        pbBall.Location = engine.Ball.Location;
        pbPaddle.Location = engine.Paddle.Location;

        foreach (var pair in blockViews.ToList())
        {
            var logicalBlock = pair.Key;
            var pictureBoxView = pair.Value;

            if (!engine.Blocks.Contains(logicalBlock))
            {
                this.Controls.Remove(pictureBoxView);
                blockViews.Remove(logicalBlock);
            }
            else
            {
                pictureBoxView.BackColor = GetBlockColor(logicalBlock.Health);
            }
        }

        foreach (var booster in engine.Boosters)
        {
            if (!boosterViews.ContainsKey(booster))
            {
                var boosterPictureBox = new PictureBox
                {
                    Bounds = booster.Bounds,
                    BackColor = FormConstants.BoosterColor,
                    BorderStyle = FormConstants.BoosterBorderStyle
                };
                this.Controls.Add(boosterPictureBox);
                boosterViews.Add(booster, boosterPictureBox);
            }
            else
            {
                boosterViews[booster].Location = booster.Bounds.Location;
            }
        }

        foreach (var pair in boosterViews.ToList())
        {
            var logicalBooster = pair.Key;
            var pictureBoxView = pair.Value;

            if (!engine.Boosters.Contains(logicalBooster))
            {
                this.Controls.Remove(pictureBoxView);
                boosterViews.Remove(logicalBooster);
            }
        }

        UpdateWindowText();

        if (engine.IsBallLost)
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        gameTimer.Stop();
        MessageBox.Show("Мяч упал! Начинаем заново.");

        engine.ResetLevel();
        SyncUiWithEngine();

        gameTimer.Start();
    }
}