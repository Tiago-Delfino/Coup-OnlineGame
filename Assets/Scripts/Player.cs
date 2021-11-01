using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoup
{
    class Player : MonoBehaviour
    {
        public string Nickname;
        public int Coins;
        public bool isAlive;
        public List<string> cards = new List<string>();
        public List<string> revealedCards = new List<string>();
        //public bool Religion;

        public void ChangeName(string nickname)
        {
            Nickname = nickname;
        }

        public void AddCoins(int quantity)
        {
            Coins += quantity;
        }

        public void loseCard(string card)
        {
            cards.Remove(card);
        }

        public void UpdateLife()
        {
            if (revealedCards.Count == 2)
            {
                isAlive = false;
                Debug.Log("O jogador " + Nickname + " está fora do jogo.");
            }
        }

        public void placeTable(string card)
        {
            revealedCards.Add(card);
            UpdateLife();
        }

    }
}
