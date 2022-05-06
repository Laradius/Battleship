namespace Battleship
{
    internal class AircraftCarrier : Ship
    {

        public AircraftCarrier() : base()
        {
            Length = 5;
            ShipType = 'A';
            ShipParts = new ShipPart[Length];
            ConstructShip();

        }



    }
}
