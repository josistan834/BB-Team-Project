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
            foreach (Block b in blocks)
            {
                if (ball.BlockCollision(b)) // block health decreases when hit by ball
                {
                    b.hp--;

                    if (b.hp > 0) // player score increases when the ball hits a block
                    {
                        playerScore = playerScore + 50; // update score
                        scoreLabel.Text = playerScore + ""; // display updated score
                    }
                    else if (b.hp == 0) // remove block from screen if its health is zero
                    {
                        playerScore = playerScore + 100; // update score
                        scoreLabel.Text = playerScore + ""; // display updated score
                        blocks.Remove(b);
                    } 
                    
                    if (blocks.Count == 0) // go to next level if player finishes current level
                    {
                        gameTimer.Enabled = false;
                        OnEnd(); 
                    }

                    break;
                }
            }
        }

        public void OnStart()
        {
            //set life counter
            playerLives = 3;

            // display life and score values
            scoreLabel.Text = playerScore + "";
            lifeLabel.Text = playerLives + "";

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

   

            #region Creates blocks for generic level. Need to replace with code that loads levels.

            //TODO - replace all the code in this region eventually with code that loads levels from xml files

            blocks.Clear();
            int x = 10;

            while (blocks.Count() < 12)
            {
                x += 57;
                Block b1 = new Block(x, 10, 1, Color.White);
                blocks.Add(b1);
            }

            #endregion

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
                ball.x = this.Width / 2 - 10 - (paddle.width/2); 
                ball.y = this.Height - paddle.height - 80;
                ball.ballUp = true;
                paddle.x = this.Width / 2 - paddle.width;
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
                    blocks.Remove(b);
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

            if (stop == true)
            {
                if (pArrowDown == true)
                {
                    stop = false;
                    gameTimer.Start();

                }
            }
        }

        public void JordanMethod()
        {

            powerPick = randJord.Next(1, 5);
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
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            #region PowerUp
            // power ups fall
            for (int i = 0; i < powers.Count(); i++)
            {
                powers[i].y += 5;
                if (powers[i].y > this.Height - 30)

                {
                    powers.RemoveAt(i);
                }
            }
            foreach (PowerUps p in powers)
            {
                paddle.PowerUpCollision(p);
            }
            
            #endregion
            
            DeclanMethod();


            // Move the paddle
            if (leftArrowDown && paddle.x > 0)
            {

                paddle.Move("left");
            }
            if (rightArrowDown && paddle.x < (this.Width - paddle.width))
            {
                paddle.Move("right");
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
            }
            

            // Draws ball
            e.Graphics.FillRectangle(ballBrush, ball.x, ball.y, ball.size, ball.size);

        }
    }
}
