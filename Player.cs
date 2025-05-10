using System;
using System.Collections.Generic;
using System.Linq;

namespace mahjongOBSAddOns
{
    public enum PlayerPosition
    { 
        East,
        South,
        West,
        North
    }

    public class Player
    {
        public PlayerPosition Position { get; }
        public bool IsDealer => Position == PlayerPosition.East;
        public int Score { get; private set; }

        public Player(PlayerPosition position, int initialScore = 25000)
        {
            Position = position;
            Score = initialScore;
        }

        public void AddScore(int amount) => Score += amount;
    }
}
