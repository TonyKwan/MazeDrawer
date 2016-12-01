using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeDrawer.HelperClasses
{
    class ArrayHelper
    {
        private int x;
        private int y;
        private TileType tileType;
        private Orientation tileOrientation;
        private Image tileImage;

        public ArrayHelper(int x, int y, TileType tileType, Orientation tileOrientation, Image tileImage)
        {
            this.x = x;
            this.y = y;
            this.tileType = tileType;
            this.tileOrientation = tileOrientation;
            this.tileImage = tileImage;
        }

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }
        public TileType TileType { get { return this.tileType; }set { this.tileType = value; } }
        public Orientation TileOrientation { get { return this.tileOrientation; } set { this.tileOrientation = value; } }
        public Image TileImage { get { return this.tileImage; } set { this.tileImage = value; } }
    }
}