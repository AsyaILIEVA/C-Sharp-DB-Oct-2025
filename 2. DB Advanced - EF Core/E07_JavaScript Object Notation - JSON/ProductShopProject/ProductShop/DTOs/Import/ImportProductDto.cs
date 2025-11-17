using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Import
{
    public class ImportProductDto
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("sellerId")]
        public string SellerId { get; set; } = null!;

        [JsonProperty("buyerId")]
        public int? BuyerId { get; set; }

        
    }
}
