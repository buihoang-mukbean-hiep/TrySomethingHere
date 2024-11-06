using static System.Console;
using static System.Net.Mime.MediaTypeNames;

Write("input path: ");
try
{
    string filePath = ReadLine() ?? throw new ArgumentNullException(nameof(filePath));
    filePath = filePath.Trim('"');
    using StreamReader reader = new(filePath ?? throw new ArgumentNullException());
    string fileInput = await reader.ReadToEndAsync();
    WriteLine(fileInput);
    //var arrayOfRow = userConsoleInput.Split(rowMark);
    //for (int i = 0; i < arrayOfRow.Length; i++)
    //{
    //    var temp = arrayOfRow[i].Split(columnMark);

    //    listT.Add(new Word
    //    {
    //        Id = listT.Count + 1,
    //        Kanji = temp[0],
    //        Hiragana = temp[1],
    //        Kanji_Vietnamese = temp[2],
    //        Definition = temp[3]
    //    }
    //    );
    //    Max(listT[^1], proInfos, ref maxOfProp);
    //}
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}