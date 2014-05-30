using NUnit.Framework;
using RandomLevel;

namespace RandomLevelTests
{
    [TestFixture]
    public class TileLayoutTests
    {
        private class TestGrid : IGrid
        {
            public int SizeX { get { return 3; }}
            public int SizeY { get { return 3; } }
            public char[,] Grid { get; set; }
        }

        [Test]
        public void TestFilled()
        {
            var grid = new TestGrid
            {
                Grid = new [,]
                {
                    {'X','X','X'},
                    {'X','X','X'},
                    {'X','X','X'},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);
            

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.Filled, tile.TileResource);
        }


        [Test]
        public void TestEmpty()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {' ',' ',' '},
                    {' ',' ',' '},
                    {' ',' ',' '},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);
            Assert.AreEqual(TileFlags.Empty, flags);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.EmptyTile, tile.TileResource);
        }

        [Test]
        public void TestRightTurn()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X','X','X'},
                    {'X',' ',' '},
                    {'X',' ','X'},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);
            Assert.AreEqual(TileFlags.NorthWest | TileFlags.West | TileFlags.SouthWest |
            TileFlags.North | TileFlags.NorthEast | TileFlags.SouthEast, flags);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.RightTurnTile, tile.TileResource);
        }

        [Test]
        public void TestLeftTurn()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X','X','X'},
                    {' ',' ','X'},
                    {'X',' ','X'},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);
            

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.RightTurnTile, tile.TileResource);
            Assert.AreEqual(90, tile.Orientation);
        }

        [Test]
        public void TestRightTurnRotated()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X',' ','X'},
                    {'X',' ',' '},
                    {'X','X','X'},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.RightTurnTile, tile.TileResource);
            Assert.AreEqual(270, tile.Orientation);
        }

        [Test]
        public void TestWall()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X',' ',' '},
                    {'X',' ',' '},
                    {'X',' ',' '},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);
            Assert.AreEqual(TileFlags.NorthWest | TileFlags.West |  TileFlags.SouthWest, flags);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.WallTile, tile.TileResource);
        }

        [Test]
        public void TestWallRotated()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X','X','X'},
                    {' ',' ',' '},
                    {' ',' ',' '},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.WallTile, tile.TileResource);
            Assert.AreEqual(90, tile.Orientation);
        }

        [Test]
        public void TestOuterCorner()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {' ',' ',' '},
                    {' ',' ',' '},
                    {'X',' ',' '},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);
            Assert.AreEqual(TileFlags.SouthWest, flags);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.EmptyTile, tile.TileResource);
        }

        public void TestCornerCorridor()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X',' ',' '},
                    {' ',' ',' '},
                    {'X','X','X'},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.CornerCorridorTile, tile.TileResource);
            Assert.AreEqual(0, tile.Orientation);
        }

        public void TestCornerCorridor2()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X','X','X'},
                    {' ',' ',' '},
                    {'X',' ',' '},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.CornerCorridor2Tile, tile.TileResource);
            Assert.AreEqual(0, tile.Orientation);
        }

        [Test]
        public void TestCorner()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X','X','X'},
                    {'X',' ',' '},
                    {'X',' ',' '},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.CornerTile, tile.TileResource);
            Assert.AreEqual(0, tile.Orientation);
        }

        [Test]
        public void TestCornerRotated()
        {
            var grid = new TestGrid
            {
                Grid = new[,]
                {
                    {'X','X','X'},
                    {' ',' ','X'},
                    {' ',' ','X'},
                }
            };
            var flags = LevelEncoder.GetTileFlags(grid, 1, 1);

            TileLayout.Tile tile;
            var result = TileLayout.TryMatchTile(1, 1, out tile, flags);

            Assert.IsTrue(result);
            Assert.AreEqual(TileConstants.CornerTile, tile.TileResource);
            Assert.AreEqual(90, tile.Orientation);
        }
    }
}
