namespace Battleship
{
    internal class ShipPart
    {
        public bool IsDestroyed { get; private set; } = false;
        public Ship Ship { get; private set; }
        public Position? Position { get; private set; }


        public ShipPart(Ship ship)
        {

            this.Ship = ship;
        }


        public void SetPosition(Position pos)
        {


            this.Position = pos;



            pos.Take(this);
        }
        public void Destroy()
        {
            IsDestroyed = true;
        }


    }
}