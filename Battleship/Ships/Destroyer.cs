namespace Battleship
{
    internal class Destroyer : Ship
    {

        public Destroyer() : base()
        {
            Length = 2;
            ShipType = 'D';
            ShipParts = new ShipPart[Length];
            ConstructShip();

        }



    }
}
