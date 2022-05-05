namespace Battleship
{
    internal abstract class Ship
    {



        public char ShipType { get; private set; }
        public Position Position { get; private set; }

        public void SetPosition(int x, int y)
        {
            Position.SetPosition(x, y);
        }




    }
}
