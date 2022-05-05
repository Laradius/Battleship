namespace Battleship
{
    internal class ShipPart
    {
        public bool IsDestroyed { get; private set; } = false;
        public Ship Ship { get; private set; }
        public Position Position { get; private set; }


        public ShipPart(Ship ship)
        {
            this.Ship = ship;
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        public void SetPosition(int x, int y)
        {
            Position.SetPosition(x, y);
        }
    }
}