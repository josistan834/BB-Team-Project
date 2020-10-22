//Clean build

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBreaker
{
    public partial class Form1 : Form
 
    {


        public static SoundPlayer bubbleSound = new SoundPlayer(Properties.Resources.bubbling2);
        public static SoundPlayer seagulSound = new SoundPlayer(Properties.Resources.seagul);
        public static SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);
        public static SoundPlayer breakBrick = new SoundPlayer(Properties.Resources.score);
        public static SoundPlayer loseSound = new SoundPlayer(Properties.Resources.lose);
        public static SoundPlayer winSound = new SoundPlayer(Properties.Resources.brickBreak);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Start the program centred on the Menu Screen
            MenuScreen ms = new MenuScreen();
            this.Controls.Add(ms);

            ms.Location = new Point((this.Width - ms.Width) / 2, (this.Height - ms.Height) / 2);
        }
    }
}
