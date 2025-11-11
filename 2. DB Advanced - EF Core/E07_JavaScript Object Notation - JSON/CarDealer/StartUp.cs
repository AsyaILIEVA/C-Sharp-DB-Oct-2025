using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext dbContext = new CarDealerContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            // Console.WriteLine(Directory.GetCurrentDirectory());
            string jsonFileDirPath = Path
                .Combine(Directory.GetCurrentDirectory(), "../../../Datasets/");
            string jsonFileName = "parts.json";
            string jsonFileText = File
                .ReadAllText(jsonFileDirPath + jsonFileName);

            string result = ImportParts(dbContext, jsonFileText);
            Console.WriteLine(result);
        }

        //Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ICollection<Supplier> suppliersToImport = new List<Supplier>();

            IEnumerable<ImportSupplierDto>? supplierDtos = JsonConvert
                .DeserializeObject<ImportSupplierDto[]>(inputJson);
            if (supplierDtos != null)
            {
                foreach (ImportSupplierDto supplierDto in supplierDtos)
                {
                    if (!IsValid(supplierDto))
                    {
                        continue;
                    }

                    bool isImporterValidVal=bool
                        .TryParse(supplierDto.IsImporter, out bool isImporter);
                    if (!isImporterValidVal)
                    {
                        continue;
                    }
                    Supplier newSupplier = new Supplier()
                    {
                        Name = supplierDto.Name,
                        IsImporter = isImporter
                    };
                    suppliersToImport.Add(newSupplier);
                }

                context.Suppliers.AddRange(suppliersToImport);
                context.SaveChanges();
            }

            return $"Successfully imported {suppliersToImport.Count}.";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ICollection<Part> partsToImport = new List<Part>();
            //Choose single query + stroe in memory, when suppliers count is small
            ICollection<int> existingSupppliers = context
                .Suppliers
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            IEnumerable<ImportPartDto>? partDtos = JsonConvert
                .DeserializeObject<ImportPartDto[]>(inputJson);
            if (partDtos != null)
            {
                foreach (ImportPartDto partDto in partDtos)
                {
                    //First validate, then insert
                    if (!IsValid(partDto))
                    {
                        continue;
                    }

                    bool isSupplierIdValid = int
                        .TryParse(partDto.SupplierId, out int supplierId);
                    if (!isSupplierIdValid ||
                        !existingSupppliers.Contains(supplierId)) 
                    {
                        continue;
                    }

                    Part newPart = new Part()
                    {
                        Name = partDto.Name,
                        Price = partDto.Price,
                        Quantity = partDto.Quantity,
                        SupplierId = supplierId,
                    };
                    partsToImport.Add(newPart);
                }

                context.Parts.AddRange(partsToImport);
                context.SaveChanges();
            }
            return $"Successfully imported {partsToImport.Count}.";
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> validationResults 
                = new List<ValidationResult>();

            return Validator
                .TryValidateObject(obj, validationContext, validationResults);
        }
    }
}