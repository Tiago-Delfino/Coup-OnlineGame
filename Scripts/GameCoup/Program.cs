using System;

namespace GameCoup
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.CreatingPlayers(4);
            game.DealCards();
            game.PrintPlayers();
            game.PrintDeck();
        }
    }
}
