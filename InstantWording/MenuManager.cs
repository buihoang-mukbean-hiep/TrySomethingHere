using System.Diagnostics;
using System.Text;
using static System.Console;

namespace InstantWording
{
    public partial class MenuManager(RepositoryWord repositoryWord)
    {
        private readonly RepositoryWord _repositoryWord = repositoryWord;

        public async Task Create()
        {

            int[] choiceBuilder = new int[3];
            int pointer = 0;
            StringBuilder menuBuilder = new();
            int max = 0;

            var sw = new Stopwatch();
            while (true)
            {
                if (pointer == 0)
                {
                    Write(_repositoryWord.SetProgressColor(asciiArts[0]));
                    Write(menuBuilder.Append($"\x1b[5;1;38;2;{_repositoryWord.ConvertRGBtoANSIcode("rgb(250, 247, 200)")}m" +
                        $"{BuildMenu(max, menuBehavior)}[0m"));
                    menuBuilder.Clear();
                }
                menuBuilder.AppendLine();
                try
                {
                    if (!int.TryParse(ReadLine(), out choiceBuilder[pointer]))
                        throw new ArgumentException("❗invalid input, only number allowed", nameof(choiceBuilder));
                    Clear(); menuBuilder.Clear();
                    switch (choiceBuilder)
                    {
                        case [1, 0, 0]:
                            WriteLine(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                                 .Append(BuildMenu(max, subMenuModify)));
                            pointer++; Reset(ref max, ref menuBuilder);
                            break;
                        case [1, 1, 0]:
                            menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                .Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[1])}")
                                       .Append(BuildMenu(max, subMenuSource));
                            WriteLine(menuBuilder);

                            pointer++;
                            Reset(ref max, ref menuBuilder);
                            break;
                        case [1, 1, 1]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                             .Append($"{BuildChosenNode(ref max, subMenuModify, choiceBuilder[1])}")
                                             .Append($"{BuildChosenNode(ref max, subMenuSource, choiceBuilder[2])}")
                                             .Append("insert the two edges of the list (format:[value1] [value2]): "));
                            string str = ReadLine() ?? throw new ArgumentNullException(nameof(str));
                            string[] values = str.Split(' ');
                            if (!int.TryParse(values[0], out int startRow))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(startRow));
                            if (!int.TryParse(values[1], out int endRow))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(endRow));
                            if (startRow > endRow) (startRow, endRow) = (endRow, startRow);
                            Write("🧷file path: ");
                            _repositoryWord.ReadExcel(startRow + 1
                                , endRow + 1
                                , ReadLine()?.Trim('"') ?? throw new ArgumentNullException());
                            WriteLine("✅loading successfully!");
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [1, 1, 2]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                             .Append($"{BuildChosenNode(ref max, subMenuModify, choiceBuilder[1])}")
                                             .Append($"{BuildChosenNode(ref max, subMenuSource, choiceBuilder[2])}")
                                             .Append("🧷file path: "));
                            await _repositoryWord.ReadAsync(
                                ReadLine()?.Trim('"') ?? throw new ArgumentNullException());
                            //consider a loading bar??
                            WriteLine($"Time 4 loading: {sw.ElapsedMilliseconds}ms" +
                            $"\n✅loading successfully!");
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [1, 1, 3]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                             .Append($"{BuildChosenNode(ref max, subMenuModify, choiceBuilder[1])}")
                                             .Append($"{BuildChosenNode(ref max, subMenuSource, choiceBuilder[2])}"));

                            sw = Stopwatch.StartNew();
                            await _repositoryWord.WriteAsync();
                            sw.Stop();
                            WriteLine($"Time 4 write to file: {sw.ElapsedMilliseconds}ms");

                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                            case [1, 6, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}"));
                            sw = Stopwatch.StartNew();
                            _repositoryWord.OrderBy();
                            sw.Stop();
                            WriteLine($"Time 4 sort: {sw.ElapsedTicks} ticks");
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [2, 0, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}"));
                            sw = Stopwatch.StartNew();
                            _repositoryWord.Shuffle();
                            sw.Stop();
                            WriteLine($"Time 4 shuffle: {sw.ElapsedTicks} ticks");
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [3, 0, 0]:
                            WriteLine(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                          .Append(BuildMenu(max, subMenuSearch)));
                            pointer++;
                            Reset(ref max, ref menuBuilder);
                            break;
                        case [3, 1, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                      .Append($"{BuildChosenNode(ref max, subMenuSearch, choiceBuilder[1])}"));
                            _repositoryWord.Get();
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [4, 0, 0]:
                            WriteLine(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                          .Append(BuildMenu(max, subMenuBrain)));
                            pointer++;
                            Reset(ref max, ref menuBuilder);
                            break;
                        case [4, 1, 0]:
                            menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}");
                            menuBuilder.Append($"{BuildChosenNode(ref max, subMenuBrain, choiceBuilder[1])}");
                            menuBuilder.Append(BuildMenu(max, subMenuPriority));
                            WriteLine(menuBuilder);

                            pointer++;
                            Reset(ref max, ref menuBuilder);
                            break;
                        case [4, 1, 1] or [4, 1, 2] or [4, 1, 3] or [4, 1, 4]:
                            menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}");
                            menuBuilder.Append($"{BuildChosenNode(ref max, subMenuBrain, choiceBuilder[1])}");
                            menuBuilder.Append($"{BuildChosenNode(ref max, subMenuPriority, choiceBuilder[2])}");
                            WriteLine(menuBuilder);
                            //if (choiceBuilder[2] >= priorityMenu.Length) throw new ArgumentOutOfRangeException("❗out of choice's range");

                            Write("delay interval (in second): ");
                            if (!int.TryParse(ReadLine(), out int delayTime))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(delayTime));
                            Write("remember level (high at 3): ");
                            if (!int.TryParse(ReadLine(), out int rememberLevel))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(delayTime));

                            _repositoryWord.Get(subMenuPriority[choiceBuilder[2] - 1], delayTime, rememberLevel);
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [4, 2, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                          .Append($"{BuildChosenNode(ref max, subMenuBrain, choiceBuilder[1])}")
                                          .Append(BuildMenu(max, subMenuPriority)));
                            pointer++;
                            Reset(ref max, ref menuBuilder);
                            break;
                        case [4, 2, 1] or [4, 2, 2] or [4, 2, 3] or [4, 2, 4]:
                            Write($"{menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                          .Append($"{BuildChosenNode(ref max, subMenuPriority, choiceBuilder[1])}")}" +
                                          $"Which level you want to review: ");
                            if (!int.TryParse(ReadLine(), out int level))
                                throw new ArgumentException("❗invalid input, only number allowed", nameof(level));
                            if (level > 3)
                                throw new ArgumentOutOfRangeException(nameof(level), "❗out of level's range");
                            _repositoryWord.CheckAnswer(subMenuPriority[choiceBuilder[1] - 1], level);
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;

                        case [3, 2, 0] or [3, 3, 0] or [1, 2, 0] or [1, 3, 0] or [1, 4, 0]:
                            WriteLine("under development... :D");
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        default:
                            Write("under mukbean's control, stay away 💤"+asciiArts[3]);
                            Thread.Sleep(3000);
                            throw new ArgumentOutOfRangeException();
                    }
                }
                //better write a custom error warning instead of heavily depend on try catch
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                    Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
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
