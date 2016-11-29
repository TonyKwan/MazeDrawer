﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeDrawer.HelperClasses;

namespace MazeDrawer
{
    class Autobot
    {
        private List<ArrayHelper> tileArray;
        private int x;
        private int y;  
        private Orientation orientation;
        private Orientation lastOrientation;

        public Autobot(Orientation orientation)
        {
            this.orientation = orientation;
            this.tileArray = new List<ArrayHelper>();
            this.x = 0;
            this.y = 0;

        }

        public List<ArrayHelper> TileArray { get { return this.tileArray; } set { this.tileArray = value; } }
        public Orientation Orientation { get { return this.orientation; } set { this.orientation = value; } }
        public Orientation LastOrientation { get {return this.lastOrientation; } set {this.lastOrientation = value; } }
        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }
        /// <summary>
        /// Adds ArrayHelper object to the array
        /// </summary>
        /// <param name="obj"></param>
        public void AddToArray(ArrayHelper obj)
        {
            this.tileArray.Add(obj);
        }
    }
}