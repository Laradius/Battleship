using Battleship;

GameManager.MakeWorlds(10, 10);


for (int i = 0; i < 10; i++)
    GameManager.AddShipToWorld(0, new PatrolBoat());


string world = GameManager.DrawWorlds();

Console.Write(world);
