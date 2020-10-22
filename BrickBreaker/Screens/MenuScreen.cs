using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BrickBreaker
{
    public partial class MenuScreen : UserControl
    {
        SolidBrush drawBrush = new SolidBrush(Color.Red);

        public MenuScreen()
        {
            InitializeComponent();
            Form1.seagulSound.Play();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            playButton.Visible = false;
            exitButton.Visible = false;

            label2.Visible = true;
            label2.Refresh();
            Thread.Sleep(1000);
            label2.Text = "2";
            label2.Refresh();
            Thread.Sleep(1000);
            label2.Text = "1";
            label2.Refresh();
            Thread.Sleep(1000);



            // Goes to the game screen
            GameScreen gs = new GameScreen();
            Form form = this.FindForm();

            form.Controls.Add(gs);
            form.Controls.Remove(this);

            gs.Location = new Point((form.Width - gs.Width) / 2, (form.Height - gs.Height) / 2);
        }


        private void exitButton_Enter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.MediumSpringGreen;
            playButton.BackColor = Color.PaleTurquoise;
            instructionsButton.BackColor = Color.PaleTurquoise;
        }

        private void playButton_Enter(object sender, EventArgs e)
        {
            playButton.BackColor = Color.MediumSpringGreen;
            exitButton.BackColor = Color.PaleTurquoise;
            instructionsButton.BackColor = Color.PaleTurquoise;
        }

        private void instructionsButton_Click(object sender, EventArgs e)
        {
            playButton.Visible = false;
            exitButton.Visible = false;
            instructionsButton.Visible = false;
            instructionsLabel.Visible = true;
            button1.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
        }

        private void instructionsButton_Enter(object sender, EventArgs e)
        {
            playButton.BackColor = Color.PaleTurquoise;
            exitButton.BackColor = Color.PaleTurquoise;
            instructionsButton.BackColor = Color.MediumSpringGreen;
        }

        private void button1_Enter(object sender, EventArgs e)
        {
            button1.BackColor = Color.MediumSpringGreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            playButton.Visible = true;
            exitButton.Visible = true;
            instructionsButton.Visible = true;
            instructionsLabel.Visible = false;
            button1.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
        }
    }
}
