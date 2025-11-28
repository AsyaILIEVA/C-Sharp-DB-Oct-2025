using P03_SalesDatabase.Data;

namespace P03_SalesDatabase
{
    public class StartUp
    {
        static void Main()
        {
            SalesContext context = new SalesContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}