/*  
 *  Created by: Team 2
 *  Project: Brick Breaker
 *  Date: 2020
 */ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        #region global values

        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, rightArrowDown;

        // Game values
        int level;
        public static int paddleSpeed;
        public static int playerLives;
        int playerScore; // many need to change if player score gets too high

        // Paddle and Ball objects
        Paddle paddle;
        Ball ball;

        // list of all blocks for current level
        List<Block> blocks = new List<Block>();

        // Brushes
        SolidBrush paddleBrush = new SolidBrush(Color.White);
        SolidBrush ballBrush = new SolidBrush(Color.White);
        SolidBrush blockBrush = new SolidBrush(Color.Red);

        SolidBrush extraLifeBrush = new SolidBrush(Color.Green);
        SolidBrush longPaddleBrush = new SolidBrush(Color.White);
        SolidBrush shortPaddleBrush = new SolidBrush(Color.Red);
        SolidBrush fastPaddleBrush = new SolidBrush(Color.Yellow);
        SolidBrush fastBallBrush = new SolidBrush(Color.Blue);
        SolidBrush oppositeBrush = new SolidBrush(Color.Pink);
        SolidBrush oppositeBallBrush = new SolidBrush(Color.Aqua);

        // Jordan Var

        public List<PowerUps> powers = new List<PowerUps>();
        Random randJord = new Random();
        int powerPick;
        int powerDec;
        #endregion

        public GameScreen()
        {
            InitializeComponent();
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
            leftArrowDown = rightArrowDown = false;

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

            // Creates a new ball
            int xSpeed = 6;
            int ySpeed = 6;
            int ballSize = 20;
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
                paddle.width = 80;
                paddle.speed = 8;
                ball.xSpeed = 6;
                ball.ySpeed = 6;
                Paddle.opp = false;
                Ball.oppBall = false;

                lifeLabel.Text = playerLives + ""; // display updated life count
                //Move paddle to middle
                paddle.x = (this.Width / 2 - paddle.width);
                // Moves the ball back to origin
                ball.x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                ball.y = (this.Height - paddle.height) - 85;
                

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
                default:
                    break;
            }
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

            lifeLabel.Text = playerLives + "";

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


            //redraw the screen
            Refresh();
        }

        public void OnEnd()
        {
            // Goes to the game over screen
            Form form = this.FindForm();
            MenuScreen ps = new MenuScreen();
            
            ps.Location = new Point((form.Width - ps.Width) / 2, (form.Height - ps.Height) / 2);

            form.Controls.Add(ps);
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
    }
}
