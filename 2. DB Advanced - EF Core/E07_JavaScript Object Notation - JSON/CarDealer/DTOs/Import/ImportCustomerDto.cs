using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class ImportCustomerDto
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("birthDate")]
        public string Birthdate { get; set; } = null!;

        [Required]
        [JsonProperty("isYoungDriver")]
        public string IsYoungDriver { get; set; } = null!;
    }
}
