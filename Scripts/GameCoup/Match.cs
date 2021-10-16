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
                        if (game.players[i].Coins >= 10)
                        {
                            Console.WriteLine("O jogador " + game.players[i].Nickname + " deve dar o coup.");
                            game.resolve(game.players[i], "cde");
                        }
                        else
                        {
                            Console.WriteLine(game.players[i].Nickname + ", please enter your action: ");
                            string action = Console.ReadLine();
                            game.resolve(game.players[i], action);
                        }
                    }
                    game.PrintPlayers();
                }
            }
        }
    }
}
