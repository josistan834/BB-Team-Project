using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrickBreaker
{
    public class Ball
    {
        public int x, y, xSpeed, ySpeed;
        public static int size;
        public Boolean ballRight, ballUp;
        public Color colour;
        public static bool oppBall = false;

        public static Random rand = new Random();

        public Ball(int _x, int _y, int _xSpeed, int _ySpeed, int _ballSize, bool _ballRight, bool _ballUp)
        {
            x = _x;
            y = _y;
            xSpeed = _xSpeed;
            ySpeed = _ySpeed;
            size = _ballSize;
            ballRight = _ballRight;
            ballUp = _ballUp;

        }

        public void Move()
        { 
            //ball goes left/right
            if (ballRight == true)
            {
                x = x + xSpeed;
            }
            else
            {
                x = x - xSpeed;
            }

            // ball goes up/down
            if (ballUp == true)
            {
                y = y - ySpeed;
            }
            else
            {
                y = y + ySpeed;
            }
        }

        //logic for ball collisions with blocks
        public bool BlockCollision(Block b)
        {
            Rectangle blockRec = new Rectangle(b.x, b.y, b.width, b.height);
            Rectangle ballRec = new Rectangle(x, y, size, size);

            if (ballRec.IntersectsWith(blockRec))
            {
                    if (x < b.x / 2 && ballRight == true)
                    {
                        ballRight = true;
                    ballUp = !ballUp;
                    }
                    else if (x < b.x / 2 && ballRight == false)
                    {
                        ballRight = false;
                    ballUp = !ballUp;
                }
                    else if (x > b.x / 2 && ballRight == true)
                    {
                        ballRight = true;
                    ballUp = !ballUp;
                }
                    else if (x > b.x / 2 && ballRight == false)
                    {
                        ballRight = false;
                    ballUp = !ballUp;
                }
            }

            return blockRec.IntersectsWith(ballRec);         
        }

        //logic for ball collisions with paddle
        public void PaddleCollision(Paddle p, bool pMovingLeft, bool pMovingRight)
        {
            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (ballRec.IntersectsWith(paddleRec))
            {
                //ball hits paddle, goes up
                if (y + size >= p.y)
                {
                    ballUp = true;
                }

                // if hits right side of paddle and paddle moving right, go right
                if (x > p.x + p.width / 2 && pMovingRight == true)
                {
                    ballRight = true;
                }
                // if hits right side of paddle and paddle not moving right, go right
                else if (x > p.x + p.width / 2 && pMovingRight == false)
                {
                    ballRight = true;
                }
                // if hits left side of paddle and paddle not moving right, go left
                else if (x < p.x + p.width / 2 && pMovingRight == false)
                {
                    ballRight = false;
                }
                // if hits left side of paddle and paddle moving right, go right
                else if (x < p.x + p.width / 2 && pMovingRight == true)
                {
                    ballRight = true;
                }

                // if hits right side of paddle and paddle not moving left, go right
                else if (x > p.x + p.width / 2 && pMovingLeft == false)
                {
                    ballRight = true;
                }
                // if hits right side of paddle and paddle moving left, go left
                else if (x > p.x + p.width / 2 && pMovingLeft == true)
                {
                    ballRight = false;
                }
                // if hits left side of paddle and paddle not moving left, go left
                else if (x < p.x + p.width / 2 && pMovingLeft == false)
                {
                    ballRight = false;
                }
                // if hits left side of paddle and paddle moving left, go left
                else if (x < p.x + p.width / 2 && pMovingLeft == true)
                {
                    ballRight = false;
                }
                

            }
        }

        public void WallCollision(UserControl UC)
        {
            // Collision with left wall, goes right
            if (x <= 1)
            {
                Form1.collisionSound.Play();
                ballRight = true;
                //if (oppBall == true)
                //{
                //    ballUp = false;
                //}
            }
            else if (x >= (UC.Width - size))
            {
                Form1.collisionSound.Play();
                ballRight = false;
                //if (oppBall == true)
                //{
                //    ballUp = false;
                //}
            }
            // Collision with top wall, goes down
            if (y <= 2)
            {
                Form1.collisionSound.Play();
                ballUp = false;
                //if (oppBall == true)
                //{
                //    ballRight = !ballRight;
                //}
            }
        }

        //logic for ball collision with bottom
        public bool BottomCollision(UserControl UC)
        {
            Boolean didCollide = false;

            if (y >= UC.Height)
            {
                didCollide = true;
            }

            return didCollide;
        }

        //logic for ball collision with powerups
        public void PowerUpBCollision(PowerUps p, Paddle s)
        {
            Rectangle powerRec = new Rectangle(p.x, p.y, p.width, p.height);
            Rectangle paddleRec = new Rectangle(s.x, s.y, s.width, s.height);
            if (p.power == "fastBall")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    xSpeed += 3;
                    ySpeed += 3;
                    p.x = 2000;
                }
            }
            else if (p.power == "oppositeBall")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    xSpeed += 0;
                    ySpeed += 2;
                    oppBall = true;
                    p.x = 2000;
                }
            }
            else if (p.power == "bigBall")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    size = size * 2;
                    p.x = 2000;
                }
                else if (p.power == "smallBall")
                {
                    if (paddleRec.IntersectsWith(powerRec))
                    {
                        size = size / 2;
                        p.x = 2000;
                    }
                }
            }
        }



    }
}
