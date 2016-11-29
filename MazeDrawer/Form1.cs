using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using MazeDrawer.HelperClasses;

namespace MazeDrawer
{
    public enum Orientation { EAST, SOUTH, WEST, NORTH };
    public enum TileType { STRAIGHT, TTILE, XTILE, CORNER, DEADEND };

    public partial class Form1 : Form
    {
        // Class for reading the Arduino serial input
        private SerialPort serialport = new SerialPort();
        private ImageList tiles = new ImageList();
        private Image tileImage;
        private Autobot optimus;
        private Autobot bumblebee;
        private Graphics graphic;

        // Size of the drawing area
        private int drawAreaX = 1000;
        private int drawAreaY = 600;

        // Position of the first tile in the Array
        private int startX = 5;
        private int startY = 5;

        public Form1()
        {
            InitializeComponent();
            graphic = CreateGraphics();
            InitSerialport();
            serialport.Open();
            serialport.DataReceived += Serialport_DataReceived;

            tiles.Images.Add(Image.FromFile(@"Tiles\deadend.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\straight.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\corner.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\t-tile.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\x-tile.jpg"));

            optimus = new Autobot(Orientation.NORTH);
            bumblebee = new Autobot(Orientation.NORTH);
        }

        private void InitSerialport()
        {
            // Change PortName to whatever COM port the Listener Arduino is connected to. 
            serialport.PortName = "";
            serialport.BaudRate = 9600;
            serialport.Parity = Parity.None;
            serialport.DataBits = 8;
            serialport.StopBits = StopBits.One;
            serialport.ReceivedBytesThreshold = 1;
        }

        private void Serialport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialport.ReadLine();
            BeginInvoke(new LineReceivedEvent(DataReceived), data);
        }

        private delegate void LineReceivedEvent(string data);

        /// <summary>
        /// Determine the sender
        /// </summary>
        /// <param name="data"></param>
        private void DataReceived(string data)
        {
            char robot = data[0];

            switch(robot)
            {
                case '1':
                    DoRobotyStuff(optimus, data);
                    break;
                case '2':
                    DoRobotyStuff(bumblebee, data);
                    break;
            }
        }

        /* TODO: Rename this function at some point or another
         * Also write a better summary
         */
        /// <summary>
        /// Do things with robots and tiles 
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="data"></param>
        private void DoRobotyStuff(Autobot robot, string data)
        {
            switch (data)
            {
                case "0":
                    Tile tileStraight = new Tile(tiles.Images[1], TileType.STRAIGHT);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tiles.Images[1].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            //Orientation is relative to the default position of the tile as depicted in the images
                            tileStraight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            tileStraight.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.SOUTH:
                            tiles.Images[1].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileStraight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.WEST:
                            tileStraight.Orientation = Orientation.NORTH;
                            break;
                    }
                    UpdateRobot(robot, tileStraight);
                    break;
                case "1":
                    Tile tileTLeft = new Tile(tiles.Images[3], TileType.TTILE);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileTLeft.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileTLeft.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.SOUTH:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileTLeft.Orientation = Orientation.WEST;
                            break;
                        case Orientation.WEST:
                            tileTLeft.Orientation = Orientation.NORTH;
                            break;
                    }
                    UpdateRobot(robot, tileTLeft);
                    break;
                case "2":
                    Tile tileTRight = new Tile(tiles.Images[3], TileType.TTILE);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileTRight.Orientation = Orientation.WEST;
                            break;
                        case Orientation.EAST:
                            tileTRight.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.SOUTH:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileTRight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.WEST:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileTRight.Orientation = Orientation.SOUTH;
                            break;
                    }
                    UpdateRobot(robot, tileTRight);
                    break;
                case "3":
                    Tile tileX = new Tile(tiles.Images[4], TileType.XTILE);
                    tileX.Orientation = Orientation.NORTH;
                    UpdateRobot(robot, tileX);
                    break;
                case "4":
                    Tile tileCornerLeft = new Tile(tiles.Images[2], TileType.CORNER);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tiles.Images[2].RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileCornerLeft.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.EAST:
                            tiles.Images[2].RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileCornerLeft.Orientation = Orientation.WEST;
                            break;
                        case Orientation.SOUTH:
                            tileCornerLeft.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.WEST:
                            tiles.Images[2].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileCornerLeft.Orientation = Orientation.EAST;
                            break;
                    }
                    UpdateRobot(robot, tileCornerLeft);
                    break;
                case "5":
                    Tile tileCornerRight = new Tile(tiles.Images[2], TileType.CORNER);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tiles.Images[2].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileCornerRight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            tiles.Images[2].RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileCornerRight.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.SOUTH:
                            tiles.Images[2].RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileCornerRight.Orientation = Orientation.WEST;
                            break;
                        case Orientation.WEST:
                            tileCornerRight.Orientation = Orientation.NORTH;
                            break;
                    }
                    UpdateRobot(robot, tileCornerRight);
                    break;
                case "6":
                    Tile tileT = new Tile(tiles.Images[3], TileType.TTILE);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileT.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.EAST:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileT.Orientation = Orientation.WEST;
                            break;
                        case Orientation.SOUTH:
                            tileT.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.WEST:
                            tiles.Images[3].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileT.Orientation = Orientation.EAST;
                            break;
                    }
                    UpdateRobot(robot, tileT);
                    break;
                case "7":
                    Tile tileDeadEnd = new Tile(tiles.Images[0], TileType.DEADEND);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileDeadEnd.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.EAST:
                            tiles.Images[0].RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileDeadEnd.Orientation = Orientation.EAST;
                            break;
                        case Orientation.SOUTH:
                            tiles.Images[0].RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileDeadEnd.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.WEST:
                            tiles.Images[0].RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileDeadEnd.Orientation = Orientation.WEST;
                            break;
                    }
                    UpdateRobot(robot, tileDeadEnd);
                    break;
            }
        }

        private void UpdateRobot(Autobot robot, Tile tile)
        {
            ArrayHelper helper = new ArrayHelper(robot.X, robot.Y, tile.Type, tile.Orientation);
            robot.AddToArray(helper);
        }

        private void resetMazeBtn_Click(object sender, EventArgs e)
        {
            optimus.TileArray.Clear();
            bumblebee.TileArray.Clear();
            graphic.Clear(Color.White);

        }
    }
}