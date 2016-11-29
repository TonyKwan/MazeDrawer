using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace MazeDrawer
{
    public enum Orientation { EAST, SOUTH, WEST, NORTH };

    public partial class Form1 : Form
    {
        // Class for reading the Arduino serial input
        private SerialPort serialport = new SerialPort();
        private ImageList tiles = new ImageList();
        private Image tileImage;
        private Autobot optimus;
        private Autobot bumblebee;
        private Graphics graphic;

        private Tile[,] tileArrayOptimus = new Tile[10, 10];
        private Tile[,] tileArrayMegatron = new Tile[10, 10];

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

            tiles.Images.Add(Image.FromFile(@"Tiles\halftile.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\straight.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\corner.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\Ttile.jpg"));
            tiles.Images.Add(Image.FromFile(@"Tiles\xtile.jpg"));

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
        /// Listener gets tile and nothing else
        /// </summary>
        /// <param name="data"></param>
        private void DataReceived(string data)
        {
            switch (data)
            {
                case "0":
                    Tile tileStraight = new Tile(tiles.Images[1]);
                    RotateTile(tileStraight);
                    break;
                case "1":
                    Tile tileTLeft = new Tile(tiles.Images[3]);
                    break;
                case "2":
                    Tile tileTRight = new Tile(tiles.Images[3]);
                    break;
                case "3":
                    Tile tileX = new Tile(tiles.Images[4]);
                    break;
                case "4":
                    Tile tileCornerLeft = new Tile(tiles.Images[2]);
                    break;
                case "5":
                    Tile tileCornerRight = new Tile(tiles.Images[2]);
                    break;
                case "6":
                    Tile tileT = new Tile(tiles.Images[3]);
                    break;
                case "7":
                    Tile tileDeadEnd = new Tile(tiles.Images[0]);
                    break;
            }
        }

        /// <summary>
        /// Corrects the tile to the correct orientation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tile"></param>
        private void RotateTile(Autobot sender, Tile tile)
        {
            switch (sender.Orientation)
            {
                case Orientation.EAST:
                    tile.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Orientation.SOUTH:
                    tile.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case Orientation.WEST:
                    tile.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
            }
        }

        private void resetMazeBtn_Click(object sender, EventArgs e)
        {
            Array.Clear(tileArrayOptimus, 0, tileArrayOptimus.Length);
            Array.Clear(tileArrayMegatron, 0, tileArrayMegatron.Length);
            graphic.Clear(Color.White);

        }
    }
}