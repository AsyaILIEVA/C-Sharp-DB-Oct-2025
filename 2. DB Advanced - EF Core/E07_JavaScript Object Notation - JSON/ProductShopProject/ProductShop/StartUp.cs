using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Linq;

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
            string jsonFileName = "categories-products.json";
            string jsonFileText = File
                .ReadAllText(jsonFileDirPath + jsonFileName);

            //Console.WriteLine(jsonFileDirPath);

            //string result = ImportUsers(dbContext, jsonFileText);
            //Console.WriteLine(result);

            //string result = ImportProducts(dbContext, jsonFileText);
            //Console.WriteLine(result);

            //string result = ImportCategories(dbContext, jsonFileText);
            //Console.WriteLine(result);

            //string result = ImportCategoryProducts(dbContext, jsonFileText);
            //Console.WriteLine(result);

            //string result = GetProductsInRange(dbContext);
            //Console.WriteLine(result);

            string result = GetSoldProducts(dbContext);
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

        // P02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ICollection<Product> productsToImport = new List<Product>();

            IEnumerable<ImportProductDto>? productDtos = JsonConvert
                .DeserializeObject<ImportProductDto[]>(inputJson);
            if (productDtos != null)
            {
                foreach (ImportProductDto productDto in productDtos)
                {
                    if (!IsValid(productDto))
                    {
                        continue;
                    }

                    bool isSellerIdValid = int
                       .TryParse(productDto.SellerId, out int sellerId);
                    if (!isSellerIdValid)
                    {
                        continue;
                    }

                    Product product = new Product()
                    {
                        Name = productDto.Name,
                        Price = productDto.Price,
                        SellerId = sellerId,
                        BuyerId = productDto.BuyerId,
                    };

                    productsToImport.Add(product);                                        
                }
                context.Products.AddRange(productsToImport);
                context.SaveChanges();
            }

            return $"Successfully imported {productsToImport.Count}";            
        }

        // P03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ICollection<Category> categoriesToImport = new List<Category>();

            IEnumerable<ImportCategoryDto>? categoryDtos = JsonConvert
                .DeserializeObject<ImportCategoryDto[]>(inputJson);
            if (categoryDtos != null)
            {
                foreach (ImportCategoryDto categoryDto in categoryDtos)
                {
                    if (!IsValid(categoryDto))
                    {
                        continue;
                    }

                    if (categoryDto.Name == null)
                    {
                        continue;
                    }

                    Category category = new Category()
                    {
                        Name = categoryDto.Name,
                    };

                    categoriesToImport.Add(category);
                }

                context.AddRange(categoriesToImport);
                context.SaveChanges();
            }

            return $"Successfully imported {categoriesToImport.Count}";
        }

        //P04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ICollection<CategoryProduct> categoryProductsToImport
                = new List<CategoryProduct>();

            IEnumerable<int> existingCategoryIds = context
                .Categories
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArray();
            IEnumerable<int> existingProductIds = context
                .Products
                .AsNoTracking()
                .Select(p => p.Id)
                .ToArray();

            ImportCategoryProductDto[]? importCategoryProductDtos = JsonConvert
                .DeserializeObject<ImportCategoryProductDto[]>(inputJson);
            if (importCategoryProductDtos != null)
            {
                foreach (ImportCategoryProductDto cpDto in importCategoryProductDtos)
                {
                    if (!IsValid(cpDto))
                    {
                        continue;
                    }

                    CategoryProduct newCategoryProduct = new CategoryProduct()
                    {
                        CategoryId = cpDto.CategoryId,
                        ProductId = cpDto.ProductId
                    };

                    categoryProductsToImport.Add(newCategoryProduct);
                }

                context.AddRange(categoryProductsToImport);
                context.SaveChanges();
            }

            return $"Successfully imported {categoryProductsToImport.Count}";
        }

        //P05
        public static string GetProductsInRange(ProductShopContext context)
        // Get all products in a specified price range:  500 to 1000 (inclusive).
        // Order them by price(from lowest to highest).
        // Select only the product name, price and the full name of the seller.
        // Export the result to JSON.
        {
            var productsInRange = context
                .Products
                .AsNoTracking()
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new 
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName == null
            ? " " + p.Seller.LastName
            : p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToArray();


            string jsonResult = JsonConvert
                .SerializeObject(productsInRange, Formatting.Indented);
            return jsonResult;
        }

        //P06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                        .ToArray()
                })
                .ToArray();

            string jsonResult = JsonConvert
                .SerializeObject(users, Formatting.Indented);
            return jsonResult;
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