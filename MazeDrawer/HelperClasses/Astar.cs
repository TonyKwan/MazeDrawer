using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MazeDrawer.HelperClasses
{
    class Astar
    {
        List<ArrayHelper> mazeList = Form1.MazeList();

        private List<ArrayHelper> FindSurroundingTiles(ArrayHelper tile)
        {
            List<ArrayHelper> surroundingTiles = new List<ArrayHelper>();

            foreach(string direction in tile.Directions)
            {
                switch (direction)
                {
                    case "up":
                        ArrayHelper upTile = mazeList.Where(u => u.DeltaX == tile.DeltaX && u.DeltaY == tile.DeltaY - 1).FirstOrDefault();
                        if(upTile != null)
                        {
                            surroundingTiles.Add(upTile);
                        }
                        break;
                    case "down":
                        ArrayHelper downTile = mazeList.Where(d => d.DeltaX == tile.DeltaX && d.DeltaY == tile.DeltaY + 1).FirstOrDefault();
                        if(downTile != null)
                        {
                            surroundingTiles.Add(downTile);
                        }
                        break;
                    case "left":
                        ArrayHelper leftTile = mazeList.Where(l => l.DeltaX == tile.DeltaX - 1 && l.DeltaY == tile.DeltaY).FirstOrDefault();
                        if(leftTile != null)
                        {
                            surroundingTiles.Add(leftTile);
                        }
                        break;
                    case "right":
                        ArrayHelper rightTile = mazeList.Where(r => r.DeltaX == tile.DeltaX + 1 && r.DeltaY == tile.DeltaY).FirstOrDefault();
                        if(rightTile != null)
                        {
                            surroundingTiles.Add(rightTile);
                        }
                        break;
                }
            }
            return surroundingTiles;
        }

        private List<ArrayHelper> GetAdjacentTiles(ArrayHelper startTile)
        {
            List<ArrayHelper> options = new List<ArrayHelper>();
            List<ArrayHelper> surroundingTiles = FindSurroundingTiles(startTile);

            foreach (ArrayHelper tile in surroundingTiles)
            {
                // Ignore already-closed tiles
                if (tile.State == ArrayHelper.HelperState.CLOSED)
                    continue;

                // Already-open tiles are only added to the list if their G-value is lower going via this route.
                if (tile.State == ArrayHelper.HelperState.OPEN)
                {
                    if (startTile.G++ < tile.G)
                    {
                        tile.ParentArrayHelper = startTile;
                        options.Add(tile);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    tile.ParentArrayHelper = startTile;
                    tile.State = ArrayHelper.HelperState.OPEN;
                    options.Add(tile);
                }
            }
            return options;
        }

        private bool Search(ArrayHelper helper, ArrayHelper end)
        {
            helper.State = ArrayHelper.HelperState.CLOSED;
            List<ArrayHelper> nextHelpers = GetAdjacentTiles(helper);

            // Sort by F-value
            nextHelpers.Sort((helper1, helper2) => helper1.F.CompareTo(helper2.F));
            foreach (var nextHelper in nextHelpers)
            {
                if (nextHelper.Equals(end))
                {
                    return true;
                }
                else
                {
                    if (Search(nextHelper, end))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<ArrayHelper> FindPath(ArrayHelper startTile, ArrayHelper endTile)
        {
            startTile.State = ArrayHelper.HelperState.OPEN;

            List<ArrayHelper> path = new List<ArrayHelper>();
            bool success = Search(startTile, endTile);
            if (success)
            {
                ArrayHelper helper = endTile;
                while (helper.ParentArrayHelper != null)
                {
                    path.Add(helper);
                    helper = helper.ParentArrayHelper;
                }
                path.Reverse();
            }
            return path;
        }
    }
}