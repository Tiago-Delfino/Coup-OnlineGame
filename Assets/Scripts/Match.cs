using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoup
{
    class Match : MonoBehaviour
    {
        Game game;

        public void Start()
        {
            game = gameObject.GetComponent<Game>();
            game.CreatingDeck();
            game.PrintDeck();
        }

        public void StartMatch()
        {
            game.StartGame();
            game.PrintPlayers();
            game.PrintDeck();
        }

        public void Round()
        {
            for (int i = 0; i < game.players.Count; i++)
            {
                if (game.players[i].isAlive)
                {
                    if (game.players[i].Coins >= 10)
                    {
                        Debug.Log("O jogador " + game.players[i].Nickname + " deve dar o coup.");
                        game.resolve(game.players[i], "cde");
                    }
                    else
                    {
                        Debug.Log(game.players[i].Nickname + ", please enter your action: ");
                        string action = Console.ReadLine();
                        game.resolve(game.players[i], action);
                    }
                }
                game.PrintPlayers();
            }
            Round();
        }
    }
}
