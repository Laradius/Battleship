namespace Battleship
{
    internal struct Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

    }
}
