using System;

namespace mahjongOBSAddOns
{
    public static class ScoreCalculator
    {
        public static string? Calculate(int han, int fu, bool isParent, bool isTsumo, int honba, int kyoutaku)
        {
            if(han <= 0 || fu < 20 || fu > 110 || fu % 10 != 0 && fu != 25 ) return null;

            if (isParent && !isTsumo)
            {
                if (MahjongScoreTable.ParentRonPoints.TryGetValue((han, fu), out int score))
                {
                    int total = score + honba * 300 + kyoutaku * 1000;
                    if(han >= 13)
                    {
                        int multiple = han / 13;
                        return $"親/ロン:{multiple}倍役満 → {score}点 + 本場:{honba}本 → {total}点(供託{kyoutaku}本)";
                    }
                    else
                    {
                        return $"親/ロン:{score}点 + 本場:{honba}本 → {total}点(供託{kyoutaku}本)";
                    }
                        
                }
            }
            else if (isParent && isTsumo)
            {
                if (MahjongScoreTable.ParentTsumoPoint.TryGetValue((han, fu), out int score))
                {
                    int total = score + honba * 100 + kyoutaku * 1000;
                    if (han >= 13)
                    {
                        int multiple = han / 13;
                        return $"親/ツモ:{multiple}倍役満 → {total}点ALL + 本場{honba}本 → {total}点ALL(供託{kyoutaku}本)";
                    }
                    else
                    {
                        return $"親/ツモ:{score}点ALL + 本場{honba}本 → {total}点ALL(供託{kyoutaku}本)";
                    }
                    
                }
            }
            else if (!isParent && !isTsumo)
            {
                if (MahjongScoreTable.ChildRonPoints.TryGetValue((han, fu), out int score))
                {
                    int total = score + honba * 300 + kyoutaku * 1000;
                    if (han >= 13)
                    {
                        int multiple = han / 13;
                        return $"子/ロン:{multiple}倍役満 → {total}点 + 本場{honba}本 → {total}点(供託{kyoutaku}本)";
                    }
                    else
                    {
                        return $"子/ロン:{score}点 + 本場{honba}本 + 供託{kyoutaku}本 → {total}点";
                    }
                }
            }
            else if (!isParent && isTsumo)
            {
                if (MahjongScoreTable.ChildTsumoPoints.TryGetValue((han, fu), out var tuple))
                {
                    int total = tuple.fromParent + tuple.fromChild * 2 + honba * 100 * 3 + kyoutaku * 1000;
                    if (han >= 13)
                    {
                        int multiple = han / 13;
                        return $"子/ツモ: {multiple}倍役満:親から{tuple.fromParent}点,子から{tuple.fromChild}点 + 本場:{honba}本 → {total}点(供託{kyoutaku}本)";
                    }
                    else
                    {
                        return $"子/ツモ: 親から{tuple.fromParent}点,子から{tuple.fromChild}点 + 本場:{honba}本 → {total}点(供託{kyoutaku}本)";
                    }
                }
            }
            return "該当する点数は見つかりませんでした。";
        }
           
    }
}


