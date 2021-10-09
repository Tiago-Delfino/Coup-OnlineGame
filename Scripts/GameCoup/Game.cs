﻿using System;
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
                deck.Add("Assassin");
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
            ShuffleDeck();
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < InitialHand; j++)
                {
                    players[i].cards.Add(deck[0]);
                    deck.RemoveAt(0);
                }
            }
        }

        public void DrawCard(int quantity, Player player)
        {     
            ShuffleDeck();
            for (int i = 0; i < quantity; i++)
            {
                player.cards.Add(deck[0]);
                deck.RemoveAt(0);
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
                for (int j = 0; j < players[i].revealedCards.Count; j++)
                {
                    Console.Write("*" + players[i].revealedCards[j] + "* ");
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
            bool success;

            if (blocker == "")
            {
                Console.WriteLine("O jogador " + player.Nickname + " comprou duas moedas.");
                player.AddCoins(2);
            }
            else
            {
                Player claimer = GetPlayer(blocker);
                Player challenger = AskForChallenger(claimer, "Doke");
                if(challenger != null)
                {
                    success = Challenge(claimer, challenger, "Doke");
                }
                else
                {
                    success = true;
                }
                if(success)
                {
                    player.AddCoins(2);
                }
            }
            
        }

        public void AssassinAction(Player player)
        {
            Console.WriteLine("Quem será o alvo?");
            Player target = GetPlayer(Console.ReadLine());
            player.AddCoins(-3);
            Console.WriteLine("O jogador " + player.Nickname + " quer assassinar " + target.Nickname + ".");
            Player challenger = AskForChallenger(player, "Assassin");
            bool success;
            if(challenger != null)
            {
                success = Challenge(player, challenger, "Assassin");
            }
            else
            {
                success = false;
            }
            if(!success)
            {
                Console.WriteLine("O jogador alvejado é a Condessa?");
                string contessa = Console.ReadLine();
                if(contessa == "n")
                {
                    Console.WriteLine(target.Nickname + " deve escolher uma carta para perder.");
                    string lostCard = Console.ReadLine();
                    target.loseCard(lostCard);
                    target.placeTable(lostCard);
                }
                else
                {
                    challenger = AskForChallenger(target, "Contessa");
                    if(challenger != null)
                    {
                        success = Challenge(target, challenger, "Contessa");
                    }
                    else
                    {
                        success = false;
                    }
                    if(success)
                    {
                        Console.WriteLine(target.Nickname + " deve escolher uma carta para perder.");
                        string lostCard = Console.ReadLine();
                        target.loseCard(lostCard);
                        target.placeTable(lostCard);
                    }
                }
            }
        }

        public void DokeAction(Player player)
        {
            Player challenger = AskForChallenger(player, "Doke");
            bool success;
            if(challenger != null)
            {
                success = Challenge(player, challenger, "Doke");
            }
            else
            {
                success = false;
            }
            if(!success)
            {
                player.AddCoins(3);
            }
        }

        public Player AskForChallenger(Player claimer, string claimedCard)
        {
            Console.WriteLine("O jogador " + claimer.Nickname + " diz ser " + claimedCard + ".");
            Console.WriteLine("Quem vai duvidar?");
            string challenger = Console.ReadLine();
            if(challenger == "")
            {
                Console.WriteLine("Ninguém desafiou.");
                return null;
            }
            return GetPlayer(challenger);
        }

        public bool Challenge(Player claimer, Player challenger, string ClaimedCard)
        {
            Console.WriteLine(challenger.Nickname + " desafiou.");
            Console.WriteLine(claimer.Nickname + " deve revelar uma carta.");
            string RevealedCard = Console.ReadLine();
            if (RevealedCard == ClaimedCard)
            {
                Console.WriteLine(challenger.Nickname + " errou ao duvidar e deve descartar uma carta.");
                string DiscardedCard = Console.ReadLine();
                challenger.loseCard(DiscardedCard);
                challenger.placeTable(DiscardedCard);
                claimer.loseCard(RevealedCard);
                deck.Add(RevealedCard);
                DrawCard(1, claimer);
                return false;
            }
            else
            {
                Console.WriteLine(claimer.Nickname + " mentiu e perdeu sua carta.");
                claimer.loseCard(RevealedCard);
                claimer.placeTable(RevealedCard);
            }
            return true;
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
            else if (action == "tax")
            {
                DokeAction(subject);
            }
            else if (action == "ass")
            {
                AssassinAction(subject);
            }
        }
    }
}
