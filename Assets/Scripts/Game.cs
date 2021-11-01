using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace GameCoup
{
    class Game : MonoBehaviour
    {
        Random rng = new Random();
        public string NickInput;
        public int InitialHand = 2;
        public int CharacterRepetition = 3;
        public int InitialCoins = 2;
        public List<Player> players = new List<Player>();
        public List<string> deck = new List<string>();
        public int CurrentPlayer;

        // adicionar enum referente as cartas do baralho --TODO--
        // o jogo deve começar apenas com um numero minimo de players --TODO--

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

        public void ReadNickname(string nickname)
        {
            NickInput = nickname;
        }

        // unique name --TODO--
        public void AddingPlayer()
        {
            Player newPlayer = gameObject.AddComponent(typeof(Player)) as Player;
            newPlayer.Nickname = NickInput;
            players.Add(newPlayer);
            Debug.Log("Player added to the game: " + newPlayer.Nickname);
        }

        public void StartGame()
        {
            Debug.Log(players[0].Nickname);
            if (players.Count < 3 || players.Count > 6)
            {
                Debug.Log("O jogo deve ter entre 3 e 6 jogares.");
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
            while (n > 1)
            {
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
            while (n > 1)
            {
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
                Debug.Log(players[i].Nickname + " ");
                for (int j = 0; j < players[i].cards.Count; j++)
                {
                    Debug.Log(players[i].cards[j] + " ");
                }
                for (int j = 0; j < players[i].revealedCards.Count; j++)
                {
                    Debug.Log("*" + players[i].revealedCards[j] + "* ");
                }
                Debug.Log(players[i].Coins);
            }
        }

        public void PrintDeck()
        {
            Debug.Log(string.Format("Deck: ({0}).", string.Join(", ", deck)));
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
            Debug.Log("O jogador " + player.Nickname + " comprou uma moeda.");
            player.AddCoins(1);
        }

        public void ForeignAid(Player player)
        {
            Debug.Log("O jogador " + player.Nickname + " quer ajuda externa.");
            Debug.Log("Quem vai bloquear?");
            string blocker = Console.ReadLine();
            bool success;

            if (blocker == "")
            {
                Debug.Log("O jogador " + player.Nickname + " comprou duas moedas.");
                player.AddCoins(2);
            }
            else
            {
                Player claimer = GetPlayer(blocker);
                Player challenger = AskForChallenger(claimer, "Doke");
                if (challenger != null)
                {
                    success = Challenge(claimer, challenger, "Doke");
                }
                else
                {
                    success = true;
                }
                if (success)
                {
                    player.AddCoins(2);
                }
            }
        }

        public void AssassinAction(Player player)
        {
            Debug.Log("Quem será o alvo?");
            Player target = GetPlayer(Console.ReadLine());
            player.AddCoins(-3);
            Debug.Log("O jogador " + player.Nickname + " quer assassinar " + target.Nickname + ".");
            Player challenger = AskForChallenger(player, "Assassin");
            bool success;
            if (challenger != null)
            {
                success = Challenge(player, challenger, "Assassin");
            }
            else
            {
                success = false;
            }
            if (!success)
            {
                Debug.Log("O jogador alvejado é a Condessa?");
                string contessa = Console.ReadLine();
                if (contessa == "n")
                {
                    Debug.Log(target.Nickname + " deve escolher uma carta para perder.");
                    string lostCard = Console.ReadLine();
                    target.loseCard(lostCard);
                    target.placeTable(lostCard);
                    if (IsGameOver())
                    {
                        Debug.Log("A partida acabou");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    challenger = AskForChallenger(target, "Contessa");
                    if (challenger != null)
                    {
                        success = Challenge(target, challenger, "Contessa");
                    }
                    else
                    {
                        success = false;
                    }
                    if (success)
                    {
                        Debug.Log(target.Nickname + " deve escolher uma carta para perder.");
                        string lostCard = Console.ReadLine();
                        target.loseCard(lostCard);
                        target.placeTable(lostCard);
                        if (IsGameOver())
                        {
                            Debug.Log("A partida acabou");
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }

        public void DokeAction(Player player)
        {
            Player challenger = AskForChallenger(player, "Doke");
            bool success;
            if (challenger != null)
            {
                success = Challenge(player, challenger, "Doke");
            }
            else
            {
                success = false;
            }
            if (!success)
            {
                player.AddCoins(3);
            }
        }

        public void CaptainAction(Player player)
        {
            Debug.Log("Quem será o alvo?");
            Player target = GetPlayer(Console.ReadLine());
            Debug.Log("O jogador " + player.Nickname + " quer extorquir " + target.Nickname + ".");
            Player challenger = AskForChallenger(player, "Captain");
            bool success;
            if (challenger != null)
            {
                success = Challenge(player, challenger, "Captain");
            }
            else
            {
                success = false;
            }
            if (!success)
            {
                Debug.Log("O jogador alvejado é Capitão ou Embaixador?");
                string blocker = Console.ReadLine();
                if (blocker == "")
                {
                    Debug.Log(target.Nickname + " foi extorquido.");
                    int lostCoins = Math.Min(target.Coins, 2);
                    target.AddCoins(-lostCoins);
                    player.AddCoins(lostCoins);
                }
                else
                {
                    challenger = AskForChallenger(target, blocker);
                    if (challenger != null)
                    {
                        success = Challenge(target, challenger, blocker);
                    }
                    else
                    {
                        success = false;
                    }
                    if (success)
                    {
                        Debug.Log(target.Nickname + " foi extorquido.");
                        int lostCoins = Math.Min(target.Coins, 2);
                        target.AddCoins(-lostCoins);
                        player.AddCoins(lostCoins);
                    }
                }
            }
        }

        public void AmbassadorAction(Player player)
        {
            Player challenger = AskForChallenger(player, "Ambassador");
            bool success;
            if (challenger != null)
            {
                success = Challenge(player, challenger, "Ambassador");
            }
            else
            {
                success = false;
            }
            if (!success)
            {
                DrawCard(2, player);
                PrintPlayers();
                PrintDeck();
                Debug.Log(player.Nickname + ", retorne a primeira carta ao baralho.");
                string returnedCard = Console.ReadLine();
                player.loseCard(returnedCard);
                deck.Add(returnedCard);
                Debug.Log(player.Nickname + ", retorne a segunda carta ao baralho.");
                returnedCard = Console.ReadLine();
                player.loseCard(returnedCard);
                deck.Add(returnedCard);
            }
        }

        public void Coup(Player player)
        {
            player.AddCoins(-7);
            Debug.Log("Quem será o alvo?");
            Player target = GetPlayer(Console.ReadLine());
            Debug.Log("O jogador " + target.Nickname + " sofreu um golpe de estado do jogador " + player.Nickname);
            Debug.Log(target.Nickname + " deve descartar uma carta.");
            string DiscardedCard = Console.ReadLine();
            target.loseCard(DiscardedCard);
            target.placeTable(DiscardedCard);
            if (IsGameOver())
            {
                Debug.Log("A partida acabou");
                Environment.Exit(0);
            }
        }

        public Player AskForChallenger(Player claimer, string claimedCard)
        {
            Debug.Log("O jogador " + claimer.Nickname + " diz ser " + claimedCard + ".");
            Debug.Log("Quem vai duvidar?");
            string challenger = Console.ReadLine();
            if (challenger == "")
            {
                Debug.Log("Ninguém desafiou.");
                return null;
            }
            return GetPlayer(challenger);
        }

        public bool Challenge(Player claimer, Player challenger, string ClaimedCard)
        {
            Debug.Log(challenger.Nickname + " desafiou.");
            Debug.Log(claimer.Nickname + " deve revelar uma carta.");
            string RevealedCard = Console.ReadLine();
            if (RevealedCard == ClaimedCard)
            {
                Debug.Log(challenger.Nickname + " errou ao duvidar e deve descartar uma carta.");
                string DiscardedCard = Console.ReadLine();
                challenger.loseCard(DiscardedCard);
                challenger.placeTable(DiscardedCard);
                if (IsGameOver())
                {
                    Debug.Log("A partida acabou");
                    Environment.Exit(0);
                }
                claimer.loseCard(RevealedCard);
                deck.Add(RevealedCard);
                DrawCard(1, claimer);
                return false;
            }
            else
            {
                Debug.Log(claimer.Nickname + " mentiu e perdeu sua carta.");
                claimer.loseCard(RevealedCard);
                claimer.placeTable(RevealedCard);
                if (IsGameOver())
                {
                    Debug.Log("A partida acabou");
                    Environment.Exit(0);
                }
            }
            return true;
        }

        public bool IsGameOver()
        {
            int count = 0;
            string winner = "";
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].isAlive)
                {
                    count++;
                    winner = players[i].Nickname;
                }
            }
            if (count == 1)
            {
                Debug.Log("O jogador " + winner + " é o vencedor");
                return true;
            }
            return false;
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
            else if (action == "stl")
            {
                CaptainAction(subject);
            }
            else if (action == "exc")
            {
                AmbassadorAction(subject);
            }
            else if (action == "cde")
            {
                Coup(subject);
            }
        }
    }
}