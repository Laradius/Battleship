namespace Battleship
{
    internal class Cruiser : Ship
    {

        public Cruiser() : base()
        {
            Length = 3;
            ShipType = 'P';
            ShipParts = new ShipPart[Length];
            ConstructShip();

        }



    }
}
