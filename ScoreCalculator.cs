using System;

namespace mahjongOBSAddOns
{
    public static class ScoreCalculator
    {
        public static string? Calculate(int han, int fu, bool isParent, bool isTsumo)
        {
            if(han <= 0 || fu < 20 || fu > 110 || fu % 10 != 0 && fu != 25 ) return null;

            if (isParent && !isTsumo)
            {
                if (MahjongScoreTable.ParentRonPoints.TryGetValue((han, fu), out int score)) return $"親/ロン:{score}点";
            }
            else if (isParent && isTsumo)
            {
                if (MahjongScoreTable.ParentTsumoPoint.TryGetValue((han, fu), out int score)) return $"親/ツモ:{score}点";
            }
            else if (!isParent && !isTsumo)
            {
                if (MahjongScoreTable.ChildRonPoints.TryGetValue((han, fu), out int score)) return $"子/ロン:{score}点";
            }
            else if (!isParent && isTsumo)
            {
                if (MahjongScoreTable.ChildTsumoPoints.TryGetValue((han, fu), out var tuple)) return $"子/ツモ: 親から{tuple.fromParent}点,子から{tuple.fromChild}点";
            }
            return "該当する点数は見つかりませんでした。";
        }
           
    }
}


