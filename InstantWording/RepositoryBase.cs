using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace InstantWording
{
    public class RepositoryBase<T>
        where T : class
    {
        protected readonly List<T> listT = [];

        private static readonly string japanesePattern = @"[\p{IsCJKUnifiedIdeographs}\p{IsHiragana}\p{IsKatakana}]";
        protected int maxJP = 0;
        protected int maxVN = 0;

        public virtual void Shuffle()
        {
            if (listT.Count == 0) throw new ArgumentNullException(nameof(listT), "❗no data yet, try 0 to add data");
            Random randIndex = new();
            int length = listT.Count;

            for (int i = 0; i < length - 1; i++)
            {
                int k = randIndex.Next(i, length);
                (listT[i], listT[k]) = (listT[k], listT[i]);
            }
        }

        protected virtual void BuildDistance(List<T> list, PropertyInfo[] propertyInfos, string? priorityProperty, byte interval)
        {
            if (!String.IsNullOrWhiteSpace(priorityProperty))
            {
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    if (propertyInfos[i].Name.Equals(priorityProperty))
                    {
                        (propertyInfos[i], propertyInfos[0]) = (propertyInfos[0], propertyInfos[i]);
                        break;
                    }
                }
            }

            foreach (var item in list)
            {
                var resetInterval = interval * 1000;
                foreach (var prop in propertyInfos)
                {
                    string tempCheck = prop.GetValue(item)?.ToString() ?? string.Empty;
                    Write(tempCheck);
                    if (Regex.IsMatch(tempCheck, japanesePattern))
                        Write(new string('＿', maxJP - tempCheck.Length) + '|');
                    else
                        Write(new string('_', maxVN - tempCheck.Length) + '|');
                    Thread.Sleep(resetInterval /= 2);
                }
                WriteLine();
            }
        }
        //━━┓
        //  ┃
        //  ┃
        //  ┗━━→
        public StringBuilder BuildBalanceDistance(string unBalance)
        => new StringBuilder().Append("┣→")
                              .Append(unBalance);

        public StringBuilder BuildBalanceDistance(string unBalance, char repeatMark, char endMark, int max)
        => BuildBalanceDistance(unBalance).Append(repeatMark, max - unBalance.Length + 2)
                                          .Append(endMark);

        public StringBuilder BuildBalanceDistance(int max, string[] menu, byte choice)
        {
            StringBuilder res = new();
            for (int i = 1; i < choice; i++) res.Append($"{BuildBalanceDistance(menu[i - 1], ' ', ' ', max)}\n");
            res.Append($"{BuildBalanceDistance(menu[choice - 1], '━', '┓', max)}\n");
            for (int i = choice; i < menu.Length; i++) res.Append($"{BuildBalanceDistance(menu[i], ' ', '┃', max)}\n");
            return res;
        }

        //public StringBuilder BuildBalanceDistance(int max, string[] menu)
        //{
        //    StringBuilder res = new();
        //    for (int i = 1; i < choice; i++) res.Append($"{BuildBalanceDistance(menu[i - 1], ' ', ' ', max)}\n");
        //    res.Append($"{BuildBalanceDistance(menu[choice - 1], '━', '┓', max)}\n");
        //    for (int i = choice; i < menu.Length; i++) res.Append($"{BuildBalanceDistance(menu[i], ' ', '┃', max)}\n");
        //    return res;
        //}

        protected virtual void Max(T item, PropertyInfo[] propertyInfos)
        {
            foreach (var prop in propertyInfos)
            {
                string tempCheck = prop.GetValue(item)?.ToString() ?? string.Empty;
                if (Regex.IsMatch(tempCheck, japanesePattern))
                    maxJP = Math.Max(tempCheck.Length, maxJP);
                else
                    maxVN = Math.Max(tempCheck.Length, maxVN);
            }
        }
        //readonly can not set value, then whether readonly == static? else consider about resource taken

        private static void FurtherReview(ref int priority)
        {
            throw new NotImplementedException();
        }

        private static bool CheckAnswer(string answer)
        {
            throw new NotImplementedException();
        }


    }
}

