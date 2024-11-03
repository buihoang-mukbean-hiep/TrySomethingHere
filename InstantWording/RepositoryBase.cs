using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace InstantWording
{
    public class RepositoryBase<T>
        where T : class
    {
        protected readonly List<T> listT = [];
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
        public StringBuilder BuildBalanceDistance(string unBalance, char repeatMark, int max)
        => new StringBuilder().Append(unBalance)
                              .Append(repeatMark, max - unBalance.Length + 1);
    }
}

