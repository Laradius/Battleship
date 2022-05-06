namespace Battleship
{
    internal static class GameManager
    {


        public static bool GameOver { get; private set; }

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


                PopulateWorlds();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Worlds are already initialized.");
            }
        }




        public static string DrawWorlds()
        {

            Console.Clear();

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



                        if (p.IsTaken && !p.Occupant!.IsDestroyed)
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


            int worldNumber = 0;
            foreach (List<Position> pos in Worlds)
            {

                List<Position> takenPos = pos.FindAll((s) => s.IsTaken);
                List<Ship> ships = new List<Ship>();

                for (int i = 0; i < takenPos.Count; i++)
                {
                    ships.Add(takenPos[i].Occupant!.Ship);
                }

                ships = ships.Distinct().ToList();

                ships.RemoveAll((s) => s.Sunk);



                worlds += $"Ship count for world {worldNumber++ + 1}: {ships.Count} {Environment.NewLine}";

                if (ships.Count < 1)
                {
                    GameOver = true;
                    return $"Game over, World{worldNumber} has lost. Press any key to exit.";
                }
            }

            worlds += $"Total Ship count: {Ships.Where((s) => !s.Sunk).ToList().Count} {Environment.NewLine}";


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
            for (int i = 0; i < Worlds.Count; i++)
            {
                List<Position> positions = Worlds[i].FindAll((p) => !p.IsShot);

                positions[Rand.Next(positions.Count)].Shoot();
            }
        }


        private static void PopulateWorlds()
        {
            for (int i = 0; i < Worlds.Count; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    GameManager.AddShipToWorld(i, new Submarine());
                    GameManager.AddShipToWorld(i, new Destroyer());
                }

                GameManager.AddShipToWorld(i, new Cruiser());
                GameManager.AddShipToWorld(i, new Battleship());
                GameManager.AddShipToWorld(i, new AircraftCarrier());
            }
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

                    if (pos.IsTaken)
                    {
                        grid.Remove(possibleElement);
                        continue;
                    }

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
                AddShip(ship);
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


                for (int i = 0; i < ship.Length; i++)
                {
                    // Scan inside;

                    if (!OutOfBounds(pos.X + i, pos.Y))
                    {


                        if (GetPosition(pos.X + i, pos.Y, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }
                }

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


                for (int i = 0; i < ship.Length; i++)
                {
                    // Scan inside;

                    if (!OutOfBounds(pos.X - i, pos.Y))
                    {




                        if (GetPosition(pos.X - i, pos.Y, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }
                }


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

                // Scan inside
                for (int i = 0; i < ship.Length; i++)
                {
                    if (!OutOfBounds(pos.X, pos.Y - i))
                    {




                        if (GetPosition(pos.X, pos.Y - i, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }
                }

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


                // Scan inside
                for (int i = 0; i < ship.Length; i++)
                {
                    if (!OutOfBounds(pos.X, pos.Y + i))
                    {




                        if (GetPosition(pos.X, pos.Y + i, world).IsTaken)
                        {
                            neighbourFound = true;
                        }

                    }
                }

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





        private static void AddShip(Ship ship)
        {
            Ships.Add(ship);
        }

        public static void RemoveShip(Ship ship)
        {
            Ships.Remove(ship);
        }
    }
}
