namespace Battleship
{
    internal class ShipPart
    {

        public Ship Ship { get; private set; }
        public Position Position { get; private set; }


        public ShipPart(Ship ship)
        {
            this.Ship = ship;
        }
        public void SetPosition(int x, int y)
        {
            Position.SetPosition(x, y);
        }
    }
}