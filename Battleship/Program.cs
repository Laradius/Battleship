using Battleship;

GameManager.MakeWorlds(10, 10);


for (int i = 0; i < 10; i++)
    GameManager.AddShipToWorld(false, new PatrolBoat());

Console.WriteLine(GameManager.DrawWorlds());
