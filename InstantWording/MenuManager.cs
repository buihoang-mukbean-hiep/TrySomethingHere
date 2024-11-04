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
            "file ",
            "console ",
            ];
        static readonly string[] showMenu =
            [
            "statistics",
            "speed eye-catching",
            ];
        static readonly string[] priorityMenu =
            [
            "Kanji",
            "Hiragana",
            "Kanji_Vietnamese",
            "Definition",
            ];
        static readonly string[] operationMenu =
            [
            "Add from ",
            "🎲 ",
            "Show ",
            "📖 Review ",
            "under mukbean control, stay away... ",
            ];
        public void Create()
        {
            int[] choiceBuilder = new int[3];
            int pointer = 0;
            StringBuilder menu = new();
            int max = 0;
            while (true)
            {
                if (pointer == 0)
                {
                    menu.AppendLine("\n***********INSTANT_WORDING***********");
                    menu.Append(BuildMenu(max, operationMenu));

                    Write(menu);
                    menu.Clear();
                }
                try
                {
                    if (!int.TryParse(ReadLine(), out choiceBuilder[pointer]))
                        throw new ArgumentException("❗invalid input, only number allowed", nameof(choiceBuilder));

                    Clear();

                    switch (choiceBuilder)
                    {
                        case [1, 0, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append(BuildMenu(max, sourceMenu));
                            WriteLine(menu);
                            pointer++;
                            Reset(ref max, ref menu);
                            break;
                        case [1, 1, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append($"{BuildChosenNode(ref max, sourceMenu, choiceBuilder[1])}");
                            menu.Append("insert the two edges of the list (format:[value1] [value2]): ");
                            Write(menu);

                            string str = ReadLine() ?? throw new ArgumentNullException(nameof(str));
                            string[] values = str.Split(' ');
                            if (!int.TryParse(values[0], out int startRow))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(startRow));

                            if (!int.TryParse(values[1], out int endRow))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(endRow));
                            if (startRow > endRow) (startRow, endRow) = (endRow, startRow);

                            _repositoryWord.GetFromExcel(startRow + 1, endRow + 1);
                            WriteLine("✅loading successfully!");
                            Reset(ref max, ref menu, ref choiceBuilder, ref pointer);
                            break;
                        case [1, 2, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append($"{BuildChosenNode(ref max, sourceMenu, choiceBuilder[1])}");
                            menu.Append("Input: ");
                            Write(menu);

                            _repositoryWord.GetFromConsole(ReadLine() ??
                                throw new ArgumentNullException(null, "❗can't not be null"));
                            WriteLine("✅loading successfully!");
                            Reset(ref max, ref menu, ref choiceBuilder, ref pointer);
                            break;
                        case [2, 0, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            Write(menu);
                            var sw = Stopwatch.StartNew();
                            _repositoryWord.Shuffle();
                            sw.Stop();
                            WriteLine($"Time 4 shuffle: {sw.ElapsedTicks} ticks");
                            Reset(ref max, ref menu, ref choiceBuilder, ref pointer);
                            break;
                        case [3, 0, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append(BuildMenu(max, showMenu));
                            WriteLine(menu);
                            pointer++;
                            Reset(ref max, ref menu);
                            break;
                        case [3, 1, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append($"{BuildChosenNode(ref max, showMenu, choiceBuilder[1])}");
                            Write(menu);
                            _repositoryWord.Show();
                            Reset(ref max, ref menu, ref choiceBuilder, ref pointer);
                            break;
                        case [3, 2, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append($"{BuildChosenNode(ref max, showMenu, choiceBuilder[1])}");
                            menu.Append(BuildMenu(max, priorityMenu));
                            WriteLine(menu);

                            pointer++;

                            Reset(ref max, ref menu);
                            break;
                        case [3, 2, 1] or [3, 2, 2] or [3, 2, 3] or [3, 2, 4]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append($"{BuildChosenNode(ref max, showMenu, choiceBuilder[1])}");
                            menu.Append($"{BuildChosenNode(ref max, priorityMenu, choiceBuilder[2])}");
                            WriteLine(menu);
                            //if (choiceBuilder[2] >= priorityMenu.Length) throw new ArgumentOutOfRangeException("❗out of choice's range");

                            Write("delay interval (in second): ");
                            if (!int.TryParse(ReadLine(), out int delayTime))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(delayTime));
                            _repositoryWord.Show(priorityMenu[choiceBuilder[2]-1], delayTime);
                            Reset(ref max, ref menu, ref choiceBuilder, ref pointer);
                            break;
                        case [4, 0, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append(BuildMenu(max, priorityMenu));
                            WriteLine(menu);

                            pointer++;
                            Reset(ref max, ref menu);
                            break;
                        case [4, 1, 0] or [4, 2, 0] or [4, 3, 0] or [4, 4, 0]:
                            menu.Append($"\n{BuildChosenNode(ref max, operationMenu, choiceBuilder[0])}");
                            menu.Append($"{BuildChosenNode(ref max, priorityMenu, choiceBuilder[1])}");
                            WriteLine(menu);

                            Write("Which level you want to review: "); 
                            if(!int.TryParse(ReadLine(), out int level))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(level));
                            if(level>3)
                                throw new ArgumentOutOfRangeException(nameof(level), "❗out of level's range");
                            _repositoryWord.CheckAnswer(priorityMenu[choiceBuilder[1]-1], level);
                            Reset(ref max, ref menu, ref choiceBuilder, ref pointer);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(choiceBuilder), "❗out of choice's range");
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                    Reset(ref max, ref menu, ref choiceBuilder, ref pointer);
                }
            }
        }

        private static void Reset(ref int max, ref StringBuilder subMenu)
        {
            max = 0; subMenu.Clear();
        }
        private static void Reset(ref int max, ref StringBuilder subMenu, ref int[] choiceBuilder)
        {
            Reset(ref max, ref subMenu);
            choiceBuilder = [0, 0, 0];
        }
        private static void Reset(ref int max, ref StringBuilder subMenu, ref int[] choiceBuilder, ref int pointer)
        {
            Reset(ref max, ref subMenu, ref choiceBuilder);
            pointer = 0;
        }

        private StringBuilder BuildChosenNode(ref int max, string[] menu, int choice)
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
                    .Append($"{_repositoryWord.BuildBalanceDistance(menu[i], ' ', menuMax)}┃\n");
            for (int i = 0; i < 2; i++)
                res.Append(' ', max + 2)
               .Append($"{_repositoryWord.BuildBalanceDistance("", ' ', menuMax)}┃\n");
            max += 3;
            max += menuMax;

            return res;
        }
        private static StringBuilder BuildMenu(int max, string[] menu)
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
