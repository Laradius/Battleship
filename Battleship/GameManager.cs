namespace Battleship
{
    internal static class GameManager
    {


        private static bool _isInitialized = false;
        public static int WorldSizeX { get; private set; }
        public static int WorldSizeY { get; private set; }

        public static List<Ship> Ships { get; private set; } = new List<Ship>();

        public static List<List<Position>> Worlds { get; private set; } = new List<List<Position>>()
        { new List<Position>(), new List<Position>() };

        public static Random Rand = new();





        public static void MakeWorlds(int sizeX, int sizeY)
        {

            if (!_isInitialized)
            {

                WorldSizeX = sizeX;
                WorldSizeY = sizeY;

                for (int i = 0; i < Worlds.Count; i++)
                    for (int y = 0; y < WorldSizeY; y++)
                    {
                        for (int x = 0; x < WorldSizeX; x++)
                        {
                            Worlds[i].Add(new Position(x, y));
                        }
                    }

                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Worlds are already initialized.");
            }
        }


        public static string DrawWorlds()
        {

            if (!_isInitialized)
            {
                throw new InvalidOperationException("Wolrd is not initialized.");
            }

            string worlds = "";





            for (int i = 0; i < Worlds.Count; i++)
            {
                worlds += $"World {i + 1}: {Environment.NewLine}";
                for (int x = 0; x < WorldSizeX; x++)
                {
                    for (int y = 0; y < WorldSizeY; y++)
                    {


                        Position p = GetPosition(x, y, i);


                        if (p.IsTaken)
                        {
                            worlds += p.Occupant.Ship.ShipType;
                        }

                        else if (p.IsShot)
                        {
                            worlds += "#";
                        }

                        else
                        {
                            worlds += "*";
                        }



                    }
                    worlds += Environment.NewLine;
                }
                worlds += Environment.NewLine;

            }

            worlds += $"Ship count: {Ships.Count} {Environment.NewLine}";

            int shipCount = 0;

            foreach (Ship ship in Ships)
            {
                foreach (ShipPart p in ship.ShipParts)
                {
                    shipCount++;
                }
            }

            worlds += $"Remaining ShipParts: {shipCount}";

            return worlds;
        }

        public static Position GetPosition(int x, int y, int world)
        {


            Position? position = Worlds[world].Find(p => p.X == x && p.Y == y);

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




        public static bool AddShipToWorld(int world, Ship ship)
        {

            bool canAddShip = false;







            while (!canAddShip && ship.ChangeOrientation())
            {

                List<Grid> grid = Grid.GetGrid(WorldSizeX, WorldSizeY);

                Grid possibleElement;

                while (grid.Count > 0 && !canAddShip)
                {

                    possibleElement = grid[Rand.Next(grid.Count)];

                    Position pos = GetPosition(possibleElement.X, possibleElement.Y, world);

                    if (ship.Orientation == CreationOrientation.Left)
                    {
                        if (!pos.IsTaken && !OutOfBounds(pos.X - ship.Length, pos.Y) && !ScanForNeighbour(world, ship, pos, ship.Orientation))
                        {


                            //Get [Right, Top, Bottom], [Top, Bottom] [Top, Bottom, Left]


                            for (int i = 0; i < ship.Length; i++)
                            {
                                ship.ShipParts[i].SetPosition(GetPosition(pos.X - i, pos.Y, world));

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
                        if (!pos.IsTaken && !OutOfBounds(pos.X + ship.Length, pos.Y) && !ScanForNeighbour(world, ship, pos, ship.Orientation))
                        {
                            for (int i = 0; i < ship.Length; i++)
                            {
                                if (pos.X + i > 9)
                                {
                                    throw new Exception();
                                }
                                ship.ShipParts[i].SetPosition(GetPosition(pos.X + i, pos.Y, world));
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

                        if (!pos.IsTaken && !OutOfBounds(pos.X, pos.Y - ship.Length) && !ScanForNeighbour(world, ship, pos, ship.Orientation))
                        {
                            for (int i = 0; i < ship.Length; i++)
                            {
                                if (pos.Y - i < 0)
                                {
                                    throw new Exception();
                                }
                                ship.ShipParts[i].SetPosition(GetPosition(pos.X, pos.Y - i, world));
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
                        if (!pos.IsTaken && !OutOfBounds(pos.X, pos.Y + ship.Length) && !ScanForNeighbour(world, ship, pos, ship.Orientation))
                        {
                            for (int i = 0; i < ship.Length; i++)
                            {
                                if (pos.Y + i < 0)
                                {
                                    throw new Exception();
                                }
                                ship.ShipParts[i].SetPosition(GetPosition(pos.X, pos.Y + i, world));
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
                AddShip(ship, world);
                return true;
            }
            Console.WriteLine("didint add ship");
            return false;
        }

        private static bool ScanForNeighbour(int world, Ship ship, Position pos, CreationOrientation orientation)
        {
            bool neighbourFound = false;


            if (orientation == CreationOrientation.Right)
            {


                for (int i = 0; i < ship.Length + 2; i++)
                {
                    //Scan Bottom

                    if (!OutOfBounds(pos.X - 1 + i, pos.Y - 1))
                    {


                        if (GetPosition(pos.X - 1 + i, pos.Y - 1, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }

                    //Scan Top


                    if (!OutOfBounds(pos.X - 1 + i, pos.Y + 1))
                    {



                        if (GetPosition(pos.X - 1 + i, pos.Y + 1, world).IsTaken)
                        {
                            neighbourFound = true;
                        }
                    }

                }
                // Scan Left
                if (!OutOfBounds(pos.X - 1, pos.Y))
                {

                    if (GetPosition(pos.X - 1, pos.Y, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }
                // Scan Right
                if (!OutOfBounds(pos.X + ship.Length, pos.Y))
                {


                    if (GetPosition(pos.X + ship.Length, pos.Y, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }


            }

            else if (orientation == CreationOrientation.Left)
            {

                for (int i = 0; i < ship.Length + 2; i++)
                {
                    //Scan Bottom

                    if (!OutOfBounds(pos.X + 1 - i, pos.Y - 1))
                    {




                        if (GetPosition(pos.X + 1 - i, pos.Y - 1, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }

                    //Scan Top


                    if (!OutOfBounds(pos.X + 1 - i, pos.Y + 1))
                    {



                        if (GetPosition(pos.X + 1 - i, pos.Y + 1, world).IsTaken)
                        {
                            neighbourFound = true;
                        }
                    }

                }
                // Scan Right
                if (!OutOfBounds(pos.X + 1, pos.Y))
                {


                    if (GetPosition(pos.X + 1, pos.Y, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }
                // Scan Left
                if (!OutOfBounds(pos.X - ship.Length, pos.Y))
                {


                    if (GetPosition(pos.X - ship.Length, pos.Y, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }
            }
            else if (orientation == CreationOrientation.Top)
            {


                for (int i = 0; i < ship.Length + 2; i++)
                {
                    //Scan Right

                    if (!OutOfBounds(pos.X + 1, pos.Y + 1 - i))
                    {




                        if (GetPosition(pos.X + 1, pos.Y + 1 - i, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }

                    //Scan Left


                    if (!OutOfBounds(pos.X - 1, pos.Y + 1 - i))
                    {




                        if (GetPosition(pos.X - 1, pos.Y + 1 - i, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }

                }
                // Scan Top
                if (!OutOfBounds(pos.X, pos.Y + 1))
                {

                    if (GetPosition(pos.X, pos.Y + 1, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }
                // Scan Bottom
                if (!OutOfBounds(pos.X, pos.Y - ship.Length))
                {

                    if (GetPosition(pos.X, pos.Y - ship.Length, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }




            }


            else if (orientation == CreationOrientation.Bottom)
            {


                for (int i = 0; i < ship.Length + 2; i++)
                {
                    //Scan Right

                    if (!OutOfBounds(pos.X + 1, pos.Y - 1 + i))
                    {




                        if (GetPosition(pos.X + 1, pos.Y - 1 + i, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }

                    //Scan Left


                    if (!OutOfBounds(pos.X - 1, pos.Y - 1 + i))
                    {


                        if (GetPosition(pos.X - 1, pos.Y - 1 + i, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }

                }
                // Scan Top
                if (!OutOfBounds(pos.X, pos.Y - 1))
                {


                    if (GetPosition(pos.X, pos.Y - 1, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }
                // Scan Bottom
                if (!OutOfBounds(pos.X, pos.Y + ship.Length))
                {


                    if (GetPosition(pos.X, pos.Y + ship.Length, world).IsTaken)
                    {
                        neighbourFound = true;
                    }
                }




            }


            return neighbourFound;
        }

        public static bool OutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0 || x > WorldSizeX - 1 || y > WorldSizeY - 1)
            {
                return true;
            }
            return false;
        }





        private static void AddShip(Ship ship, int world)
        {
            Ships.Add(ship);
        }

        public static void RemoveShip(Ship ship)
        {
            Ships.Remove(ship);
        }
    }
}
