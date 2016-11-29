using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MazeDrawer
{
    class Tile
    {
        private Image image;
        private TileType type;
        private Orientation orientation;

        public Tile(Image image, TileType type)
        {
            this.image = image;
            this.type = type;
        }

        public Image Image { get { return this.image; } set {this.image = value; } }
        public TileType Type { get { return this.type; } set { this.type = value; } }
        public Orientation Orientation { get { return this.orientation; } set { this.orientation = value; } }
    }
}
