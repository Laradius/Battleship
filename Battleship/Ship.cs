namespace Battleship
{
    internal abstract class Ship
    {


        public char ShipType { get; private set; }

        public ShipPart[] ShipParts;

        public abstract void CreateShip();



    }
}
