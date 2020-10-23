using System.Drawing;

namespace BrickBreaker
{
    //paddle class properties and behaviors
    public class Paddle
    {
        public int x, y, width, height, speed;
        public Color colour;
        public static bool opp = false;
        public Paddle(int _x, int _y, int _width, int _height, int _speed, Color _colour)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            speed = _speed;
            colour = _colour;
        }

        //movement behaviour
        public void Move(string direction)
        {
            if (direction == "left")
            {
                x -= speed;
            }
            if (direction == "right")
            {
                x += speed;
            }
        }

        //when the paddle collides with a powerup
        public void PowerUpCollision(PowerUps p)
        {

            Rectangle powerRec = new Rectangle(p.x, p.y, p.width, p.height);
            Rectangle paddleRec = new Rectangle(x, y, width, height);
            if (p.power == "longPaddle")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    width += 40;
                    p.x = 2000;
                }
            }
            else if (p.power == "shortPaddle")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    width -= 40;
                    p.x = 2000;
                }
            }
            else if (p.power == "extraLife")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    GameScreen.playerLives++;
                    p.x = 2000;
                }
            }
            else if (p.power == "fastPaddle")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    speed += 8;
                    p.x = 2000;
                }
            }
            else if (p.power == "oppositeDir")
            {
                if (paddleRec.IntersectsWith(powerRec))
                {
                    opp = true;
                    p.x = 2000;
                }
            }
        }
    }
}
