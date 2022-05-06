namespace Battleship
{
    internal class Grid
    {


        public int X { get; private set; }
        public int Y { get; private set; }

        public static List<Grid> GetGrid(int sizeX, int sizeY)
        {



            List<Grid> grids = new();

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {

                    grids.Add(new Grid() { X = x, Y = y });

                }
            }
            return grids;
        }

    }
}


