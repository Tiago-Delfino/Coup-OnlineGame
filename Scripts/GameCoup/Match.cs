using System;

namespace GameCoup
{
    class Match
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.CreatingPlayers(4);
            game.StartGame();
            game.PrintPlayers();
            game.PrintDeck();

            while (true)
            {
                for (int i = 0; i < game.players.Count; i++)
                {
                    if (game.players[i].isAlive)
                    {
                        Console.WriteLine( game.players[i].Nickname + ", please enter your action: ");
                        string action = Console.ReadLine();
                        game.resolve(game.players[i],action);
                    }
                    game.PrintPlayers();

                }
                break;
            }
        }
    }
}
