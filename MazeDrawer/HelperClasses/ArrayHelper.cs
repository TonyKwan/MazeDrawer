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
        private int deltaX;
        private int deltaY;

        public ArrayHelper(int x, int y, TileType tileType, Orientation tileOrientation, Image tileImage)
        {
            this.x = x;
            this.y = y;
            this.tileType = tileType;
            this.tileOrientation = tileOrientation;
            this.tileImage = tileImage;
        }

        public ArrayHelper(TileType tileType, Orientation tileOrientation, int deltaX = 0, int deltaY = 0)
        {
            this.deltaX = deltaX;
            this.deltaY = deltaY;
            this.tileType = tileType;
            this.tileOrientation = tileOrientation;
        }

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }
        public TileType TileType { get { return this.tileType; }set { this.tileType = value; } }
        public Orientation TileOrientation { get { return this.tileOrientation; } set { this.tileOrientation = value; } }
        public Image TileImage { get { return this.tileImage; } set { this.tileImage = value; } }
        public int DeltaX { get { return this.deltaX; } set { this.deltaX = value; } }
        public int DeltaY { get { return this.deltaY; } set { this.deltaY = value; } }
    }
}