using Battleship;

GameManager.MakeWorlds(10, 10);


while (true)
{
    Console.WriteLine(GameManager.DrawWorlds());
    Console.ReadKey();
    GameManager.MakeTurn();
    if (GameManager.GameOver)
    {
        break;
    }
}
Console.ReadKey();
