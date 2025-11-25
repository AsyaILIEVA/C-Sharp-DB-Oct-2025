using P01_StudentSystem.Data;


namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            try
            {
                using StudentSystemContext context = new StudentSystemContext();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Console.WriteLine("Database created!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}