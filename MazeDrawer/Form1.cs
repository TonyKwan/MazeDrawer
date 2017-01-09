using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Management;
using System.Windows.Forms;
using MazeDrawer.HelperClasses;
using System.Linq;

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

        private List<ArrayHelper> mazeList;
        private int xOptimus;
        private int yOptimus;
        private int xBumblebee;
        private int yBumblebee;

        private ArrayHelper optimusTile;
        private ArrayHelper bumbleBTile;

        private bool isMerged; 

        public Form1()
        {
            InitializeComponent();
            graphic = CreateGraphics();

            serialport = new SerialPort();
            InitSerialport();
            serialport.DataReceived += Serialport_DataReceived;

            tiles = new ImageList();
            AddImagesToList();

            mazeList = new List<ArrayHelper>();

            optimus = new Autobot("Optimus", Orientation.NORTH);
            bumblebee = new Autobot("Bumblebee", Orientation.NORTH);

            xOptimus = 237;
            yOptimus = 287;
            xBumblebee = 737;
            yBumblebee = 287;
            isMerged = false;
        }

        /// <summary>
        /// Prepares COM port properties 
        /// </summary>
        private void InitSerialport()
        {
            serialport.PortName = DetectArduinoPort();
            serialport.BaudRate = 9600;
            serialport.Parity = Parity.None;
            serialport.DataBits = 8;
            serialport.StopBits = StopBits.One;
            serialport.ReceivedBytesThreshold = 1;
            try
            {
                serialport.Open();
            }
            catch(UnauthorizedAccessException e)
            {
                MessageBox.Show("Something went wrong... =(" + Environment.NewLine + "Close whatever is using " + serialport.PortName, "WHOOPS!");
                System.Environment.Exit(1);
            }
        }

        /// <summary>
        /// Automatically detects the COM port the Arduino is connected to and returns it 
        /// </summary>
        /// <returns>Returns COM port Arduino is connected to</returns>
        private string DetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            string message = "Something went wrong... =(" + Environment.NewLine + "Check if the Arduino is connected!";
            string caption = "WHOOPS!";

            try
            {
                foreach(ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string port = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino"))
                    {
                        return port;
                    }
                }
            }
            catch(ManagementException e)
            {
                MessageBox.Show(message, caption);
                System.Environment.Exit(1);
            }

            MessageBox.Show(message, caption);
            System.Environment.Exit(1);
            return null;
        }

        /// <summary>
        /// Adds images to the list
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
        /// <param name="robot">The robot that sent the data</param>
        /// <param name="data">The data</param>
        private void DoRobotyStuff(Autobot robot, string data)
        {
            switch (data[1])
            {
                case '0':
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
                    UpdateRobotArray(robot, tileStraight, data);
                    UpdateRobotOrientation(robot, data);
                    break;
                case '1':
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
                    UpdateRobotArray(robot, tileTLeft, data);
                    UpdateRobotOrientation(robot, data);
                    break;
                case '2':
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
                    UpdateRobotArray(robot, tileTRight, data);
                    UpdateRobotOrientation(robot, data);
                    break;
                case '3':
                    Tile tileX = new Tile(tiles.Images[4], TileType.XTILE);
                    tileX.Orientation = Orientation.NORTH;
                    UpdateRobotArray(robot, tileX, data);
                    UpdateRobotOrientation(robot, data);
                    break;
                case '4':
                    Tile tileCornerLeft = new Tile(tiles.Images[2], TileType.CORNER);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileCornerLeft.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileCornerLeft.Orientation = Orientation.SOUTH;
                            //robot.Orientation = Orientation.WEST;
                            break;
                        case Orientation.EAST:
                            tileCornerLeft.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileCornerLeft.Orientation = Orientation.WEST;
                            //robot.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.SOUTH:
                            tileCornerLeft.Orientation = Orientation.NORTH;
                            //robot.Orientation = Orientation.EAST;
                            break;
                        case Orientation.WEST:
                            tileCornerLeft.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileCornerLeft.Orientation = Orientation.EAST;
                            //robot.Orientation = Orientation.SOUTH;
                            break;
                    }
                    UpdateRobotArray(robot, tileCornerLeft, data);
                    UpdateRobotOrientation(robot, data);
                    break;
                case '5':
                    Tile tileCornerRight = new Tile(tiles.Images[2], TileType.CORNER);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            tileCornerRight.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileCornerRight.Orientation = Orientation.EAST;
                            //robot.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            tileCornerRight.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileCornerRight.Orientation = Orientation.SOUTH;
                            //robot.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.SOUTH:
                            tileCornerRight.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileCornerRight.Orientation = Orientation.WEST;
                            //robot.Orientation = Orientation.WEST;
                            break;
                        case Orientation.WEST:
                            tileCornerRight.Orientation = Orientation.NORTH;
                            //robot.Orientation = Orientation.NORTH;
                            break;
                    }
                    UpdateRobotArray(robot, tileCornerRight, data);
                    UpdateRobotOrientation(robot, data);
                    break;
                case '6':
                    Tile tileT = new Tile(tiles.Images[3], TileType.TTILE);
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            //tileT.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileT.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.EAST:
                            tileT.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            tileT.Orientation = Orientation.EAST;
                            break;
                        case Orientation.SOUTH:
                            tileT.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            tileT.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.WEST:
                            tileT.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            tileT.Orientation = Orientation.WEST;
                            break;
                    }
                    UpdateRobotArray(robot, tileT, data);
                    UpdateRobotOrientation(robot, data);
                    break;
                case '7':
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
                    UpdateRobotArray(robot, tileDeadEnd, data);
                    UpdateRobotOrientation(robot, data);
                    break;
            }
        }

        /// <summary>
        /// Adjusts a robot's orientation.
        /// </summary>
        /// <param name="robot">The Robot</param>
        /// <param name="data">The data</param>
        private void UpdateRobotOrientation(Autobot robot, string data)
        {
            switch (data[2])
            {
                case '1':
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            robot.Orientation = Orientation.EAST;
                            break;
                        case Orientation.EAST:
                            robot.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.SOUTH:
                            robot.Orientation = Orientation.WEST;
                            break;
                        case Orientation.WEST:
                            robot.Orientation = Orientation.NORTH;
                            break;
                    }
                    break;
                case '2':
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            robot.Orientation = Orientation.SOUTH;
                            break;
                        case Orientation.EAST:
                            robot.Orientation = Orientation.WEST;
                            break;
                        case Orientation.SOUTH:
                            robot.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.WEST:
                            robot.Orientation = Orientation.EAST;
                            break;
                    }
                    break;
                case '3':
                    switch (robot.Orientation)
                    {
                        case Orientation.NORTH:
                            robot.Orientation = Orientation.WEST;
                            break;
                        case Orientation.EAST:
                            robot.Orientation = Orientation.NORTH;
                            break;
                        case Orientation.SOUTH:
                            robot.Orientation = Orientation.EAST;
                            break;
                        case Orientation.WEST:
                            robot.Orientation = Orientation.SOUTH;
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Adds tile to the robot-specific array
        /// </summary>
        /// <param name="robot">The robot</param>
        /// <param name="tile">The tile</param>
        private void UpdateRobotArray(Autobot robot, Tile tile, string data)
        {
            ArrayHelper helper = new ArrayHelper(robot.X, robot.Y * -1, tile.Type, tile.Orientation, tile.Image);
            robot.AddToArray(helper);

            // Only update robot coordinates if it's the second time the robot sends a tile 
            if (robot.TileArray.Count >= 1)
            {
                switch (data[2])
                {
                    case '0':
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
                                robot.X--;
                                break;
                        }
                        break;
                    case '1':
                        switch (robot.Orientation)
                        {
                            case Orientation.NORTH:
                                robot.X++;
                                break;
                            case Orientation.EAST:
                                robot.Y--;
                                break;
                            case Orientation.SOUTH:
                                robot.X--;
                                break;
                            case Orientation.WEST:
                                robot.Y++;
                                break;
                        }
                        break;
                    case '2':
                        switch (robot.Orientation)
                        {
                            case Orientation.NORTH:
                                robot.Y--;
                                break;
                            case Orientation.EAST:
                                robot.X--;
                                break;
                            case Orientation.SOUTH:
                                robot.Y++;
                                break;
                            case Orientation.WEST:
                                robot.X++;
                                break;
                        }
                        break;
                    case '3':
                        switch (robot.Orientation)
                        {
                            case Orientation.NORTH:
                                robot.X--;
                                break;
                            case Orientation.EAST:
                                robot.Y++;
                                break;
                            case Orientation.SOUTH:
                                robot.X++;
                                break;
                            case Orientation.WEST:
                                robot.Y--;
                                break;
                        }
                        break;
                }
            }

            DrawTile(robot, tile);
            if (!isMerged)
            {
                CompareMaze();
            }
        }

        /// <summary>
        /// Draws the tiles
        /// </summary>
        private void DrawTile(Autobot robot, Tile tile)
        {
            if (!isMerged)
            {
                // Start drawing in the center of their respective drawing areas. 
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
                        if (helper.X == prevHelper.X && helper.Y == prevHelper.Y)
                        {
                            graphic.DrawImage(tile.Image, xOptimus, yOptimus);
                        }
                        else if (helper.X < prevHelper.X)
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
            else
            {
                // secondary draw after merging mazes

            }
        }

        /// <summary>
        /// Draws merged maze
        /// </summary>
        /// <param name="list">List of tiles</param>
        private void DrawMaze(List<ArrayHelper> list)
        {
            // Clear current mazes first
            graphic.Clear(Color.White);
            int x = 475;
            int y = 275;
            int counter = 0; 

            foreach(ArrayHelper tile in list)
            {
                // draw the first tile in the middle of the screen 
                if(counter == 0)
                {
                    graphic.DrawImage(tile.TileImage, x, y);
                }
                else
                {
                    graphic.DrawImage(tile.TileImage, x + tile.DeltaX * 50, y - tile.DeltaY * -50);
                }
                counter++;
            }
        }
        
        /// <summary>
        /// Compares the maze of both robots to check for overlap
        /// </summary>
        private void CompareMaze()
        {
            if(optimus.TileArray.Count != 0)
            {
                optimusTile = optimus.TileArray.Last();
                ArrayHelper optimusEast = (ArrayHelper)optimus.TileArray.Where(o => o.X == optimusTile.X + 1 && o.Y == optimusTile.Y).FirstOrDefault();
                ArrayHelper optimusSouth = (ArrayHelper)optimus.TileArray.Where(o => o.Y == optimusTile.Y + 1 && o.X == optimusTile.X).FirstOrDefault();
                ArrayHelper optimusWest = (ArrayHelper)optimus.TileArray.Where(o => o.X == optimusTile.X - 1 && o.Y == optimusTile.Y).FirstOrDefault();
                ArrayHelper optimusNorth = (ArrayHelper)optimus.TileArray.Where(o => o.Y == optimusTile.Y - 1 && o.X == optimusTile.X).FirstOrDefault();
                int counter = 0;

                // Arbitrary amount of tiles that need to have been scanned before the computer starts comparing the two mazes 
                if (optimus.TileArray.Count >= 5 && bumblebee.TileArray.Count >= 5)
                {
                    foreach (ArrayHelper bumbleB in bumblebee.TileArray.Where(b => b.TileType.Equals(optimusTile.TileType) && b.TileOrientation.Equals(optimusTile.TileOrientation)))
                    {
                        ArrayHelper bumblebeeEast = (ArrayHelper)bumblebee.TileArray.Where(b => b.X == bumbleB.X + 1 && b.Y == bumbleB.Y).FirstOrDefault();
                        ArrayHelper bumblebeeSouth = (ArrayHelper)bumblebee.TileArray.Where(b => b.Y == bumbleB.Y + 1 && b.X == bumbleB.X).FirstOrDefault();
                        ArrayHelper bumblebeeWest = (ArrayHelper)bumblebee.TileArray.Where(b => b.X == bumbleB.X - 1 && b.Y == bumbleB.Y).FirstOrDefault();
                        ArrayHelper bumblebeeNorth = (ArrayHelper)bumblebee.TileArray.Where(b => b.Y == bumbleB.Y - 1 && b.X == bumbleB.X).FirstOrDefault();

                        // Is ugly, will remain ugly until an alternative is found
                        if (optimusEast != null && bumblebeeEast != null)
                        {
                            if (optimusEast.TileType.Equals(bumblebeeEast.TileType) && optimusEast.TileOrientation.Equals(bumblebeeEast.TileOrientation))
                            {
                                counter++;
                            }
                        }

                        if (optimusSouth != null && bumblebeeSouth != null)
                        {
                            if (optimusSouth.TileType.Equals(bumblebeeSouth.TileType) && optimusSouth.TileOrientation.Equals(bumblebeeSouth.TileOrientation))
                            {
                                counter++;
                            }
                        }


                        if (optimusWest != null && bumblebeeWest != null)
                        {
                            if (optimusWest.TileType.Equals(bumblebeeWest.TileType) && optimusWest.TileOrientation.Equals(bumblebeeWest.TileOrientation))
                            {
                                counter++;
                            }
                        }

                        if (optimusNorth != null && bumblebeeNorth != null)
                        {
                            if (optimusNorth.TileType.Equals(bumblebeeNorth.TileType) && optimusNorth.TileOrientation.Equals(bumblebeeNorth.TileOrientation))
                            {
                                counter++;
                            }
                        }

                        if (counter >= 3)
                        {
                            // If 3 or more tiles match, go ahead and merge the two mazes.
                            bumbleBTile = bumbleB;
                            MergeMaze(bumbleBTile, optimusTile);
                            break;
                        }
                        else
                        {
                            counter = 0;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// "Fixes" the robots' coordinates after merging the mazes
        /// </summary>
        private void FixRobotCoordinates()
        {

        }

        /// <summary>
        /// Merges both mazes
        /// </summary>
        /// <param name="bumbleB">Bumblebee tile</param>
        /// <param name="optimus">Optimus tile</param>
        private void MergeMaze(ArrayHelper bumbleB, ArrayHelper optimus)
        {
            foreach(ArrayHelper tile in bumblebee.TileArray)
            {
                tile.DeltaX = tile.X - bumbleB.X;
                tile.DeltaY = tile.Y + bumbleB.Y;
                mazeList.Add(tile);
            }
            foreach(ArrayHelper tile in this.optimus.TileArray)
            {
                tile.DeltaX = tile.X - optimus.X;
                tile.DeltaY = tile.Y + optimus.Y;
                mazeList.Add(tile);
            }
            isMerged = true;
            DrawMaze(mazeList);
        }

        /// <summary>
        /// Tell robot which way to go
        /// </summary>
        private void SendData(string data, Autobot robot)
        {
            string message;

            switch (robot.Name)
            {
                case "Optimus":
                    message = "1 " + data;
                    serialport.Write(message);
                    break;
                case "Bumblebee":
                    message = "2 " + data;
                    serialport.Write(message);
                    break;
            }
        }

        /*
         * TODO: Fix this funtion, doesn't work consistently. Maybe function is crap, maybe external factors causing problems.  
         */
        /// <summary>
        /// Resets the application, sort of, doesn't really work yet. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void resetMazeBtn_Click(object sender, EventArgs e)
        {
            xOptimus = 237;
            yOptimus = 287;
            xBumblebee = 737;
            yBumblebee = 287;
            optimus.TileArray.Clear();
            bumblebee.TileArray.Clear();
            graphic.Clear(Color.White);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendData("3", bumblebee);
        }
    }
}