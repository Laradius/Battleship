namespace Battleship
{
    internal abstract class Ship
    {


        public bool Sunk { get; protected set; } = false;

        public int Length { get; protected set; }
        public char ShipType { get; protected set; }

        public ShipPart[] ShipParts { get; protected set; } = null!;

        public CreationOrientation Orientation { get; protected set; }

        public List<CreationOrientation> CreationOrientations { get; protected set; }



        public Ship()
        {

            CreationOrientations = Enum.GetValues(typeof(CreationOrientation)).Cast<CreationOrientation>().ToList();



        }


        public void RemoveShipPart(ShipPart part)
        {

            ShipParts = ShipParts.Where((p) => p.Position != part.Position).ToArray();


        }

        public bool ChangeOrientation()
        {
            if (CreationOrientations.Count > 0)
            {
                Orientation = CreationOrientations[GameManager.Rand.Next(CreationOrientations.Count)];
                CreationOrientations.Remove(Orientation);
                return true;
            }
            else
            {
                return false;
            }
        }


        public virtual void KillShip()
        {
            Sunk = true;

        }
        protected virtual void ConstructShip()
        {

            for (int i = 0; i < Length; i++)
            {
                ShipParts[i] = new ShipPart(this);

            }

        }




    }
}
