using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext dbContext = new ProductShopContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            string jsonFileDirPath = Path
                .Combine(Directory.GetCurrentDirectory(), "../../../Datasets/");
            string jsonFileName = "users.json";
            string jsonFileText = File
                .ReadAllText(jsonFileDirPath + jsonFileName);

            //Console.WriteLine(jsonFileDirPath);

            string result = ImportUsers(dbContext, jsonFileText);
            Console.WriteLine(result);
        }

        // P01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ICollection<User> usersToImport = new List<User>();

            IEnumerable<ImportUserDto>? userDtos = JsonConvert
                .DeserializeObject<ImportUserDto[]>(inputJson);

            if (userDtos != null)
            {
                foreach (ImportUserDto userDto in userDtos)
                {
                    if (!IsValid(userDto))
                    {
                        continue; // skip invalid users
                    }

                    User user = new User
                    {
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Age = userDto.Age
                    };

                    usersToImport.Add(user);
                }

                context.Users.AddRange(usersToImport);
                context.SaveChanges();
            }

            return $"Successfully imported {usersToImport.Count}";
        }


        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator
                .TryValidateObject(obj, validationContext, validationResults, true);
        }

    }
}