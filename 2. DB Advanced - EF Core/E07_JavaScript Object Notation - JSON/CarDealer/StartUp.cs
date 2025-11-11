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
            string jsonFileName = "cars.json";
            string jsonFileText = File
                .ReadAllText(jsonFileDirPath + jsonFileName);

            string result = ImportCars(dbContext, jsonFileText);
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

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            ICollection<Car> carsToImport = new List<Car>();
            ICollection<PartCar> partsCarsToImport = new List<PartCar>();

            IEnumerable<ImportCarDto>? carDtos = JsonConvert
                .DeserializeObject<ImportCarDto[]>(inputJson);
            if (carDtos != null)
            {
                foreach (ImportCarDto carDto in carDtos)
                {
                    if (!IsValid(carDto))
                    {
                        continue;
                    }

                    Car newCar = new Car()
                    {
                        Make = carDto.Make,
                        Model = carDto.Model,
                        TraveledDistance = carDto.TraveledDistance
                    };
                    carsToImport.Add(newCar);

                    foreach(int partId in carDto.PartsIds.Distinct())
                    {
                        if (!context.Parts.Any(p => p.Id == partId))
                        {
                            continue;
                        }

                        PartCar newPartCar = new PartCar()
                        {
                            PartId = partId,
                            Car = newCar
                        };
                        partsCarsToImport.Add(newPartCar);
                    }

                }
                context.Cars.AddRange(carsToImport);
                context.PartsCars.AddRange(partsCarsToImport);

                context.SaveChanges();
            }

            return $"Successfully imported {carsToImport.Count}.";
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