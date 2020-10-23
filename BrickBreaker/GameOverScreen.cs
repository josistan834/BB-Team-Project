using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBreaker
{
    public partial class GameOverScreen : UserControl
    {
        public GameOverScreen()
        {
            InitializeComponent();

            GameScreen.HighScoreWrite();
            GameScreen.HighScoreRead();

            // display high scores to high score label
            foreach (score s in GameScreen.highScoreList)
            {
                highscoreLabel.Text += s.numScore + "\n";
            }
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            GameScreen gs = new GameScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuScreen gs = new MenuScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }

        private void exitButton_Enter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.MediumSpringGreen;
            playButton.BackColor = Color.PaleTurquoise;
            button1.BackColor = Color.PaleTurquoise;
        }

        private void button1_Enter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.PaleTurquoise;
            playButton.BackColor = Color.PaleTurquoise;
            button1.BackColor = Color.MediumSpringGreen;
        }

        private void playButton_Enter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.PaleTurquoise;
            playButton.BackColor = Color.MediumSpringGreen;
            button1.BackColor = Color.PaleTurquoise;
        }
    }
}
