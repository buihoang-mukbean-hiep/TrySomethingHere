using static System.Console;
class Program
{
    static void Main()
    {
        TimeSpan check = DateTime.Now.Date- DateTime.Parse("11/9/2024 5:32:02").Date;
        int a = 0;
        _ = a == 0 ? a++ : a--;
        Console.WriteLine(a);
    }
}
