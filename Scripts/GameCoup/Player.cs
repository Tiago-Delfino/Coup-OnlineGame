using System;
using System.Collections.Generic;
using System.Text;

namespace GameCoup
{
    class Player
    {
        public string Nickname { get; private set; }
        public int Coins;
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
    }
}
