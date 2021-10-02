using System;
using System.Collections.Generic;
using System.Text;

namespace GameCoup
{
    class Game
    {
        Random rng = new Random();
        public int InitialHand = 2;
        public int CharacterRepetition = 3;
        public int InitialCoins = 2;
        public List<Player> players = new List<Player>();
        public List<string> deck = new List<string>();
        public int CurrentPlayer;

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

        public void StartGame()
        {
            if (players.Count < 3 || players.Count > 6)
            {
                Console.WriteLine("O jogo deve ter entre 3 e 6 jogares.");
            }
            else 
            {
                SetOrder();
                Revive();
                GiveCoins();
                DealCards();
            }

            
        }


        public void SetOrder()
        {
            int n = players.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                Player value = players[k];  
                players[k] = players[n];  
                players[n] = value;   
            }
            CurrentPlayer = 0;
        }

        public void ShuffleDeck()
        {
            int n = deck.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                string value = deck[k];  
                deck[k] = deck[n];  
                deck[n] = value;   
            }
        }

        public void Revive()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].isAlive = true;
            }
        }

        public void GiveCoins()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].AddCoins(2);
            }
        }

        public void DealCards()
        {

            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < InitialHand; j++)
                {

                    int randonInteger = rng.Next(0, deck.Count);
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
                for (int j = 0; j < players[i].cards.Count; j++)
                {
                    Console.Write(players[i].cards[j] + " ");
                }
                Console.Write(players[i].Coins);
                Console.WriteLine("----------------");
            }
        }
        public void PrintDeck()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                Console.Write(deck[i] + " ");
            }
            Console.WriteLine();
        }

        public void Income(Player player)
        {
            Console.WriteLine("O jogador " + player.Nickname + " comprou uma moeda.");
            player.AddCoins(1);
        }

        public void ForeignAid(Player player)
        {
            Console.WriteLine("O jogador " + player.Nickname + " quer ajuda externa.");
            Console.WriteLine("Quem vai bloquear?");
            string blocker = Console.ReadLine();

            if (blocker == "")
            {
                Console.WriteLine("O jogador " + player.Nickname + " comprou duas moedas.");
                player.AddCoins(2);
            }
            else
            {
                Player claimer = GetPlayer(blocker);
                Challenge(claimer,"Doke");
            }
            
        }

        public Player GetPlayer(string nickname)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Nickname == nickname)
                {
                    return players[i];
                }

            }
            return null;
        }

        public void Challenge(Player claimer, string ClaimedCard)
        {
            Console.WriteLine("O jogador " + claimer.Nickname + " diz ser " + ClaimedCard + ".");
            Console.WriteLine("Quem vai duvidar?");
            string challenger = Console.ReadLine();

            if (challenger == "")
            {
                Console.WriteLine("Ninguém desafiou.");
            }
            else
            {
                Console.WriteLine(challenger + "desafiou.");
                Console.WriteLine(claimer.Nickname + "deve revelar uma carta.");
                string ReveleadCard = Console.ReadLine();
                if (ReveleadCard == ClaimedCard)
                {
                    Console.WriteLine(challenger + " errou ao duvidar e deve descartar uma carta.");
                    Player loser = GetPlayer(challenger);
                    string DiscardedCard = Console.ReadLine();
                    loser.cards.Remove(DiscardedCard);
                    deck.Add(DiscardedCard);
                    ShuffleDeck();
                }
                else
                {
                    Console.WriteLine(claimer.Nickname + "mentiu e perdeu sua carta.");
                    claimer.cards.Remove(ReveleadCard);
                    deck.Add(ReveleadCard);
                    ShuffleDeck();
                    
                }
            }
        }

        public void resolve(Player subject, string action)
        {
            if (action == "inc")
            {
                Income(subject);
            } 
            else if (action == "aid")
            {
                ForeignAid(subject);
            }
        }
    }
}
