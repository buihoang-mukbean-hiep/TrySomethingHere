using System.Diagnostics;
using System.Text;
using static System.Console;

namespace InstantWording
{
    public class MenuManager(RepositoryWord repositoryWord)
    {
        private readonly RepositoryWord _repositoryWord = repositoryWord;

        public readonly string[] specialSets =
            [
            "┗→",
            "❗",
            "❓",
            "|\n┗→",
            "▼",
            "✅",
            ];
        static readonly string[] sourceMenu =
            [
            "file",
            "console",
            ];

        static readonly string[] showMenu =
            [
            "all",
            "priority field with delay",
            "Back"
            ];
        static readonly string[] priorityMenu =
            [
            "漢字",
            "ひらがな",
            "Kanji_Vietnamese",
            "Definition",
            "Back"
            ];
        static readonly string[] operationMenu =
            [
            "Add from",
            "Shuffle list",
            "Show",
            "Exit"
            ];
        public void Create()
        {
            StringBuilder choiceBuilder = new();
            do
            {
                if (choiceBuilder.Length == 0)
                {
                    StringBuilder menu = new();
                    menu.AppendLine("\n***********INSTANT_WORDING***********");
                    for (int i = 1; i < operationMenu.Length; i++)
                    {
                        menu.AppendLine($"{i} . {operationMenu[i - 1]}");
                    }
                    Write(menu.ToString());
                }

                choiceBuilder.Append(ReadKey().KeyChar);
                Clear();

                StringBuilder subMenu = new();
                subMenu.Append($"| \n");
                try
                {
                    switch (choiceBuilder.ToString())
                    {
                        case "1":
                            Write("\n" + _repositoryWord
                                .BuildBalanceDistance((int)operationMenu.Max()?.Length, operationMenu, byte.Parse(choiceBuilder.ToString()))
                                .ToString());
                            for (int i = 1; i <= sourceMenu.Length; i++)
                            {
                                subMenu.AppendLine($"1.{i}| {sourceMenu[i - 1]}");
                            }
                            //Write($"{subMenu}┓\n|\n┗→");

                            choiceBuilder.Append('.');
                            break;

                        case "1.1":
                            subMenu.Append("insert the two edges of the list (format:[value1] [value2]): ");
                            Write(subMenu.ToString());

                            string str = ReadLine() ?? throw new ArgumentNullException(nameof(str));
                            string[] values = str.Split(' ');
                            if (!int.TryParse(values[0], out int startRow)
                                || !int.TryParse(values[1], out int endRow)) throw new InvalidDataException("❗invalid input, only number allowed");
                            if (startRow > endRow) (startRow, endRow) = (endRow, startRow);

                            _repositoryWord.GetFromExcel(startRow + 1, endRow + 1);
                            WriteLine("✅loading successfully!");

                            choiceBuilder.Clear();
                            break;

                        case "1.2":
                            subMenu.AppendLine("Input: ");
                            Write(subMenu.ToString());

                            _repositoryWord.GetFromConsole(ReadLine());
                            WriteLine("✅loading successfully!");
                            choiceBuilder.Clear();
                            break;

                        case "2":
                            Write(subMenu.ToString());
                            var sw = Stopwatch.StartNew();
                            _repositoryWord.Shuffle();
                            sw.Stop();
                            WriteLine($"Time 4 shuffle: {sw.ElapsedTicks} ticks\nShuffle done, u can try new list now...");

                            choiceBuilder.Clear();
                            break;

                        case "3":
                            Write("\n" + _repositoryWord
                                .BuildBalanceDistance((int)operationMenu.Max()?.Length, operationMenu, byte.Parse(choiceBuilder.ToString()))
                                .ToString());
                            for (int i = 1; i < showMenu.Length; i++)
                            {
                                subMenu.AppendLine($"{i}. {showMenu[i - 1]}");
                            }
                            Write($"{subMenu}|\n┗→");

                            choiceBuilder.Append('.');
                            break;
                        case "3.1":
                            Write(subMenu.ToString());
                            _repositoryWord.Get();

                            choiceBuilder.Clear();
                            break;

                        case "3.2":
                            subMenu.AppendLine("| Shuffle_it");
                            for (int i = 1; i < priorityMenu.Length; i++)
                            {
                                subMenu.AppendLine($"{i}. {priorityMenu[i - 1]}");
                            }
                            Write(subMenu.ToString() + "|\n┗→Your choice: ");

                            if (!byte.TryParse(ReadLine(), out byte subUserChoice)) throw new InvalidDataException("❗invalid input, only number allowed");
                            if (subUserChoice >= priorityMenu.Length) throw new ArgumentOutOfRangeException("❗out of choice's range");

                            Write("delay interval (in second): ");
                            if (!byte.TryParse(ReadLine(), out byte delayTime)) throw new InvalidDataException("❗invalid input, only number allowed");
                            _repositoryWord.Get(priorityMenu[subUserChoice], delayTime);

                            choiceBuilder.Clear();
                            break;
                        case "3.4" or "3.5":
                                break;
                        default:
                            choiceBuilder.Clear();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message);
                    WriteLine(ex.Message);
                    choiceBuilder.Clear();
                }
            }
            while (true);
        }


    }
}
