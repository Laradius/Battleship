namespace Battleship
{
    internal class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }


        public bool IsTaken { get; private set; } = false;
        public bool IsShot { get; private set; }

        public ShipPart? Occupant { get; private set; }

        public void Take(ShipPart ship)
        {

            if (IsTaken)
            {
                Console.WriteLine("Ship inside ship");
            }
            this.Occupant = ship;
            IsTaken = true;
        }



        public void Shoot()
        {
            if (IsTaken)
            {
                Occupant!.Destroy();
            }
            IsShot = true;
        }
        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }


        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

    }
}
