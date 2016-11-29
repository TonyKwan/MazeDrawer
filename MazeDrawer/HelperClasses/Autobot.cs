using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeDrawer
{
    class Autobot
    {
        private Orientation orientation;
        private Orientation lastOrientation;

        public Autobot(Orientation orientation)
        {
            this.orientation = orientation;
        }

        public Orientation Orientation { get { return this.orientation; } set { this.orientation = value; } }
        public Orientation LastOrientation { get {return this.lastOrientation; } set {this.lastOrientation = value; } }
    }
}