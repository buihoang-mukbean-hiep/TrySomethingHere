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
                Write(pointer == 0 ?
                    menuBuilder.AppendLine("***********INSTANT_WORDING***********")
                        .Append(BuildMenu(max, menuBehavior)) : String.Empty);
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
                                                 .Append(BuildMenu(max, subMenuSource)));
                            pointer++; Reset(ref max, ref menuBuilder);
                            break;
                        case [1, 1, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                             .Append($"{BuildChosenNode(ref max, subMenuSource, choiceBuilder[1])}")
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
                        case [1, 2, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                      .Append($"{BuildChosenNode(ref max, subMenuSource, choiceBuilder[1])}")
                                      .Append("🧷file path: "));
                            await _repositoryWord.ReadAsync(ReadLine() ??
                                throw new ArgumentNullException(null, "❗can't not be null"));
                            //consider a loading bar??
                            WriteLine("✅loading successfully!");
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
                                          .Append(BuildMenu(max, subMenuShow)));
                            pointer++;
                            Reset(ref max, ref menuBuilder);
                            break;
                        case [3, 1, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                      .Append($"{BuildChosenNode(ref max, subMenuShow, choiceBuilder[1])}"));
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
                        case [5, 0, 0]:
                            WriteLine(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                          .Append(BuildMenu(max, subMenuModify)));
                            pointer++;
                            Reset(ref max, ref menuBuilder);
                            break;
                        case [5, 1, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                      .Append($"{BuildChosenNode(ref max, subMenuModify, choiceBuilder[1])}"));

                            _repositoryWord.Get();
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [5, 2, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                      .Append($"{BuildChosenNode(ref max, subMenuModify, choiceBuilder[1])}"));

                            _repositoryWord.Get();
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [5, 3, 0]:

                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                      .Append($"{BuildChosenNode(ref max, subMenuModify, choiceBuilder[1])}"));

                            _repositoryWord.Get();
                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [6, 0, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}"));

                            sw = Stopwatch.StartNew();
                            await _repositoryWord.WriteAsync();
                            sw.Stop();
                            WriteLine($"Time 4 write to file: {sw.ElapsedMilliseconds}ms");

                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [7, 0, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}"));

                            sw = Stopwatch.StartNew();
                            await _repositoryWord.WriteAsync();
                            sw.Stop();
                            WriteLine($"Time 4 write to file: {sw.ElapsedMilliseconds}ms");

                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        case [8, 0, 0]:
                            Write(menuBuilder.Append($"{BuildChosenNode(ref max, menuBehavior, choiceBuilder[0])}")
                                      .Append("🧷file path: "));

                            sw = Stopwatch.StartNew();
                            await _repositoryWord.ReadAsync(
                                ReadLine()?.Trim('"') ?? throw new ArgumentNullException());
                            sw.Stop();
                            WriteLine($"Time 4 loading: {sw.ElapsedMilliseconds}ms" +
                                $"\n✅loading successfully!");

                            Reset(ref max, ref menuBuilder, ref choiceBuilder, ref pointer);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(choiceBuilder), "❗out of choice's range");
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
