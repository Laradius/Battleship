namespace Battleship
{
    internal static class GameManager
    {


        private static bool _isInitialized = false;
        public static int WorldSizeX { get; private set; }
        public static int WorldSizeY { get; private set; }

        public static List<Ship> Ships { get; private set; } = new List<Ship>();
        public static List<Position> WorldOnePositions { get; private set; } = new List<Position>();
        public static List<Position> WorldTwoPositions { get; private set; } = new List<Position>();





        public static void MakeWorlds(int sizeX, int sizeY)
        {

            if (!_isInitialized)
            {

                WorldSizeX = sizeX;
                WorldSizeY = sizeY;

                for (int y = 0; y < WorldSizeY; y++)
                {
                    for (int x = 0; x < WorldSizeX; x++)
                    {
                        WorldOnePositions.Add(new Position(x, y));
                    }
                }
                for (int y = 0; y < WorldSizeY; y++)
                {
                    for (int x = 0; x < WorldSizeX; x++)
                    {
                        WorldTwoPositions.Add(new Position(x, y));

                    }
                }
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("World is already initialized.");
            }
        }


        public static string DrawWorlds()
        {

            if (!_isInitialized)
            {
                throw new InvalidOperationException("Wolrd is not initialized.");
            }

            string world1 = "World 1:" + Environment.NewLine;
            string world2 = "World 2:" + Environment.NewLine;

            for (int y = 0; y < WorldSizeY; y++)
            {
                for (int x = 0; x < WorldSizeX; x++)
                {

                    Position p = GetPosition(x, y, true);

                    if (p.IsTaken)
                    {
                        world1 += p.Occupant.Ship.ShipType;
                    }
                    else if (p.IsShot)
                    {
                        world1 += "#";
                    }
                    else
                    {
                        world1 += "*";
                    }

                }
                world1 += Environment.NewLine;
            }

            for (int y = 0; y < WorldSizeY; y++)
            {
                for (int x = 0; x < WorldSizeX; x++)
                {

                    Position p = GetPosition(x, y, false);

                    if (p.IsTaken)
                    {
                        world2 += p.Occupant.Ship.ShipType;
                    }
                    else if (p.IsShot)
                    {
                        world2 += "#";
                    }
                    else
                    {
                        world2 += "*";
                    }

                }
                world2 += Environment.NewLine;
            }
            return world1 + world2;
        }

        public static Position GetPosition(int x, int y, bool world1)
        {
            List<Position> positions;

            if (world1)
            {
                positions = WorldOnePositions;
            }
            else
            {
                positions = WorldTwoPositions;
            }

            Position position = positions.Find(p => p.X == x && p.Y == y);

            if (position == null)
            {
                throw new InvalidOperationException("Position is out of world bounds.");
            }
            else
            {
                return position;
            }


        }

        public static void MakeTurn()
        {
            throw new NotImplementedException();
        }

        public static void AddShip(Ship ship)
        {
            Ships.Add(ship);
        }

        public static void RemoveShip(Ship ship)
        {
            Ships.Remove(ship);
        }



    }
}
