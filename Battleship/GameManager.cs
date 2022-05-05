namespace Battleship
{
    internal static class GameManager
    {
        public static List<Ship> Ships { get; private set; } = new List<Ship>();


        private static string MakeWorlds()
        {
            throw new NotImplementedException();
        }

        public static void MakeTurn()
        {
            throw new NotImplementedException();
        }

        public static void AddShip(Ship ship)
        {
            Ships.Add(ship);
        }

        public static void RemoveShip(Ship ship)
        {
            Ships.Remove(ship);
        }



    }
}
