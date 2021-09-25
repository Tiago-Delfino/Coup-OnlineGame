using System;
using System.Collections.Generic;
using System.Text;

namespace GameCoup
{
    class Game
    {
        public int InitialHand = 2;
        public int CharacterRepetition = 3;
        public List<Player> players = new List<Player>();
        public List<string> deck = new List<string>();

        // adicionar enum referente as cartas do baralho --TODO--
        // o jogo deve começar apenas com um numero minimo de players --TODO--
        public Game()
        {
            CreatingDeck();
        }

        public void CreatingDeck()
        {
            for (int i = 0; i < CharacterRepetition; i++)
            {
                deck.Add("Doke");
                deck.Add("Assasin");
                deck.Add("Captain");
                deck.Add("Ambassador");
                deck.Add("Contessa");
            }
        }

        public void CreatingPlayers(int numberOfPlayers)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                AddingPlayer();
            }
        }

        // unique name --TODO--
        public void AddingPlayer()
        {
            Console.WriteLine("Please enter the nickname: ");
            string nickname = Console.ReadLine();
            Player player = new Player(nickname);
            players.Add(player);
            Console.WriteLine("Player added to the game: " + player.Nickname);
        }

        public void DealCards()
        {

            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < InitialHand; j++)
                {
                    Random random = new Random();
                    int randonInteger = random.Next(0, deck.Count);
                    players[i].cards.Add(deck[randonInteger]);
                    deck.RemoveAt(randonInteger);
                }
            }
        }

        public void PrintPlayers()
        {
            for (int i = 0; i < players.Count; i++)
            {
                Console.Write(players[i].Nickname + " ");
                for (int j = 0; j < 2; j++)
                {
                    Console.Write(players[i].cards[j] + " ");
                }
                Console.WriteLine("----------------");
            }
        }
        public void PrintDeck()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                Console.Write(deck[i] + " ");
            }
        }
    }
}
