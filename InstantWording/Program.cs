using OfficeOpenXml;
namespace InstantWording
{
    public class Program(MenuManager menuManager)
    {
        private readonly MenuManager _menuManager = menuManager;

        static async Task Main()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var repo = new RepositoryWord();
            var menuManager = new MenuManager(repo);
            
            var program = new Program(menuManager);
            await program.Execute();
        }

        public async Task Execute()
        {
            await _menuManager.Create();
        }
    }
}
