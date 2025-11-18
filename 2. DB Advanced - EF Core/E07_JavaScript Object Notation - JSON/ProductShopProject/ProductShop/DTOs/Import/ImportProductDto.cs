using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Import
{
    public class ImportProductDto
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("SellerId")]
        public string SellerId { get; set; } = null!;

        [JsonProperty("BuyerId")]
        public int? BuyerId { get; set; }

        
    }
}
