namespace Battleship
{
    internal class PatrolBoat : Ship
    {

        public PatrolBoat() : base()
        {
            Length = 2;
            ShipType = 'P';
            ShipParts = new ShipPart[Length];
            ConstructShip();

        }



    }
}
