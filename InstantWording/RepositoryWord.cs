using OfficeOpenXml;
using System.Reflection;

namespace InstantWording
{
    public class RepositoryWord : RepositoryBase<Word>
    {
        private PropertyInfo[] proInfos = typeof(Word).GetProperties();

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
                    漢字 = row.ElementAt(0),
                    ひらがな = row.ElementAt(1),
                    Kanji_Vietnamese = row.ElementAt(2),
                    Definition = row.ElementAt(2) //If the number of element row is null then the code break => should replace ElementAt() /set a default array with null and store the gotten data into it /make verification data if null 

                });
                Max(listT[rowNum - 2], proInfos);
            }
        }

        public virtual void GetFromConsole(string? userConsoleInput)
        {
            char columnMark = ';';
            char rowMark = '.';

            if (String.IsNullOrWhiteSpace(userConsoleInput)) throw new ArgumentNullException(nameof(userConsoleInput), "❗no data yet");
            var arrayOfRow = userConsoleInput.Split(rowMark);
            for (int i = 0; i < arrayOfRow.Length; i++)
            {
                var temp = arrayOfRow[i].Split(columnMark);

                listT.Add(new Word
                {
                    漢字 = temp[0],
                    ひらがな = temp[1],
                    Kanji_Vietnamese = temp[2],
                    Definition = temp[3]
                }
                );
                Max(listT[i], proInfos);
            }
        }

        public virtual void Get() => Get(null, 0);

        public virtual void Get(string? priorityProperty, byte intervalInMilisec)
        {
            if (listT.Count == 0) throw new ArgumentNullException(nameof(listT), "❗no data yet, try 0 to add data");
            BuildDistance(listT, proInfos, priorityProperty, intervalInMilisec);
        }
    }
}
