using OfficeOpenXml;
using static System.Console;


namespace InstantWording
{
    public class Program(MenuManager menuManager)
    {
        private readonly MenuManager _menuManager = menuManager;

        static void Main()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var repo = new RepositoryWord();
            var menuManager = new MenuManager(repo);
            var program = new Program(menuManager);
            program.Execute();
        }

        public void Execute()
        {
            _menuManager.Create();

        }
    }
}
