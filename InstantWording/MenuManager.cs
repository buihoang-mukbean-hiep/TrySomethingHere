using System.Diagnostics;
using System.Text;
using static System.Console;

namespace InstantWording
{
    public class MenuManager(RepositoryWord repositoryWord)
    {
        private readonly RepositoryWord _repositoryWord = repositoryWord;
        static readonly string[] sourceMenu =
            [
            "file",
            "console",
            ];
        int sourceMenuMax = sourceMenu.Max(x => x.Length);
        static readonly string[] showMenu =
            [
            "all",
            "priority field with delay",
            ];
        int showMenuMax = sourceMenu.Max(x => x.Length);
        static readonly string[] priorityMenu =
            [
            "漢字",
            "ひらがな",
            "Kanji_Vietnamese",
            "Definition",
            ];
        int priorityMenuMax = sourceMenu.Max(x => x.Length);
        static readonly string[] operationMenu =
            [
            "Add from ",
            "Shuffle list ",
            "Show ",
            "asdasd",
            "asdasd",
            ];
        int operationMenuMax = sourceMenu.Max(x => x.Length);
        public void Create()
        {
            byte[] choiceBuilder = new byte[3];
            int pointer = 0;
            StringBuilder subMenu = new();
            int max = 0;
            do
            {
                if (pointer == 0)
                {
                    subMenu.AppendLine("\n***********INSTANT_WORDING***********");
                    subMenu.Append(BuildTail(max, operationMenu));
                    //subMenu.Append($"\n{BuildNode(ref max, operationMenu, choiceBuilder)}");

                    Write(subMenu.ToString());
                    subMenu.Clear();
                }

                //choiceBuilder.Append(ReadKey().KeyChar);
                choiceBuilder[pointer] = byte.Parse(ReadLine() ?? throw new NullReferenceException());
                Clear();

                try
                {
                    switch (choiceBuilder)
                    {

                        case [1, 0, 0]:
                            subMenu.Append($"\n{BuildHead(ref max, operationMenu, choiceBuilder[0])}");
                            subMenu.Append(BuildTail(max, sourceMenu));
                            WriteLine(subMenu);
                            //choiceBuilder.Append('.');
                            pointer++;
                            subMenu.Clear(); max = 0;
                            break;
                        case [1, 1, 0]:
                            subMenu.Append($"\n{BuildHead(ref max, operationMenu, choiceBuilder[0])}");
                            subMenu.Append($"{BuildHead(ref max, sourceMenu, choiceBuilder[1])}");
                            subMenu.Append("insert the two edges of the list (format:[value1] [value2]): ");
                            Write(subMenu);

                            string str = ReadLine() ?? throw new ArgumentNullException(nameof(str));
                            string[] values = str.Split(' ');
                            if (!int.TryParse(values[0], out int startRow)
                                || !int.TryParse(values[1], out int endRow)) throw new InvalidDataException("❗invalid input, only number allowed");
                            if (startRow > endRow) (startRow, endRow) = (endRow, startRow);

                            _repositoryWord.GetFromExcel(startRow + 1, endRow + 1);
                            WriteLine("✅loading successfully!");

                            throw new Exception("clear some data");
                        case [1, 2, 0]:
                            subMenu.AppendLine("Input: ");
                            Write(subMenu.ToString());

                            _repositoryWord.GetFromConsole(ReadLine());
                            WriteLine("✅loading successfully!");
                            throw new Exception("clear some data");
                        case [2, 0, 0]:
                            Write(subMenu.ToString());
                            var sw = Stopwatch.StartNew();
                            _repositoryWord.Shuffle();
                            sw.Stop();
                            WriteLine($"Time 4 shuffle: {sw.ElapsedTicks} ticks\n" +
                                $"Shuffle done, u can try new list now...");
                            throw new Exception("clear some data");
                        case [3, 0, 0]:
                            subMenu.Append($"\n{BuildHead(ref max, operationMenu, choiceBuilder[0])}");
                            subMenu.Append(BuildTail(max, showMenu));
                            WriteLine(subMenu);

                            pointer++;
                            subMenu.Clear(); max = 0;
                            break;
                        case [3, 1, 0]:
                            subMenu.Append($"\n{BuildHead(ref max, operationMenu, choiceBuilder[0])}");
                            subMenu.Append($"\n{BuildHead(ref max, operationMenu, choiceBuilder[1])}");
                            Write(subMenu);
                            _repositoryWord.Get();
                            throw new Exception("clear some data");
                        case [3, 2, 0]:

                            subMenu.Append($"\n{BuildHead(ref max, operationMenu, choiceBuilder[0])}");
                            subMenu.Append($"{BuildHead(ref max, showMenu, choiceBuilder[1])}");
                            subMenu.Append(BuildTail(max, priorityMenu));
                            WriteLine(subMenu);

                            if (!byte.TryParse(ReadLine(), out byte subUserChoice)) throw new InvalidDataException("❗invalid input, only number allowed");
                            if (subUserChoice >= priorityMenu.Length) throw new ArgumentOutOfRangeException("❗out of choice's range");

                            Write("delay interval (in second): ");
                            if (!byte.TryParse(ReadLine(), out byte delayTime)) throw new InvalidDataException("❗invalid input, only number allowed");
                            _repositoryWord.Get(priorityMenu[subUserChoice], delayTime);

                            throw new Exception("clear some data");
                        //break;
                        default:
                            throw new Exception("clear some data");
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                    pointer = 0; subMenu.Clear(); max = 0; choiceBuilder = [0, 0, 0];

                }
            }
            while (true);
        }
        private StringBuilder BuildHead(ref int max, string[] menu, byte choice)
        {
            StringBuilder res = new();

            int menuMax = (menu.Max(s => s.Length) + 2);
            for (int i = 1; i < choice; i++)
                res.Append(' ', max)
                    .Append("┣→")
                    .Append($"{_repositoryWord.BuildBalanceDistance(menu[i - 1], ' ', menuMax)}\n");
            res.Append(' ', max)
                    .Append("┣→")
                .Append($"{_repositoryWord.BuildBalanceDistance(menu[choice - 1], '━', menuMax)}┓\n");
            for (int i = choice; i < menu.Length; i++)
                res.Append(' ', max)
                    .Append("┣→")
                    .Append($"{_repositoryWord.BuildBalanceDistance(menu[i - 1], ' ', menuMax)}┃\n");
            max += 4;
            max += menuMax;

            return res;
        }
        private static StringBuilder BuildTail(int max, string[] menu)
        {
            StringBuilder res = new();
            for (int i = 0; i < menu.Length; i++)
                res.Append(' ', max)
                    .Append("┣→")
                    .Append($"{i + 1}.{menu[i]}\n");
            return res;
        }
    }
}
