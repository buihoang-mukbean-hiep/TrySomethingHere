﻿using OfficeOpenXml;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Console;

namespace InstantWording
{
    public partial class RepositoryWord : RepositoryBase<Word>
    {
        private static readonly PropertyInfo[] proInfos = typeof(Word)
            .GetProperties()
            .Where(prop => prop.PropertyType == typeof(string))
            .ToArray();
        private int[] maxOfProp = new int[proInfos.Length];
        [GeneratedRegex(@"[\p{IsCJKUnifiedIdeographs}\p{IsHiragana}\p{IsKatakana}]")]
        private static partial Regex JPRegex();

        public void GetFromExcel(int min, int max)
        {
            ExcelPackage xlPackage = new(new FileInfo(@"D:\Documents\日本語\Mimikara.xlsx"));

            var myWorksheet = xlPackage.Workbook.Worksheets.ElementAt(0); //select sheet here
            var totalColumns = myWorksheet.Dimension.End.Column;

            for (int rowNum = min; rowNum <= max; rowNum++) //select starting row here
            {
                var row = myWorksheet.Cells[rowNum, 2, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToList();
                listT.Add(new Word
                {
                    Kanji = row.ElementAt(0),
                    Hiragana = row.ElementAt(1),
                    Kanji_Vietnamese = row.ElementAt(2),
                    Definition = row.ElementAt(2) //If the number of element row is null then the code break => should replace ElementAt() /set a default array with null and store the gotten data into it /make verification data if null 

                });
                Max(listT[rowNum - 2], proInfos, ref maxOfProp);
            }
        }

        public virtual void GetFromConsole(string userConsoleInput)
        {
            char columnMark = ';';
            char rowMark = '.';

            var arrayOfRow = userConsoleInput.Split(rowMark);
            for (int i = 0; i < arrayOfRow.Length; i++)
            {
                var temp = arrayOfRow[i].Split(columnMark);

                listT.Add(new Word
                {
                    Kanji = temp[0],
                    Hiragana = temp[1],
                    Kanji_Vietnamese = temp[2],
                    Definition = temp[3]
                }
                );
                Max(listT[i], proInfos, ref maxOfProp);
            }
        }

        public virtual void Get() => Get(null, 0);
        public virtual void Get(string? priorityProperty, int intervalInMilisec)
        {
            if (listT.Count == 0)
                throw new ArgumentNullException(message: "❗no data yet, try 0 to add data", paramName: nameof(priorityProperty));
            if (!String.IsNullOrWhiteSpace(priorityProperty)) SwapPriority(priorityProperty);
            foreach (var item in listT)
            {
                var resetInterval = intervalInMilisec;
                int i = 0;
                foreach (var prop in proInfos)
                {
                    string tempCheck = prop.GetValue(item)?.ToString() ?? string.Empty;
                    if (JPRegex().IsMatch(tempCheck))
                        Write(BuildBalanceDistance(tempCheck, '＿', maxOfProp[i]));
                    else
                        Write(BuildBalanceDistance(tempCheck, '_', maxOfProp[i]));
                    Thread.Sleep(resetInterval /= 2);
                    i++;
                }
                Write($"Level:{item.RememberLevel}|Times:{item.ReviewTimes}" +
                    $"|PreviousOn:{item.LastReviewDate}|⏳:{item.NextReviewLeft}");
                WriteLine();
            }
        }

        public void CheckAnswer(string? priorityProperty, int reviewLevel)
        {
            if (listT.Count == 0)
                throw new ArgumentNullException(message: "❗no data yet, try 0 to add data", paramName: nameof(priorityProperty));
            if (!String.IsNullOrWhiteSpace(priorityProperty)) SwapPriority(priorityProperty);

            foreach (var item in listT)
            {
                if (item.RememberLevel>reviewLevel) continue;
                string root = proInfos[0].GetValue(item)?.ToString() ?? string.Empty;
                WriteLine(root);

                var sw = Stopwatch.StartNew();
                Write($"other meanings: ");
                string raw = ReadLine() ?? string.Empty;
                sw.Stop();
                WriteLine($"⏰: {sw.ElapsedMilliseconds / 1000} s");

                if (raw.Equals("esc") || raw.Equals("exit") ||raw.Equals("out")) break;
                string[] check = raw.Split(';');

                char[] matchedAnswer = new char[proInfos.Length - 1];

                for (int i = 0; i < 3; i++)
                {
                    string tempCheck = proInfos[i + 1].GetValue(item)?.ToString() ?? string.Empty;
                    if (JPRegex().IsMatch(tempCheck))
                        Write(BuildBalanceDistance(tempCheck, '＿', maxOfProp[i + 1]));
                    else
                        Write(BuildBalanceDistance(tempCheck, '_', maxOfProp[i + 1]));
                    for (int j = 0; j < check.Length; j++)
                    {
                        if (check[j].Equals(tempCheck))
                        {
                            matchedAnswer[i] = '✅';
                            item.RememberLevel++;
                            UpdateFurtherReview(item);
                            break;
                        }
                        else
                            matchedAnswer[i] = '❌';
                    }
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
    }
}
