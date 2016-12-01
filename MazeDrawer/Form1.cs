using System;
using System.Collections.Generic;
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
        private const string DEFAULT_IMAGE_PATH = "D:\\School\\Robots\\MazeDrawer\\MazeDrawer\\Tiles\\";

        // Class for reading the Arduino serial input
        private SerialPort serialport;
        private ImageList tiles;
        private Autobot optimus;
        private Autobot bumblebee;
        private Graphics graphic;

        private int xOptimus;
        private int yOptimus;
        private int xBumblebee;
        private int yBumblebee;

        public Form1()
        {
            InitializeComponent();
            graphic = CreateGraphics();

            serialport = new SerialPort();
            //InitSerialport();
            //serialport.Open();
            //serialport.DataReceived += Serialport_DataReceived;

            tiles = new ImageList();
            AddImagesToList();

            optimus = new Autobot("Optimus", Orientation.NORTH);
            bumblebee = new Autobot("Bumblebee", Orientation.NORTH);
            
            // Coordinates for the first tile, in the middle of their respective drawing areas
            xOptimus = 237;
            yOptimus = 287;
            xBumblebee = 737;
            yBumblebee = 287;
        }

        private void InitSerialport()
        {
            // Change PortName to whatever COM port the Listener Arduino is connected to. 
            serialport.PortName = "COM5";
            serialport.BaudRate = 9600;
            serialport.Parity = Parity.None;
            serialport.DataBits = 8;
            serialport.StopBits = StopBits.One;
            serialport.ReceivedBytesThreshold = 1;
        }

        /// <summary>
        /// Adds images to the list, why, I dunno....
        /// </summary>
        private void AddImagesToList()
        {
            tiles.Images.Add(Image.FromFile(DEFAULT_IMAGE_PATH + "deadend.jpg"));
            tiles.Images.Add(Image.FromFile(DEFAULT_IMAGE_PATH + "straight.jpg"));
            tiles.Images.Add(Image.FromFile(DEFAULT_IMAGE_PATH + "corner.jpg"));
            tiles.Images.Add(Image.FromFile(DEFAULT_IMAGE_PATH + "t-tile.jpg"));
            tiles.Images.Add(Image.FromFile(DEFAULT_IMAGE_PATH + "x-tile.jpg"));
            tiles.ImageSize = new Size(50, 50);
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
                            tileStraight.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            //Orientation is relative to the default position of the tile as depicted in the images
                            tileStraight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            tileStraight.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.SOUTH:
                            tileStraight.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileStraight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.WEST:
                            tileStraight.Orientation = Orientation.NORTH;
                            break;
                    }
                    UpdateRobotArray(robot, tileStraight);
                    break;
                case "1":
                    Tile tileTLeft = new Tile(tiles.Images[3], TileType.TTILE);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileTLeft.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileTLeft.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            tileTLeft.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileTLeft.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.SOUTH:
                            tileTLeft.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileTLeft.Orientation = Orientation.WEST;
                            break;
                        case Orientation.WEST:
                            tileTLeft.Orientation = Orientation.NORTH;
                            break;
                    }
                    UpdateRobotArray(robot, tileTLeft);
                    break;
                case "2":
                    Tile tileTRight = new Tile(tiles.Images[3], TileType.TTILE);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileTRight.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileTRight.Orientation = Orientation.WEST;
                            break;
                        case Orientation.EAST:
                            tileTRight.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.SOUTH:
                            tileTRight.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileTRight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.WEST:
                            tileTRight.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileTRight.Orientation = Orientation.SOUTH;
                            break;
                    }
                    UpdateRobotArray(robot, tileTRight);
                    break;
                case "3":
                    Tile tileX = new Tile(tiles.Images[4], TileType.XTILE);
                    tileX.Orientation = Orientation.NORTH;
                    UpdateRobotArray(robot, tileX);
                    break;
                case "4":
                    Tile tileCornerLeft = new Tile(tiles.Images[2], TileType.CORNER);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileCornerLeft.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileCornerLeft.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.EAST:
                            tileCornerLeft.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileCornerLeft.Orientation = Orientation.WEST;
                            break;
                        case Orientation.SOUTH:
                            tileCornerLeft.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.WEST:
                            tileCornerLeft.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileCornerLeft.Orientation = Orientation.EAST;
                            break;
                    }
                    UpdateRobotArray(robot, tileCornerLeft);
                    break;
                case "5":
                    Tile tileCornerRight = new Tile(tiles.Images[2], TileType.CORNER);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileCornerRight.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileCornerRight.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            tileCornerRight.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileCornerRight.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.SOUTH:
                            tileCornerRight.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileCornerRight.Orientation = Orientation.WEST;
                            break;
                        case Orientation.WEST:
                            tileCornerRight.Orientation = Orientation.NORTH;
                            break;
                    }
                    UpdateRobotArray(robot, tileCornerRight);
                    break;
                case "6":
                    Tile tileT = new Tile(tiles.Images[3], TileType.TTILE);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileT.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileT.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.EAST:
                            tileT.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileT.Orientation = Orientation.WEST;
                            break;
                        case Orientation.SOUTH:
                            tileT.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.WEST:
                            tileT.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileT.Orientation = Orientation.EAST;
                            break;
                    }
                    UpdateRobotArray(robot, tileT);
                    break;
                case "7":
                    Tile tileDeadEnd = new Tile(tiles.Images[0], TileType.DEADEND);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileDeadEnd.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.EAST:
                            tileDeadEnd.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileDeadEnd.Orientation = Orientation.EAST;
                            break;
                        case Orientation.SOUTH:
                            tileDeadEnd.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileDeadEnd.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.WEST:
                            tileDeadEnd.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileDeadEnd.Orientation = Orientation.WEST;
                            break;
                    }
                    UpdateRobotArray(robot, tileDeadEnd);
                    break;
            }
        }

        private void UpdateRobotArray(Autobot robot, Tile tile)
        {
            // Only update robot coordinates if it's the second time the robot sends a tile 
            if(robot.TileArray.Count >= 1)
            {
                switch (robot.Orientation)
                {
                    case Orientation.NORTH:
                        robot.Y++; 
                        break;
                    case Orientation.EAST:
                        robot.X++;
                        break;
                    case Orientation.SOUTH:
                        robot.Y--;
                        break;
                    case Orientation.WEST:
                        robot.X++;
                        break;
                }
            }
            ArrayHelper helper = new ArrayHelper(robot.X, robot.Y, tile.Type, tile.Orientation, tile.Image);
            robot.AddToArray(helper);
        }

        /// <summary>
        /// Draws the tiles
        /// </summary>
        private void DrawTile(Autobot robot, Tile tile)
        {
            List<ArrayHelper> helperList = robot.TileArray;
            ArrayHelper helper = helperList[helperList.Count - 1];
            ArrayHelper prevHelper = new ArrayHelper(0, 0, tile.Type, tile.Orientation, tile.Image);

            if (helperList.Count > 1)
            {
                prevHelper = helperList[helperList.Count - 2];
            }

            switch (robot.Name)
            {
                case "Optimus":
                    if(helper.X == prevHelper.X && helper.Y == prevHelper.Y)
                    {
                        graphic.DrawImage(tile.Image, xOptimus, yOptimus);
                    }
                    else if(helper.X < prevHelper.X)
                    {
                        xOptimus -= 50;
                        graphic.DrawImage(tile.Image, xOptimus, yOptimus);
                    }
                    else if (helper.X > prevHelper.X)
                    {
                        xOptimus += 50;
                        graphic.DrawImage(tile.Image, xOptimus, yOptimus);
                    }
                    else if (helper.Y < prevHelper.Y)
                    {
                        yOptimus -= 50;
                        graphic.DrawImage(tile.Image, xOptimus, yOptimus);
                    }
                    else if (helper.Y > prevHelper.Y)
                    {
                        yOptimus += 50;
                        graphic.DrawImage(tile.Image, xOptimus, yOptimus);
                    }
                    else
                    {
                        this.messagebox.Items.Add("Something went wrong...");
                    }
                    break;
                case "Bumblebee":
                    if (helper.X == prevHelper.X && helper.Y == prevHelper.Y)
                    {
                        graphic.DrawImage(tile.Image, xBumblebee, yBumblebee);
                    }
                    else if (helper.X < prevHelper.X)
                    {
                        xBumblebee -= 50;
                        graphic.DrawImage(tile.Image, xBumblebee, yBumblebee);
                    }
                    else if (helper.X > prevHelper.X)
                    {
                        xBumblebee += 50;
                        graphic.DrawImage(tile.Image, xBumblebee, yBumblebee);
                    }
                    else if (helper.Y < prevHelper.Y)
                    {
                        yBumblebee -= 50;
                        graphic.DrawImage(tile.Image, xBumblebee, yBumblebee);
                    }
                    else if (helper.Y > prevHelper.Y)
                    {
                        yBumblebee += 50;
                        graphic.DrawImage(tile.Image, xBumblebee, yBumblebee);
                    }
                    else
                    {
                        this.messagebox.Items.Add("Something went wrong...");
                    }
                    break;
            }
        }

        /// <summary>
        /// Compares the maze of both robots to figure out where they are relative to one another. 
        /// </summary>
        private void CompareMaze()
        {
            List<ArrayHelper> optimusList = optimus.TileArray;
            List<ArrayHelper> bumblebeeList = bumblebee.TileArray;


        }

        private void resetMazeBtn_Click(object sender, EventArgs e)
        {
            optimus.TileArray.Clear();
            bumblebee.TileArray.Clear();
            graphic.Clear(Color.White);
        }
    }
}