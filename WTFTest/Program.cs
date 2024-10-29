using static System.Console;

#region all about null
static void NullableTest()
{
    //Nullable<int> a = null;
    //int? b = null;
    //string? c = null;
    //var s = new string('-', 20);
    User? user = null;
    WriteLine($"Name: {user?.Name?.FirstName}");
    var c = new { Name = "Hoang", Age = 10 };
    var d = new { Name = "Hoang", Age = 10.2 };
    var e = new { Age = 10, Name = "Hoang", };
    //Console.WriteLine(c == d); // Không compile, báo lỗi
    //Console.WriteLine(c == e);
}
#endregion

#region using stopwatch
//var sw = Stopwatch.StartNew();
//sw.Stop();
//Console.WriteLine($"Time: {sw.ElapsedMilliseconds}"); 
#endregion

#region using tuple
List<Tuple<Name, string>> UsingTuple()
{
    var stu = new List<User>().Select(s => new Tuple<Name, string>(s.Name, s.Age));
    return stu.ToList();
}
#endregion

#region out vs. ref
string init = "";
static void InitializeOutStr(out string str)
{
    str = "out test";
    WriteLine(str);
}
static void InitializeRefStr(ref string str)
{
    WriteLine(str);
    str = "change ref test";
}

InitializeOutStr(out init);
WriteLine(init);

string inputRefStr = "test";
InitializeRefStr(ref inputRefStr);
WriteLine(inputRefStr);
#endregion

//Console.OutputEncoding = Encoding.Unicode;
//string wtf = "美しいと\nうるさい！\\";
//WriteLine( wtf );

//string s = @"abc \n \t spacethentab".ToUpper();
//string s = "abc".ToUpper();
//string s = $"{"abc".ToUpper}"; // => string s = $"{"abc".ToUpper()}";
//string s = $"abc".ToUpper();
//string s = "abc.ToUpper()";
//WriteLine( s );


class User
{
    public Name? Name { get; set; }
    public string? Age { get; set; }
}
class Name
{
    public String? FirstName { get; set; }
    public string? LastName { get; set; }
}

