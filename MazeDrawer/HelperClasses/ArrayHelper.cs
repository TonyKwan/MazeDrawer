using System.Collections.Generic;
using System.Drawing;

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
        private List<string> directions;

        public ArrayHelper(Image tileImage, TileType tileType)
        {
            this.tileImage = tileImage;
            this.tileType = tileType;
            directions = new List<string>();
        }

        public ArrayHelper(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }
        public TileType TileType { get { return this.tileType; }set { this.tileType = value; } }
        public Orientation TileOrientation { get { return this.tileOrientation; } set { this.tileOrientation = value; } }
        public Image TileImage { get { return this.tileImage; } set { this.tileImage = value; } }
        public int DeltaX { get { return this.deltaX; } set { this.deltaX = value; } }
        public int DeltaY { get { return this.deltaY; } set { this.deltaY = value; } }
        public List<string> Directions { get { return this.directions; } }

        public void ClearDirections()
        {
            directions.Clear();
        }

        public void AddDirections(List<string> directions)
        {
            foreach(string s in directions)
            {
                this.directions.Add(s);
            }
        }
    }
}