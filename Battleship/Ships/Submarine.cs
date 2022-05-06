namespace Battleship
{
    internal class Submarine : Ship
    {

        public Submarine() : base()
        {
            Length = 1;
            ShipType = 'S';
            ShipParts = new ShipPart[Length];
            ConstructShip();

        }



    }
}
