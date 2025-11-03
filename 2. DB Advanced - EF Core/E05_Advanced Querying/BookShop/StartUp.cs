namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
        }
            // Problem 02 
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            bool isCommandValid = Enum
                .TryParse(command, true, out AgeRestriction ageRestriction);

            if (isCommandValid)
            {
                IEnumerable<string> bookTitles = context
                    .Books
                    .AsNoTracking()
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();

                foreach (string title in bookTitles)
                {
                    sb.AppendLine(title);
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}



