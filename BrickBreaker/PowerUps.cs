using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    public class PowerUps
    {
        public int x, y, width, height;
        public string  power;
        public PowerUps(int _x, int _y, int _width, int _height, string _power)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            power = _power;
        }

    }
}
