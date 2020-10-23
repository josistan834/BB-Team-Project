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
        //load game over screen and display highscores
        public GameOverScreen()
        {
            InitializeComponent();

            List<score> sortedlist = GameScreen.highScoreList.OrderBy(s => s.numScore).ToList();
            foreach (score s in sortedlist)
            {
                highscoreLabel.Text += s.numScore + "\n";
            }
        }

        //close the program on exit
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //go to the gamescreen on play again
        private void playButton_Click(object sender, EventArgs e)
        {
            GameScreen gs = new GameScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }

        //go to menu screen on menu click
        private void button1_Click(object sender, EventArgs e)
        {
            MenuScreen gs = new MenuScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }

        //when you select the exit button
        private void exitButton_Enter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.MediumSpringGreen;
            playButton.BackColor = Color.PaleTurquoise;
            button1.BackColor = Color.PaleTurquoise;
        }

        //when you select the menu button
        private void button1_Enter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.PaleTurquoise;
            playButton.BackColor = Color.PaleTurquoise;
            button1.BackColor = Color.MediumSpringGreen;
        }

        //when you select the play button
        private void playButton_Enter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.PaleTurquoise;
            playButton.BackColor = Color.MediumSpringGreen;
            button1.BackColor = Color.PaleTurquoise;
        }
    }
}
