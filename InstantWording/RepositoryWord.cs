using OfficeOpenXml;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace InstantWording
{
    public partial class RepositoryWord : RepositoryBase<Word>
    {
        private readonly List<Word> sameKanjiComponentWordList = [];
        private static readonly PropertyInfo[] proInfos = typeof(Word)
            .GetProperties()
            .Where(prop => new string[] { "Kanji", "Hiragana", "Kanji_Vietnamese", "Definition" }.Any(str => str.Equals(prop.Name)))
            .ToArray();
        private static readonly PropertyInfo[] fullProInfos = typeof(Word).GetProperties();

        private int[] maxOfProp = new int[proInfos.Length];
        private int[] maxOfFullProp = new int[fullProInfos.Length];

        [GeneratedRegex(@"[\p{IsCJKUnifiedIdeographs}\p{IsHiragana}\p{IsKatakana}]")]
        private static partial Regex JPRegex();

        public void ReadExcel(int min, int max, string filePath)
        {
            ExcelPackage xlPackage = new(new FileInfo(filePath));

            var myWorksheet = xlPackage.Workbook.Worksheets.ElementAt(0); //select sheet here
            var totalColumns = myWorksheet.Dimension.End.Column;

            for (int rowNum = min; rowNum <= max; rowNum++) //select starting row here
            {
                var row = myWorksheet.Cells[rowNum, 2, rowNum, totalColumns]
                    .Select(c => c.Value == null ? string.Empty : c.Value.ToString())
                    .ToList();
                listT.Add(new Word
                {
                    Id = (listT.Count+1).ToString(),
                    Kanji = row.ElementAt(0),
                    Hiragana = row.ElementAt(1),
                    Kanji_Vietnamese = row.ElementAt(2),
                    Definition = row.ElementAt(2),
                    LastReviewDate = DateTime.Now,
                });
                Max(listT[^1], proInfos, ref maxOfProp);
            }
        }
        public async Task ReadAsync(string filePath)
        {
            using StreamReader reader = new(filePath);
            string fileInput = await reader.ReadToEndAsync();

            char columnMark = ';';
            char rowMark = '.';

            var arrayOfRow = fileInput.Split(rowMark);

            for (int i = 0; i < arrayOfRow.Length - 1; i++)
            {
                var temp = arrayOfRow[i].Split(columnMark);
                if (temp.Length == 4)
                    listT.Add(new Word
                    {
                        Id = (listT.Count + 1).ToString(),
                        Kanji = temp[0],
                        Hiragana = temp[1],
                        Kanji_Vietnamese = temp[2],
                        Definition = temp[3],
                        LastReviewDate = DateTime.Now,
                    });
                else
                    listT.Add(new Word
                    {
                        Id = temp[0],
                        Kanji = temp[1],
                        Hiragana = temp[2],
                        Kanji_Vietnamese = temp[3],
                        Definition = temp[4],
                        RememberLevel = int.Parse(temp[5]),
                        LastReviewDate = DateTime.Parse(temp[6]),
                        NextReviewLeft = int.Parse(temp[7]),
                        ReviewTimes = int.Parse(temp[8]),
                    }
                    );
                Max(listT[^1], proInfos, ref maxOfProp);
            }
        }
        public async Task WriteAsync()
        {
            string filePath = $@"C:\Users\ADMIN\Desktop\Review on {DateTime.Now:d-MMM-yy HH：MM：ss}.txt";
            var listToSb = new StringBuilder();
            foreach (var item in listT)
            {
                foreach (var prop in fullProInfos)
                {
                    string temp = prop.GetValue(item)?.ToString() ?? String.Empty;
                    listToSb.Append($"{temp};");
                }
                listToSb[^1] = '.';
            }
            await File.WriteAllTextAsync(filePath, listToSb.ToString());
        }

        public virtual void Get() => Get(null, 0, 4);
        public virtual void Get(string? priorityProperty, int intervalInMilisec, int reviewLevel)
        {
            if (listT.Count == 0)
                throw new ArgumentNullException(message: "❗no data yet, try 0 to add data", paramName: nameof(priorityProperty));
            if (!String.IsNullOrWhiteSpace(priorityProperty)) SwapPriority(priorityProperty);
            foreach (var item in listT)
            {
                if (item.RememberLevel > reviewLevel) continue;
                var resetInterval = intervalInMilisec;
                int i = 0;

                Write($"{item.Id}.{new string(' ', 3 - item.Id.ToString().Length)}");
                foreach (var prop in proInfos)
                {
                    string tempCheck = prop.GetValue(item)?.ToString() ?? string.Empty;
                    if (JPRegex().IsMatch(tempCheck))
                    {
                        SetRememberLevelColor(item);
                        Write(BuildBalanceDistance(tempCheck, '＿', maxOfProp[i]));
                        ResetColor();
                    }
                    else
                    {
                        Write(BuildBalanceDistance(tempCheck, ' ', maxOfProp[i]));
                    }
                    Thread.Sleep(resetInterval /= 2);
                    i++;
                }
                ForegroundColor = ConsoleColor.DarkGray;
                Write($"Level:{item.RememberLevel}|Times:{item.ReviewTimes}" +
                    $"|Last:{item.LastReviewDate:dd-MMM-yyyy}|⏳:{item.NextReviewLeft}");
                ResetColor();
                WriteLine('\n');
            }
        }
        public virtual void Add(string userConsoleInput)
        {
            char columnMark = ';';
            char rowMark = '.';

            var arrayOfRow = userConsoleInput.Split(rowMark);
            for (int i = 0; i < arrayOfRow.Length; i++)
            {
                var temp = arrayOfRow[i].Split(columnMark);

                listT.Add(new Word
                {
                    Id = (listT.Count + 1).ToString(),
                    Kanji = temp[0],
                    Hiragana = temp[1],
                    Kanji_Vietnamese = temp[2],
                    Definition = temp[3]
                }
                );
                Max(listT[^1], proInfos, ref maxOfProp);
            }
        }

        public void CheckAnswer(string? priorityProperty, int reviewLevel)
        {
            if (listT.Count == 0)
                throw new ArgumentNullException(message: "❗no data yet, try 0 to add data", paramName: nameof(priorityProperty));
            if (!String.IsNullOrWhiteSpace(priorityProperty)) SwapPriority(priorityProperty);

            foreach (var item in listT)
            {
                if (item.RememberLevel > reviewLevel) continue;
                string root = proInfos[0].GetValue(item)?.ToString() ?? string.Empty;
                SetRememberLevelColor(item);
                WriteLine(root);
                ResetColor();

                var sw = Stopwatch.StartNew();
                Write($"other meanings: ");
                string raw = ReadLine() ?? string.Empty;
                raw = raw.ToLower();
                sw.Stop();
                WriteLine($"⏰: {sw.ElapsedMilliseconds / 1000} s");

                if (raw.Equals("esc") || raw.Equals("exit") || raw.Equals("out")) break;
                string[] check = raw.Split(';');

                char[] matchedAnswer = new char[proInfos.Length - 1];

                for (int i = 0; i < 3; i++)
                {
                    string tempCheck = proInfos[i + 1].GetValue(item)?.ToString() ?? string.Empty;
                    for (int j = 0; j < check.Length; j++)
                    {
                        ResetColor();
                        if (check[j].Equals(tempCheck.ToLower()))
                        {
                            matchedAnswer[i] = '✅';
                            item.RememberLevel++;
                            UpdateFurtherReview(item);
                            break;
                        }
                        else
                        {
                            ForegroundColor = ConsoleColor.Red;
                            matchedAnswer[i] = '❌';
                        }
                    }
                    if (JPRegex().IsMatch(tempCheck))
                        Write(BuildBalanceDistance(tempCheck, ' ', maxOfProp[i + 1]));
                    else
                    {
                        Write(BuildBalanceDistance(tempCheck, ' ', maxOfProp[i + 1]));
                    }
                    ResetColor();
                    WriteLine($"{matchedAnswer[i]} " +
                                $"{matchedAnswer.Count(x => x.Equals('✅')) * 100 / matchedAnswer.Length}%");
                }
                WriteLine();

                if (matchedAnswer.All(x => x.Equals('❌')))
                {
                    item.RememberLevel--;
                    UpdateFurtherReview(item);
                }
            }
        }

        //new update will reset some fields likes review times to 0,...
        public void UpdateAsync() => throw new NotImplementedException();

        private static void Max(Word item, PropertyInfo[] propertyInfos, ref int[] max)
        {
            int i = 0;
            foreach (var prop in propertyInfos)
            {
                string tempCheck = prop.GetValue(item)?.ToString() ?? string.Empty;
                if (JPRegex().IsMatch(tempCheck))
                    max[i] = Math.Max(tempCheck.Length, max[i]);
                else
                    max[i] = Math.Max(tempCheck.Length, max[i]);
                i++;
            }
        }
        private static void SetRememberLevelColor(Word item)
        {
            ForegroundColor = item.RememberLevel < -2 ? ConsoleColor.DarkGray
                                        : item.RememberLevel < 0 ? ConsoleColor.DarkRed
                                        : item.RememberLevel == 0 ? ConsoleColor.White
                                        : item.RememberLevel < 3 ? ConsoleColor.DarkYellow
                                                                 : ConsoleColor.DarkGreen;
            BackgroundColor = item.RememberLevel < -2 ? ConsoleColor.DarkRed
                                                      : ConsoleColor.Black;
        }
        private static void UpdateFurtherReview(Word word)
        {
            if (word.RememberLevel > 3)
            {
                word.ReviewTimes++;
                word.RememberLevel = 0;
                word.LastReviewDate = DateTime.Now;
                word.NextReviewLeft = 2 * word.ReviewTimes + 1;
            }
            else if (word.RememberLevel < -3)
            {
                word.ReviewTimes--;
                word.RememberLevel = 0;
                word.LastReviewDate = DateTime.Now;
                word.NextReviewLeft = 2 * word.ReviewTimes + 1;
            }
        }
        private static void SwapPriority(string prior)
        {
            for (int i = 0; i < proInfos.Length; i++)
            {
                if (proInfos[i].Name.Equals(prior))
                {
                    (proInfos[i], proInfos[0]) = (proInfos[0], proInfos[i]);
                    break;
                }
            }
        }

        public void Delete() => throw new NotImplementedException();
        public virtual void PrimierReview()
        {

            //auto repeat during long review with the false word
        }

    }
}
