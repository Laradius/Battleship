namespace Battleship
{
    internal class Battleship : Ship
    {

        public Battleship() : base()
        {
            Length = 4;
            ShipType = 'B';
            ShipParts = new ShipPart[Length];
            ConstructShip();

        }



    }
}
