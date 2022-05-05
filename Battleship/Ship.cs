namespace Battleship
{
    internal abstract class Ship
    {


        public char ShipType { get; private set; }

        public ShipPart[] ShipParts { get; private set; }

        public abstract void CreateShip();



    }
}
