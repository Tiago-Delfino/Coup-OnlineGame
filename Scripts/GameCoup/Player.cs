using System;
using System.Collections.Generic;
using System.Text;

namespace GameCoup
{
    class Player
    {
        public string Nickname { get; private set; }
        public int Coins;
        public bool isAlive;
        public List<string> cards = new List<string>();
        //public bool Religion;
        public Player(string nickname)
        {
            Nickname = nickname;
        }

        public void ChangeName(string nickname)
        {
            Nickname = nickname;
        } 

        public void AddCoins(int quantity)
        {
            Coins += quantity;
        } 

        public void Income()
        {
            AddCoins(1);
        }
        
    }
}
