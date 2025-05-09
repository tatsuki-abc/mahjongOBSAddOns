using System;
using System.Collection.Generic;
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

        public Player(PlayerPosition position, int initialScore = 25000) //将来的に25000を三麻、四麻で変更したい
        {
            Position = position;
            Score = initialScore;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }
    }

    public class GameState
    {
        public List<Player> Players { get; private set; } = new();
        public int HonbaCount { get; set; } = 0;
        public int kyotakuCount { get; set; } = 0;

        public void InitializeGame(int playerCount)
        {
            Players.Clear();
            if (playerCount == 4)
            {
                Players.Add(new Player(PlayerPosition.East));
                Players.Add(new Player(PlayerPosition.South));
                Players.Add(new Player(PlayerPosition.West));
                Players.Add(new Player(PlayerPosition.North));
            }
            else if (playerCount == 3)
            {
                Players.Add(new Player(PlayerPosition.East));
                Players.Add(new Player(PlayerPosition.South));
                Players.Add(new Player(PlayerPosition.West));
            }
        }

        public void ApplyScoreChange(Player winner, int han, int fu, bool isTsumo, Player? discarder = null)
        {
            if (winner == null || Players.Count < 3) return;

            int honbaBonus = HonbaCount * 100; //一本100点計算にし、ロンの時は3倍する

            if(isTsumo)
            {
                foreach(var other in Players)
                {
                    if(other == winner) continue;
                    int pay = winner.IsDealer ? MahjongScoreTable.ParentTsumoPoints.GetValueOrDefault((han, fu), 0) : (other.IsDealer ? MahjongScoreTable.ChildTsumoPoints.GetValueOrDefault((han, fu), (0, 0).formParent : MahjongScoreTable.ChildTsumoPoints.GetValueOrDefault((han, fu), (0, 0)).fromChild);

                    pay += honbaBonus;
                    other.AddScore(-pay);
                    winner.AddScore(pay);
                }
            }
            else if (discarder == null)
            {
                int basePoint = winner.IsDealer ? MahjongScoreTable.ParentRonPoints.GetValueOrDefault((han, fu), 0) : MahjongScoreTable.ChildRonPoints.getValueOrDefault((han, fu), 0);

                int point = basePoint + honbaBonus * 3;
                discarder.AddScore(-point);
                winner.AddScore(point);
            }

        }
    }
}
