using System;
using System.Collections.Generic;
using System.Linq;

namespace mahjongOBSAddOns
{
    public class GameState
    {
        public List<Player> Players { get; private set; } = new();
        public int HonbaCount { get; set; } = 0;
        public int KyotakuCount { get; set; } = 0;

        public void InitializeGame(int playerCount)
        {
            Players.Clear();
            var positions = playerCount == 4
                ? new[] { PlayerPosition.East, PlayerPosition.South, PlayerPosition.West, PlayerPosition.North }
                : new[] { PlayerPosition.East, PlayerPosition.South, PlayerPosition.West };

            foreach (var position in positions)
            {
                Players.Add(new Player(position));
            }
        }

        public void ApplyScoreChange(Player winner, int han, int fu, bool isTsumo, Player? discarder = null)
        {
            if (Players.Count < 3) return;

            int honbaBonus = HonbaCount * 100;

            if (isTsumo)
            {
                foreach (var other in Players)
                {
                    if (other == winner) continue;

                    int pay = 0;

                    if (winner.IsDealer)
                    {
                        pay = MahjongScoreTable.ParentTsumoPoints.TryGetValue((han, fu), out var value) ? value : 0;
                    }
                    else
                    {
                        var (fromParent, fromChild) = MahjongScoreTable.ChildTsumoPoints.TryGetValue((han, fu), out var tuple) ? tuple : (0, 0);
                        pay = other.IsDealer ? fromParent : fromChild;
                    }

                    pay += honbaBonus;
                    other.AddScore(-pay);
                    winner.AddScore(pay);
                }
            }
            else if (discarder != null)
            {
                int basePoint = winner.IsDealer
                    ? MahjongScoreTable.ParentRonPoints.TryGetValue((han, fu), out var value) ? value : 0
                    : MahjongScoreTable.ChildRonPoints.TryGetValue((han, fu), out var Childvalue) ? Childvalue : 0;

                int total = basePoint + honbaBonus * 3;
                discarder.AddScore(-total);
                winner.AddScore(total);
            }
        }
    }
}
