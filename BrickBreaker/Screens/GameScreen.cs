/*  

 *  Created by: Calem, Declan, Kyle, Jordan, Josiah, Phaedra

 *  Project: Brick Breaker

 *  Date: Oct, 2020 
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;


namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        #region global values

        //player1 button control keys - DO NOT CHANGE

        Boolean leftArrowDown, rightArrowDown, pArrowDown, upArrowDown;
        Boolean stop = false;



        // Game values
        int level;
        public static int paddleSpeed;
        public static int playerLives;
        int playerScore; // many need to change if player score gets too high

        // ball values
        int xSpeed = 6;
        int ySpeed = 6;
        int ballSize = 20;

        // Paddle and Ball objects
        Paddle paddle;
        Ball ball;

        // list of all blocks for current level
        public static List<Block> blocks = new List<Block>();

        // Brushes
        SolidBrush paddleBrush = new SolidBrush(Color.White);
        SolidBrush ballBrush = new SolidBrush(Color.White);
        SolidBrush blockBrush = new SolidBrush(Color.Red);
        Pen anglePen = new Pen(Color.Red);

        SolidBrush extraLifeBrush = new SolidBrush(Color.Green);
        SolidBrush longPaddleBrush = new SolidBrush(Color.White);
        SolidBrush shortPaddleBrush = new SolidBrush(Color.Red);
        SolidBrush fastPaddleBrush = new SolidBrush(Color.Yellow);
        SolidBrush fastBallBrush = new SolidBrush(Color.Blue);
        SolidBrush oppositeBrush = new SolidBrush(Color.Pink);
        SolidBrush oppositeBallBrush = new SolidBrush(Color.Aqua);


        //List that will build highscores using a class to then commit them to a XML file
        List<score> highScoreList = new List<score>();

        // Jordan Var

        public List<PowerUps> powers = new List<PowerUps>();
        Random randJord = new Random();
        int powerPick;
        int powerDec;

        #endregion

        public GameScreen()
        {
            InitializeComponent();
            Form1.seagulSound.Stop();
            OnStart();
        }


        public void DeclanMethod()
        {
            // Check if ball has collided with any blocks
            
            if (playerLives == 0)
            {
                gameTimer.Enabled = false;
                OnEnd();
            }
        }


        public void OnStart()
        {
            //set life counter
            playerLives = 3;

            // display life and score values
            scoreLab.Text = playerScore + "";
            lifeLab.Text = playerLives + "";

            //set all button presses to false.
            leftArrowDown = rightArrowDown = pArrowDown = false;

            // setup starting paddle values and create paddle object
            int paddleWidth = 80;
            int paddleHeight = 20;
            int paddleX = ((this.Width / 2) - (paddleWidth / 2));
            int paddleY = (this.Height - paddleHeight) - 60;
            paddleSpeed = 8;
            paddle = new Paddle(paddleX, paddleY, paddleWidth, paddleHeight, paddleSpeed, Color.White);

            #region ball variables
            // setup starting ball values
            int ballX = this.Width / 2 - 10;
            int ballY = this.Height - paddle.height - 80;

            //AngleMethod();

            //starts ball moving up and right
            bool ballRight = true;
            bool ballUp = true;
            ball = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize, ballRight, ballUp);
            #endregion

            // chooses starting powerUp

            powerPick = randJord.Next(1, 3);


            levelOne(); // call level one method

            // start the game engine loop
            gameTimer.Enabled = true;
        }

        public void CalemMethod()
        {
            // Move ball
            ball.Move();

            #region collisions
            // Check for collision with top and side walls
            ball.WallCollision(this);

            // Check for ball hitting bottom of screen
            if (ball.BottomCollision(this))
            {

                playerLives--;
                paddle.width = 80;
                paddle.speed = 8;
                ball.xSpeed = 6;
                ball.ySpeed = 6;
                Paddle.opp = false;
                Ball.oppBall = false;
                lifeLab.Text = playerLives + ""; // display updated life count

                //Move paddle to middle
                paddle.x = (this.Width / 2 - paddle.width);
                // Moves the ball back to origin
                ball.x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                ball.y = (this.Height - paddle.height) - 85;
                ball.ballUp = true;               
                Refresh();
                if(playerLives != 0)
                {
                    Thread.Sleep(2000);
                }

                if (playerLives == 0)
                {
                    
                    gameTimer.Enabled = false;
                    OnEnd();
                }
            }

            // Check for collision of ball with paddle, (incl. paddle movement)
            ball.PaddleCollision(paddle, leftArrowDown, rightArrowDown);

            // Check if ball has collided with any blocks
            foreach (Block b in blocks)
            {
                if (ball.BlockCollision(b))
                {
                    b.hp--;
                    BlockColour();
                    if (b.hp > 0) // player score increases when the ball hits a block
                    {
                        
                        playerScore = playerScore + 50; // update score
                        scoreLab.Text = playerScore + ""; // display updated score
                    }

                    else  // remove block from screen if its health is zero
                    {
                        playerScore = playerScore + 100; // update score
                        scoreLab.Text = playerScore + ""; // display updated score
                        blocks.Remove(b);
                    }

                    
                    powerDec = randJord.Next(1, 100);
                    if (powerDec > 1 && powerDec < 100)
                    {
                        JordanMethod();
                    }
                    if (blocks.Count == 0)
                    {
                        gameTimer.Enabled = false;
                        OnEnd();
                    }

                    break;  
                }
            }
            #endregion
        }


        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;

                case Keys.P:
                    pArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;

                    break;
                default:
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.P:
                    pArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;

                    break;
                default:
                    break;
            }
        }


        public void pause()
        {
            pArrowDown = false;
            gameTimer.Stop();
            stop = true;
            label1.Visible = true;
            playButton.Visible = true;
            exitButton.Visible = true;
            playButton.Focus();

        }


        public void JordanMethod()
        {

            powerPick = randJord.Next(1, 8);
            if (powerPick == 1)

            {
                PowerUps extraLife = new PowerUps(ball.x, ball.y, 20, 20, "extraLife");
                powers.Add(extraLife);
            }
            else if (powerPick == 2)
            {
                PowerUps longPaddle = new PowerUps(ball.x, ball.y, 20, 20, "longPaddle");
                powers.Add(longPaddle);
            }

            else if (powerPick == 3)
            {
                PowerUps shortPaddle = new PowerUps(ball.x, ball.y, 20, 20, "shortPaddle");
                powers.Add(shortPaddle);
            }
            else if (powerPick == 4)
            {
                PowerUps fastPaddle = new PowerUps(ball.x, ball.y, 20, 20, "fastPaddle");
                powers.Add(fastPaddle);
            }
            else if (powerPick == 5)
            {
                PowerUps fastBall = new PowerUps(ball.x, ball.y, 20, 20, "fastBall");
                powers.Add(fastBall);
            }
            else if (powerPick == 6)
            {
                PowerUps oppositeDir = new PowerUps(ball.x, ball.y, 20, 20, "oppositeDir");
                powers.Add(oppositeDir);
            }
            else if (powerPick == 7)
            {
                PowerUps oppositeBall = new PowerUps(ball.x, ball.y, 20, 20, "oppositeBall");
                powers.Add(oppositeBall);
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            playButton.Visible = false;
            exitButton.Visible = false;
            gameTimer.Start();
            this.Focus();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void playenter(object sender, EventArgs e)
        {
            exitButton.BackColor = Color.MediumSpringGreen;
            playButton.BackColor = Color.PaleTurquoise;
        }

        private void exitenter(object sender, EventArgs e)
        {
            playButton.BackColor = Color.MediumSpringGreen;
            exitButton.BackColor = Color.PaleTurquoise;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            #region PowerUp
            // power ups fall
            for (int i = 0; i < powers.Count(); i++)
            {
                powers[i].y += 8;
                if (powers[i].y > this.Height - 30)

                {
                    powers.RemoveAt(i);
                }
            }
            foreach (PowerUps p in powers)
            {
                paddle.PowerUpCollision(p);
                ball.PowerUpBCollision(p, paddle);

            }

            #endregion

            DeclanMethod();

            // Move the paddle
            if (leftArrowDown && paddle.x > 0)
            {
                if (Paddle.opp == true)
                {
                    paddle.Move("right");
                }
                else
                {
                    paddle.Move("left");
                }  

            }
            if (rightArrowDown && paddle.x < (this.Width - paddle.width))
            {
                if (Paddle.opp == true)
                {
                    paddle.Move("left");
                }
                else
                {
                    paddle.Move("right");
                }
            }



            CalemMethod();




            if (pArrowDown == true)
            {
                pause();

            }


            //redraw the screen
            Refresh();
        }

        public void OnEnd()
        {
            HighScoreWrite();
            HighScoreRead();

            // Goes to the game over screen
            Form form = this.FindForm();

            GameOverScreen go = new GameOverScreen();

            go.Location = new Point((form.Width - go.Width) / 2, (form.Height - go.Height) / 2);

            form.Controls.Add(go);
            form.Controls.Remove(this);
        }

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            // Draws paddle
            paddleBrush.Color = paddle.colour;
            e.Graphics.FillRectangle(paddleBrush, paddle.x, paddle.y, paddle.width, paddle.height);

            // Draws blocks
            foreach (Block b in blocks)
            {
                if (b.hp == 1)
                {
                    b.colour = Color.LightYellow;
                }
                else if (b.hp == 2)
                {
                    b.colour = Color.Yellow;
                }
                else if (b.hp == 3)
                {
                    b.colour = Color.OrangeRed;
                }
                else if (b.hp == 4)
                {
                    b.colour = Color.Orange;
                }
                else if (b.hp == 5)
                {
                    b.colour = Color.DarkOrange;
                }
                blockBrush.Color = b.colour;

                e.Graphics.FillRectangle(blockBrush, b.x, b.y, b.width, b.height);
            }

            // Draws powerUp
            foreach (PowerUps p in powers)
            {
                if (p.power == "longPaddle")
                {
                    e.Graphics.FillEllipse(longPaddleBrush, p.x, p.y, p.width, p.height);
                }
                else if (p.power == "extraLife")
                {
                    e.Graphics.FillEllipse(extraLifeBrush, p.x, p.y, p.width, p.height);
                }
                else if (p.power == "shortPaddle")
                {
                    e.Graphics.FillEllipse(shortPaddleBrush, p.x, p.y, p.width, p.height);
                }
                else if (p.power == "fastPaddle")
                {
                    e.Graphics.FillEllipse(fastPaddleBrush, p.x, p.y, p.width, p.height);
                }
                else if (p.power == "fastBall")
                {
                    e.Graphics.FillEllipse(fastBallBrush, p.x, p.y, p.width, p.height);
                }
                else if (p.power == "oppositeDir")
                {
                    e.Graphics.FillEllipse(oppositeBrush, p.x, p.y, p.width, p.height);
                }
                else if (p.power == "oppositeBall")
                {
                    e.Graphics.FillEllipse(oppositeBallBrush, p.x, p.y, p.width, p.height);
                }
            }


            // Draws ball
            e.Graphics.FillRectangle(ballBrush, ball.x, ball.y, ball.size, ball.size);

        }

        public void HighScoreRead()
        {
            // create reader
            XmlTextReader reader = new XmlTextReader("Resources/highScores.xml");

            // read high score xml file
            while (reader.Read())
            {
                // take high score values from high score xml file
                if (reader.NodeType == XmlNodeType.Text)
                {
                    reader.ReadToNextSibling("score");
                    string numScore = reader.ReadString();

                    // add score to high score list
                    score s = new score(numScore);
                    highScoreList.Add(s);

                    //highScoreLabel.Text += s.numScore + "\n";
                }
            }

            // remove the lowest high score if there are already 10 scores when adding a new score 
            if (highScoreList.Count > 10)
            {
                highScoreList.RemoveAt(10);
            }

            reader.Close();
        }

        public void HighScoreWrite()
        {
            // create write for xml file
            XmlWriter writer = XmlWriter.Create("highScores.xml", null);

            // start writer
            writer.WriteStartElement("Highscores");

            // write every score in high score list
            foreach (score s in highScoreList)
            {
                writer.WriteStartElement("playerScore");

                writer.WriteElementString("score", s.numScore);

                writer.WriteEndElement();
            }

            // end and close writer
            writer.WriteEndElement();
            writer.Close();
        }

        public void BlockColour()
        {
            // change block colour based on the block's health
            foreach (Block b in blocks)
            {
                if (b.hp == 1)
                {
                    b.colour = Color.LightYellow;
                }
                else if (b.hp == 2)
                {
                    b.colour = Color.Yellow;
                }
                else if (b.hp == 3)
                {
                    b.colour = Color.OrangeRed;
                }
                else if (b.hp == 4)
                {
                    b.colour = Color.Orange;
                }
                else if (b.hp == 5)
                {
                    b.colour = Color.DarkOrange;
                }
            }
        }

        public void levelOne()
        {
            // current level
            level = 1;

            // variables for block x and y values
            string blockX;
            string blockY;
            int intX;
            int intY;

            // create xml reader
            XmlTextReader reader = new XmlTextReader($"Resources/level{level}.xml");

            reader.ReadStartElement("level");

            //Grabs all the blocks for the current level and adds them to the list
            while (reader.Read())
            {
                reader.ReadToFollowing("x");
                blockX = reader.ReadString();

                reader.ReadToFollowing("y");
                blockY = reader.ReadString();

                if (blockX != "")
                {
                    intX = Convert.ToInt32(blockX);
                    intY = Convert.ToInt32(blockY);
                    Block b = new Block(intX, intY, level);
                    blocks.Add(b);                    
                }
            }
            // close reader
            reader.Close();
        }
    }
}
