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

        public static Random Rand = new();





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
                        world1 += p.Occupant!.Ship.ShipType;
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
                        world2 += p.Occupant!.Ship.ShipType;
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

            Position? position = positions.Find(p => p.X == x && p.Y == y);

            if (position == null)
            {
                throw new InvalidOperationException("Position is out of world bounds." + x + y);
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




        public static bool AddShipToWorld(bool world1, Ship ship)
        {

            bool canAddShip = false;







            while (!canAddShip && ship.ChangeOrientation())
            {

                List<Grid> grid = Grid.GetGrid(WorldSizeX, WorldSizeY);

                Grid possibleElement;

                while (grid.Count > 0 && !canAddShip)
                {

                    possibleElement = grid[Rand.Next(grid.Count)];

                    Position pos = GetPosition(possibleElement.X, possibleElement.Y, world1);

                    if (ship.Orientation == CreationOrientation.Left)
                    {
                        if (!OutOfBounds(pos.X - ship.Length, pos.Y))
                        {

                            for (int i = 0; i < ship.Length; i++)
                            {
                                ship.ShipParts[i].SetPosition(pos.X - i, pos.Y);

                                if (pos.X - i < 0)
                                {
                                    throw new Exception();
                                }
                            }

                            canAddShip = true;
                            break;
                        }
                        else
                        {
                            grid.Remove(possibleElement);
                            continue;
                        }
                    }
                    else if (ship.Orientation == CreationOrientation.Right)
                    {
                        if (!OutOfBounds(pos.X + ship.Length, pos.Y))
                        {
                            for (int i = 0; i < ship.Length; i++)
                            {
                                if (pos.X + i > 9)
                                {
                                    throw new Exception();
                                }
                                ship.ShipParts[i].SetPosition(pos.X + i, pos.Y);
                            }

                            canAddShip = true;
                            break;
                        }
                        else
                        {
                            grid.Remove(possibleElement);
                            continue;
                        }
                    }
                    else if (ship.Orientation == CreationOrientation.Top)
                    {

                        if (!OutOfBounds(pos.X, pos.Y - ship.Length))
                        {
                            for (int i = 0; i < ship.Length; i++)
                            {
                                if (pos.Y - i < 0)
                                {
                                    throw new Exception();
                                }
                                ship.ShipParts[i].SetPosition(pos.X, pos.Y - i);
                            }

                            canAddShip = true;
                            break;
                        }
                        else
                        {
                            grid.Remove(possibleElement);
                            continue;
                        }

                    }
                    else
                    {
                        if (!OutOfBounds(pos.X, pos.Y + ship.Length))
                        {
                            for (int i = 0; i < ship.Length; i++)
                            {
                                if (pos.Y + i < 0)
                                {
                                    throw new Exception();
                                }
                                ship.ShipParts[i].SetPosition(pos.X, pos.Y + i);
                            }

                            canAddShip = true;
                            break;
                        }
                        else
                        {
                            grid.Remove(possibleElement);
                            continue;
                        }
                    }


                }
            }
            if (canAddShip)
            {
                AddShip(ship, world1);
                return true;
            }
            return false;
        }

        public static bool OutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0 || x > WorldSizeX - 1 || y > WorldSizeY - 1)
            {
                return true;
            }
            return false;
        }



        private static void AddShip(Ship ship, bool world1)
        {



            for (int i = 0; i < ship.ShipParts.Length; i++)
            {
                GetPosition(ship.ShipParts[i].Position!.X, ship.ShipParts[i].Position!.Y, world1).Take(ship.ShipParts[i]);
            }




            Ships.Add(ship);
        }

        public static void RemoveShip(Ship ship)
        {
            Ships.Remove(ship);
        }
    }
}
